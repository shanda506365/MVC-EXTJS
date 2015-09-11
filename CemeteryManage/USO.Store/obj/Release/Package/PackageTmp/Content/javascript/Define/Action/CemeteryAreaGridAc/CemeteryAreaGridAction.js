Ext.define('chl.Action.CemeteryAreaGridAction', {
    extend: 'WS.action.Base',
    category: 'CemeteryAreaGridAction'
});

Ext.create('chl.Action.CemeteryAreaGridAction', {
    itemId: 'addCemeteryArea',
    iconCls: 'add',
    tooltip: '添加区域',
    text: '添加区域',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        ActionManager.addCemeteryArea(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(!GlobalFun.IsAllowFun('墓碑区域添加'));
    }
});

Ext.create('chl.Action.CemeteryAreaGridAction', {
    itemId: 'addTombstoneRowList',
    iconCls: 'add',
    tooltip: '批量添加墓碑',
    text: '批量添加墓碑',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        ActionManager.addTombstoneRowList(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1 || !GlobalFun.IsAllowFun('墓碑添加'));
    }
});

Ext.create('chl.Action.CemeteryAreaGridAction', {
    itemId: 'editCemeteryArea',
    iconCls: 'edit',
    tooltip: '编辑',
    text: '编辑',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        var record = target.getSelectionModel().getSelection()[0];
        ActionManager.editCemeteryArea(target, record);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1 || !GlobalFun.IsAllowFun('墓碑区域编辑'));
    }
});


Ext.create('chl.Action.CemeteryAreaGridAction', {
    itemId: 'delCemeteryArea',
    iconCls: 'delete',
    tooltip: '删除',
    text: '删除',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.delCemeteryArea(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length == 0 || !GlobalFun.IsAllowFun('墓碑区域删除'));
    }
});

Ext.create('chl.Action.CemeteryAreaGridAction', {
    itemId: 'refreshCemeteryArea',
    iconCls: 'refresh',
    tooltip: '刷新',
    text: '刷新',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.refreshCemeteryArea(target);
    },
    updateStatus: function (selection) {
    }
});



//新建墓碑区域
ActionManager.addCemeteryArea = function (traget) {
    WindowManager.AddUpdateCemeteryAreaWin = Ext.create('chl.Grid.AddUpdateCemeteryAreaWin', {
        grid: traget,
        iconCls: 'add',
        action: 'create',
        record: false,
        title: "新建墓碑区域"
    });
    WindowManager.AddUpdateCemeteryAreaWin.show(null, function () { });
};
//批量新建墓碑
ActionManager.addTombstoneRowList = function (target) {
    var records = target.getSelectionModel().getSelection();
    if (!records[0])
        return;
    WindowManager.AddTombstoneRowListWin = Ext.create('Ext.window.Window', {
        grid: target,
        iconCls: 'add',
        action: 'create',
        record: false,
        title: "新建墓碑",
        defaultFocus: 'NameItemId',
        height: 300,
        width: 500,
        layout: 'vbox',
        modal: true,
        resizable: false,
        items: [{
            xtype: 'form',
            itemId: 'formId',
            autoScroll: true,
            height: 450,
            width: 490,
            border: false,
            bodyPadding: 5,
            layout: {
                type: 'table',
                columns: 2
            },
            defaults: {
                xtype: 'textfield',
                labelAlign: 'right',
                labelPad: 15,
                width: 340,
                labelWidth: 125,
                maxLength: 100,
                maxLengthText: '最大长度为100'
            },
            items: [{
                xtype: 'displayfield',
                colspan: 2,
                width: 400,
                eidtable:false,
                fieldLabel: '墓碑区域',
                //fieldStyle: GlobalConfig.Css.RemarkReadOnlyDisplay,
                value: records[0].data.Name,
                itemId: 'AreaItemId'
            }, {
                fieldLabel: '排',
                xtype: 'combobox',
                editable: false,
                colspan: 2,
                name: 'RowId',
                itemId: 'RowIdItemId',
                store: 'RowStoreId',
                queryMode: 'local',
                displayField: 'Name',
                valueField: 'Id',
                allowBlank: false,
                blankText: '不能为空',
                listeners: {
                    boxready: function (com) {
                        var w = com.up('window');
                        GlobalFun.comboSelectFirstOrDefaultVal(com);
                    }
                }
            }, {
                width: 150,
                xtype: 'displayfield',
                fieldLabel: '已有个数',
                itemId: 'HadCountItemId',
                validateOnBlur: false,
                value: records[0].data.TotalCount
            }, {
                width: 150,
                labelWidth: 60,
                fieldLabel: '添加个数',
                name: 'count',
                itemId: 'CountItemId',
                validateOnBlur: false,
                allowBlank: false,
                blankText: '个数不能为空',
                regex: GlobalConfig.RegexController.regexNumber,
                regexText: '请输入数字'
            }, {
                fieldLabel: '墓碑类别',
                width: 300,
                colspan: 2,
                xtype: 'combobox',
                editable: false,
                name: 'TypeId',
                itemId: 'TypeIdItemId',
                store: 'TombstoneTypeStoreId',
                queryMode: 'local',
                displayField: 'Name',
                valueField: 'Id',
                allowBlank: false,
                blankText: '不能为空',
                listeners: {
                    boxready: function (com) {
                        var w = com.up('window');
                        GlobalFun.comboSelectFirstOrDefaultVal(com);
                    }
                }
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
                var w = me.up('window');

                var form = w.down('#formId').getForm();

                //var ids = [];
                //Ext.Array.each(records, function (rec) {
                //    ids.push(rec.data.Id);
                //});
                if (form.isValid()) {
                    var url = GlobalConfig.Controllers.TombstoneGrid.addTombstoneRowList;
                    form.submit({
                        url: url,
                        params: {
                            req: 'dataset',
                            dataname: 'SelectCustomer', // dataset名称，根据实际情况设置,数据库名
                            restype: 'json',
                            Id: records[0].data.Id,
                            action: w.action,
                            sessiontoken: GlobalFun.getSeesionToken()

                        },
                        success: function (form, action) {
                            w.grid.loadGrid();
                            w.close();

                        },
                        failure: function (form, action) {
                            if (!GlobalFun.errorProcess(action.result.code)) {
                                Ext.Msg.alert('失败', action.result.msg);
                            }
                        }

                    });
                }
            }
        }, {
            text: '取消',
            handler: function () {
                var me = this;
                me.up('window').close();
            }
        }]
    });
    WindowManager.AddTombstoneRowListWin.show();
};
//编辑墓碑区域
ActionManager.editCemeteryArea = function (traget, record) {
    WindowManager.AddUpdateCemeteryAreaWin = Ext.create('chl.Grid.AddUpdateCemeteryAreaWin', {
        grid: traget,
        iconCls: 'edit',
        record: record,
        action: 'update',
        title: "编辑墓碑区域"
    });

    WindowManager.AddUpdateCemeteryAreaWin.show(null, function () {
        WindowManager.AddUpdateCemeteryAreaWin.down("#formId").loadRecord(record);
    });
};
//删除墓碑区域
ActionManager.delCemeteryArea = function (traget) {
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
        msg: '删除区域后将自动删除该区域下的所有墓碑及信息，您确定要删除选定的墓碑区域吗？',
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
//刷新墓碑区域
ActionManager.refreshCemeteryArea = function (traget) {
    traget.loadGrid();
};
