Ext.define('chl.Action.UserGridAction', {
    extend: 'WS.action.Base',
    category: 'UserGridAction'
});

Ext.create('chl.Action.UserGridAction', {
    itemId: 'addUser',
    iconCls: 'add',
    tooltip: '添加',
    text: '添加',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        StoreManager.ComboStore.RoleStore.load({
            callback: function (records, operation, success) {
                ActionManager.addUser(target);
            }
        });
        
    },
    updateStatus: function (selection) {
        this.setDisabled(!GlobalFun.IsAllowFun('用户添加'));
    }
});

Ext.create('chl.Action.UserGridAction', {
    itemId: 'editUser',
    iconCls: 'edit',
    tooltip: '编辑',
    text: '编辑',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        var record = target.getSelectionModel().getSelection()[0];
        StoreManager.ComboStore.RoleStore.load({
            callback: function (records, operation, success) {
                ActionManager.editUser(target, record);
            }
        });
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1 || !GlobalFun.IsAllowFun('用户编辑'));
    }
});


Ext.create('chl.Action.UserGridAction', {
    itemId: 'delUser',
    iconCls: 'delete',
    tooltip: '删除',
    text: '删除',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.delUser(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length == 0 || !GlobalFun.IsAllowFun('用户删除'));
    }
});

Ext.create('chl.Action.UserGridAction', {
    itemId: 'refreshUser',
    iconCls: 'refresh',
    tooltip: '刷新',
    text: '刷新',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.refreshUser(target);
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.UserGridAction', {
    itemId: 'searchUser',
    iconCls: 'search',
    tooltip: '查询',
    text: '查询',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.searchUser(target);
    },
    updateStatus: function (selection) {
    }
});

//新建用户
ActionManager.addUser = function (traget) {
    WindowManager.AddUpdateUserWin = Ext.create('chl.Grid.AddUpdateUserWin', {
        grid: traget,
        iconCls: 'add',
        action: 'create',
        record: false,
        title: "新建用户"
    });
    //ifEditorPass 隐藏
    //oldPassword 隐藏
    //newPassword 改文本 setDisabled 
    //repeatPassword
    WindowManager.AddUpdateUserWin.show(null, function () {
        WindowManager.AddUpdateUserWin.down("#ifEditorPass").hide();
        WindowManager.AddUpdateUserWin.down("#oldPassword").hide();
        var newPassword = WindowManager.AddUpdateUserWin.down("#newPassword");
        newPassword.setFieldLabel("密码");
        newPassword.setDisabled(false);
        var repeatPassword = WindowManager.AddUpdateUserWin.down("#repeatPassword");
        repeatPassword.setFieldLabel("确认密码");
        repeatPassword.setDisabled(false);
    });
};
//编辑用户
ActionManager.editUser = function (traget, record) {
    WindowManager.AddUpdateUserWin = Ext.create('chl.Grid.AddUpdateUserWin', {
        grid: traget,
        iconCls: 'edit',
        record: record,
        action: 'update',
        title: "编辑用户"
    });
    WindowManager.AddUpdateUserWin.show(null, function () {
        WindowManager.AddUpdateUserWin.down("#formId").loadRecord(record);
        //ifEditorPass 显示
        //oldPassword 显示
        //newPassword 改文本 setDisabled
        //repeatPassword
        WindowManager.AddUpdateUserWin.down("#ifEditorPass").show();
        WindowManager.AddUpdateUserWin.down("#oldPassword").show();
        var newPassword = WindowManager.AddUpdateUserWin.down("#newPassword");
        newPassword.setFieldLabel("新密码");
        newPassword.setDisabled(true);
        var repeatPassword = WindowManager.AddUpdateUserWin.down("#repeatPassword");
        repeatPassword.setFieldLabel("确认新密码");
        repeatPassword.setDisabled(true);
    });
};
//删除用户
ActionManager.delUser = function (traget) {
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
//刷新用户
ActionManager.refreshUser = function (traget) {
    traget.loadGrid();
};

//用户查询
ActionManager.searchUser = function (traget) {
    if (WindowManager.searchUserWin && WindowManager.searchUserWin != '') {
        WindowManager.searchUserWin.show();
    } else {
        WindowManager.searchUserWin = Ext.create('Ext.window.Window', {
            modal: true,
            resizable: false,
            closeAction: 'hide',
            title: "用户查询",
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