Ext.define('chl.Action.RoleGridAction', {
    extend: 'WS.action.Base',
    category: 'RoleGridAction'
});

Ext.create('chl.Action.RoleGridAction', {
    itemId: 'addRole',
    iconCls: 'add',
    tooltip: '添加',
    text: '添加',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        ActionManager.addRole(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(!GlobalFun.IsAllowFun('角色添加'));
    }
});

Ext.create('chl.Action.RoleGridAction', {
    itemId: 'editRole',
    iconCls: 'edit',
    tooltip: '编辑',
    text: '编辑',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        var record = target.getSelectionModel().getSelection()[0];
        ActionManager.editRole(target, record);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1 || !GlobalFun.IsAllowFun('角色编辑'));
    }
});


Ext.create('chl.Action.RoleGridAction', {
    itemId: 'delRole',
    iconCls: 'delete',
    tooltip: '删除',
    text: '删除',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.delRole(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length == 0 || !GlobalFun.IsAllowFun('角色删除'));
    }
});

Ext.create('chl.Action.RoleGridAction', {
    itemId: 'refreshRole',
    iconCls: 'refresh',
    tooltip: '刷新',
    text: '刷新',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.refreshRole(target);
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.RoleGridAction', {
    itemId: 'searchRole',
    iconCls: 'search',
    tooltip: '查询',
    text: '查询',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.searchRole(target);
    },
    updateStatus: function (selection) {
    }
});

//新建角色
ActionManager.addRole = function (traget) {
    WindowManager.AddUpdateRoleWin = Ext.create('chl.Grid.AddUpdateRoleWin', {
        grid: traget,
        iconCls: 'add',
        action: 'create',
        record: false,
        title: "新建角色"
    });

    //获取相应的功能
    //调用接口
    //调用
    var param = {};
    param.sessiontoken = Ext.util.Cookies.get("sessiontoken");
    WsCall.call(GlobalConfig.Controllers.RoleGrid.LoadFunctions, 'LoadFunctions', param, function (response, opts) {
        var data = response.dataset;
        WindowManager.AddUpdateRoleWin.show(null, function () {
            ActionManager.addUpdateRoleLoadFunction(data,true);
        });
    }, function (response, opts) {
        if (!GlobalFun.errorProcess(response.code)) {
            Ext.Msg.alert('失败', response.msg);
        }
    }, true, "请稍候...", Ext.getBody());


};
//编辑角色
ActionManager.editRole = function (traget, record) {
    WindowManager.AddUpdateRoleWin = Ext.create('chl.Grid.AddUpdateRoleWin', {
        grid: traget,
        iconCls: 'edit',
        record: record,
        action: 'update',
        title: "编辑角色"
    });
    //获取相应的功能
    //调用接口
    //调用
    var param = {};
    param.sessiontoken = Ext.util.Cookies.get("sessiontoken");
    WsCall.call(GlobalConfig.Controllers.RoleGrid.LoadFunctions, 'LoadFunctions', param, function (response, opts) {
        var data = response.dataset;
        WindowManager.AddUpdateRoleWin.show(null, function () {
            WindowManager.AddUpdateRoleWin.down("#formId").loadRecord(record);
            ActionManager.addUpdateRoleLoadFunction(data);
            //var functionItemContains = WindowManager.AddUpdateRoleWin.down("#functionItemContainer");
            //Ext.each(data, function (item, index, alls) {
            //    functionItemContains.add({
            //        itemId: 'functionItemId_' + item.Id,
            //        boxLabel: item.Name,
            //        submitValue: false
            //    });
            //});
            var functions = record.data.FunctionDtos;
            Ext.each(functions, function (item, index) {
                var field = WindowManager.AddUpdateRoleWin.down("#functionItemId_" + item.Id);
                field.setValue(true);
                if (item.ParentId == 0) {
                    var fieldset = field.up('fieldset');
                    fieldset.fistCheck = false;
                }
            });

        });
    }, function (response, opts) {
        if (!GlobalFun.errorProcess(response.code)) {
            Ext.Msg.alert('失败', response.msg);
        }
    }, true, "请稍候...", Ext.getBody());
};
//删除角色
ActionManager.delRole = function (traget) {
    var sm = traget.getSelectionModel();
    var records = sm.getSelection();
    if (!records[0])
        return;
    var ids = [];
    Ext.Array.each(records, function (rec) {
        ids.push(rec.data.Id);
    });
    var store = traget.getStore();
    store.getProxy().extraParams.idList = ids.join();
    GlobalConfig.newMessageBox.show({
        title: '提示',
        msg: '您确定要删除选定的项目吗？',
        buttons: Ext.MessageBox.YESNO,
        closable: false,
        fn: function (btn) {
            if (btn == 'yes') {
                store.remove(records);
                store.sync();
                store.getProxy().extraParams.idList = '';
                for (var i = 0; i < records.length; i++) {
                    records[i].commit(true);			//将之前选中的数据设置脏数据为false
                }
                (new Ext.util.DelayedTask(function () {
                    store.load();
                })).delay(500);
            }
        },
        icon: Ext.MessageBox.QUESTION
    });
};
//刷新角色
ActionManager.refreshRole = function (traget) {
    traget.loadGrid();
};

//角色查询
ActionManager.searchRole = function (traget) {
    if (WindowManager.AddUpdateRoleWin && WindowManager.AddUpdateRoleWin != '') {
        WindowManager.AddUpdateRoleWin.show();
    } else {
        WindowManager.AddUpdateRoleWin = Ext.create('Ext.window.Window', {
            modal: true,
            resizable: false,
            closeAction: 'hide',
            title: "角色查询",
            defaultFocus: 'NameId',
            iconCls: '',
            record: false,
            height: 500,
            width: 500,
            layout: 'vbox',
            items: [{
                xtype: 'form',
                itemId: 'formId',
                autoScroll: true,
                height: 450,
                width: 490,
                border: false,
                bodyPadding: 5,
                defaults: {
                    xtype: 'fieldset',
                    collapsible: true,
                    defaultType: 'textfield',
                    defaults: {
                        labelAlign: 'right',
                        labelPad: 15,
                        width: 340,
                        labelWidth: 125,
                        maxLength: 100,
                        maxLengthText: '最大长度为100'
                    }
                },
                items: [{
                    title: '信息',
                    items: [{
                        name: 'Name',
                        width: 300,
                        colspan: 2,
                        fieldLabel: '名称',
                        itemId: 'NameItemId',
                        validateOnBlur: false,
                        allowBlank: false,
                        blankText: '名称不能为空'
                    }]
                }]
            }],
            buttons: [{
                text: '重置',
                handler: function () {
                    var me = this;
                    var w = me.up('window');
                    var f = w.down('#formId');
                    f.getForm().reset();
                }
            }, {
                text: '确定',
                itemId: 'submit',
                handler: function () {
                    var me = this;
                    var win = me.up('window');

                    var name = win.down('#NameItemId').getValue();
                    var searchFlag = false;
                    var store = traget.getStore();
                    if (name != '') {
                        //加入filterMap
                        if (!store.filterMap.containsKey('Name')) {
                            store.filterMap.add('Name', name);
                        } else {
                            store.filterMap.replace('Name', name);
                        }
                        searchFlag = true;
                    } else {
                        store.filterMap.removeAtKey('Name');
                    }

                    if (searchFlag) {
                        win.close();
                        traget.loadGrid(true);
                    } else {
                        win.close();
                        traget.loadGrid();
                    }

                }
            }, {
                text: '取消',
                handler: function () {
                    var me = this;
                    me.up('window').close();
                }
            }]
        }).show();
    }

};


//角色功能加载通用方法
ActionManager.addUpdateRoleLoadFunction = function (data,isAdd) {
    var functionItemContains = WindowManager.AddUpdateRoleWin.down("#functionItemContainer");
    var titleItems = [];
    Ext.each(data, function (item, index, alls) {
        if (item.ParentId == 0) {
            titleItems.push({
                xtype: 'fieldset',
                title: item.Name,
                myItemId: 'functionItemId_' + item.Id,
                checkboxToggle: true,
                onCheckChange: GlobalFun.onCheckChange,
                createCheckboxCmp: GlobalFun.createCheckboxCmp,
                width: 380,
                fistCheck: isAdd?false:true,
                layout: {
                    type: 'table',
                    columns: 3
                },
                defaults: {
                    xtype: 'checkbox',
                    labelAlign: 'right',
                    width: 120
                },
                items: []
            });
        }
    });
    Ext.each(titleItems, function (titleitem, index, alls) {
        var subItems = [];
        Ext.each(data, function (item, index, alls) {
            var starParentId = item.Code.substring(0, item.Code.indexOf('-'));
            if (item.parentId != 0 && starParentId == titleitem.myItemId.replace('functionItemId_', '')) {
                subItems.push({
                    itemId: 'functionItemId_' + item.Id,
                    boxLabel: item.Name,
                    submitValue: false
                });
            }
        });
        titleitem.items = subItems;
    });
    functionItemContains.add(titleItems);
}