Ext.define('chl.Action.TombstoneGridAction', {
    extend: 'WS.action.Base',
    category: 'TombstoneGridAction'
});

Ext.create('chl.Action.TombstoneGridAction', {
    itemId: 'addTombstone',
    iconCls: 'add',
    tooltip: '添加',
    text: '添加',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        ActionManager.addTombstone(target);
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.TombstoneGridAction', {
    itemId: 'editTombstone',
    iconCls: 'edit',
    tooltip: '编辑',
    text: '编辑',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        var record = target.getSelectionModel().getSelection()[0];
        ActionManager.editTombstone(target, record);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1);
    }
});


Ext.create('chl.Action.TombstoneGridAction', {
    itemId: 'delTombstone',
    iconCls: 'delete',
    tooltip: '删除',
    text: '删除',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.delTombstone(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length == 0);
    }
});

Ext.create('chl.Action.TombstoneGridAction', {
    itemId: 'refreshTombstone',
    iconCls: 'refresh',
    tooltip: '刷新',
    text: '刷新',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.refreshTombstone(target);
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.TombstoneGridAction', {
    itemId: 'sortTombstone',
    iconCls: 'sort',
    tooltip: '墓碑排序',
    text: '墓碑排序',
    handler: function () {
        ActionManager.sortTombstone();
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.TombstoneGridAction', {
    itemId: 'searchTombstone',
    iconCls: 'search',
    tooltip: '查询',
    text: '查询',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.searchTombstone(target);
    },
    updateStatus: function (selection) {
    }
});

//新建墓碑
ActionManager.addTombstone = function (traget) {
    WindowManager.AddUpdateTombstoneWin = Ext.create('chl.Grid.AddUpdateTombstoneWin', {
        grid: traget,
        iconCls: 'add',
        action: 'create',
        record: false,
        title: "新建墓碑"
    });
    WindowManager.AddUpdateTombstoneWin.show();
};
//编辑墓碑
ActionManager.editTombstone = function (traget, record) {
    WindowManager.AddUpdateTombstoneWin = Ext.create('chl.Grid.AddUpdateTombstoneWin', {
        grid: traget,
        iconCls: 'edit',
        record: record,
        action: 'update',
        title: "编辑墓碑"
    });
    WindowManager.AddUpdateTombstoneWin.show(null, function () {
        WindowManager.AddUpdateTombstoneWin.down("#formId").loadRecord(record);
        //为日期赋值
        WindowManager.AddUpdateTombstoneWin.down("#ExpiryDateID").setValue(record.data.ExpiryDate.replace(" ", "T"), true);
        WindowManager.AddUpdateTombstoneWin.down("#BuyDateID").setValue(record.data.BuyDate.replace(" ", "T"), true);
        WindowManager.AddUpdateTombstoneWin.down("#LastPaymentDateID").setValue(record.data.LastPaymentDate.replace(" ", "T"), true);
        WindowManager.AddUpdateTombstoneWin.down("#BuryDateID").setValue(record.data.BuryDate.replace(" ", "T"), true);
    });
};
//删除墓碑
ActionManager.delTombstone = function (traget) {
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
//刷新墓碑
ActionManager.refreshTombstone = function (traget) {
    traget.loadGrid();
};
//排序墓碑
ActionManager.sortTombstone = function () {
    var pngClass = Ext.create('MiniPngDDSortViewClass', {
        AreaId: 1,
        RowId: 1
    });
    var fullShowWin = Ext.create('Ext.window.Window', {
        title: '全屏查看',
        modal: true,
        iconCls: 'coverStamp',
        height: Ext.getBody().getHeight(),
        width: Ext.getBody().getWidth(),
        groupName: 'TombstoneDDGroup',
        pngClass: pngClass,
        items: [{
            xtype: 'MiniPngDDSortPanel',
            listeners: {
                boxready: function (compent) {
                    //pngClass.initPngPanel(compent);
                }
            }
        }],
        dockedItems: [{
            xtype: 'toolbar',
            dock: 'top',
            defaults: {
                labelAlign: 'right',
                width: 200,
                labelWidth: 40
            },
            items: [
                {
                    fieldLabel: '区域',
                    width: 200,
                    //colspan: 2,
                    xtype: 'combobox',
                    editable: false,
                    name: 'AreaId',
                    itemId: 'AreaIdItemId',
                    store: 'AreaStoreId',
                    queryMode: 'local',
                    displayField: 'Name',
                    valueField: 'Id',
                    allowBlank: false,
                    blankText: '不能为空',
                    listeners: {
                        boxready: function (com) {
                            var w = com.up('window');
                            var record = w.record;
                            if (record) {
                                GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.AreaId);
                            } else {
                                GlobalFun.comboSelectFirstOrDefaultVal(com);
                            }
                        }
                    }
                }, {
                    xtype: 'tbseparator',
                    width: 2
                },
                {
                    fieldLabel: '行',
                    xtype: 'combobox',
                    editable: false,
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
                            var record = w.record;
                            if (record) {
                                GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.RowId);
                            } else {
                                GlobalFun.comboSelectFirstOrDefaultVal(com);
                            }
                        }
                    }
                }, {
                    xtype: 'tbseparator',
                    width: 2
                }, {
                    xtype: 'button',
                    width: 80,
                    iconCls: 'search',
                    text: '展示',
                    handler: function () {
                        var area = fullShowWin.down('#AreaIdItemId');
                        var row = fullShowWin.down('#RowIdItemId');
                        if (area.isValid() && row.isValid()) {
                            pngClass.setAreaId(area.getValue());
                            pngClass.setRowId(row.getValue());
                            var miniPngDDSortPanel = fullShowWin.down('MiniPngDDSortPanel');
                            miniPngDDSortPanel.removeAll();
                            pngClass.initPngPanel(miniPngDDSortPanel);
                        }
                    }
                }
            ]
        }]
    }).show();
};
//墓碑查询
ActionManager.searchTombstone = function (traget) {
    if (WindowManager.AddUpdateTombstoneWin && WindowManager.AddUpdateTombstoneWin != '') {
        WindowManager.AddUpdateTombstoneWin.show();
    } else {
        WindowManager.AddUpdateTombstoneWin = Ext.create('Ext.window.Window', {
            modal: true,
            resizable: false,
            closeAction: 'hide',
            title: "墓碑查询",
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