Ext.define('chl.Action.CustomerGridAction', {
    extend: 'WS.action.Base',
    category: 'CustomerGridAction'
});

Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'addCustomer',
    iconCls: 'add',
    tooltip: '添加',
    text: '添加',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        ActionManager.addCustomer(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(!GlobalFun.IsAllowFun('客户添加'));
    }
});

Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'editCustomer',
    iconCls: 'edit',
    tooltip: '编辑',
    text: '编辑',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        var record = target.getSelectionModel().getSelection()[0];
        ActionManager.editCustomer(target, record);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1 || !GlobalFun.IsAllowFun('客户编辑'));
    }
});


Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'delCustomer',
    iconCls: 'delete',
    tooltip: '删除',
    text: '删除',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.delCustomer(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length == 0 || !GlobalFun.IsAllowFun('客户删除'));
    }
});

Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'refreshCustomer',
    iconCls: 'refresh',
    tooltip: '刷新',
    text: '刷新',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.refreshCustomer(target);
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'exportCustomer',
    iconCls: 'export',
    tooltip: '导出',
    text: '导出',
    handler: function () {
        var target = this.getTargetView();
        var store = target.getStore();
        var extraParams = store.getProxy().extraParams;
        var param = {
            downType: 'Customer',
            dir: 'ASC',
            sort: 'Id',
            filter: extraParams.filter,
            sessiontoken: GlobalFun.getSeesionToken()
        };
        WsCall.downloadFile(GlobalConfig.Controllers.CustomerGrid.outPutExcelCustomer, 'download', param);
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'importCustomer',
    iconCls: 'import',
    tooltip: '导入',
    text: '导入',
    handler: function () {
        var target = this.getTargetView();
        var win = Ext.create('chl.UniversalWindow.ImportDataWin', {
            grid: target,
            importType:'customer'
        });
        win.show();
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'searchCustomer',
    iconCls: 'search',
    tooltip: '查询',
    text: '查询',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.searchCustomer(target);
    },
    updateStatus: function (selection) {
    }
});

//Ext.create('chl.Action.CustomerGridAction', {
//    text: '全选/取消',
//    itemId: 'selectOrCancelAllCustomer',
//    handler: function () {
//        var target = this.getTargetView();
//        ActionManager.selectOrCancelAllRecord(target);
//    },
//    updateStatus: function (selection) {

//    }
//});
//Ext.create('chl.Action.CustomerGridAction', {
//    text: '反选',
//    itemId: 'reverseSelectCustomer',
//    handler: function () {
//        var target = this.getTargetView();
//        ActionManager.reverseSelectRecord(target);
//    },
//    updateStatus: function (selection) {

//    }
//});
//Ext.create('chl.Action.CustomerGridAction', {
//    text: '全部类别',
//    checked: true,
//    group: 'customerTypeGroup',
//    itemId: 'allCustomerType',
//    handler: function (btn, eve, suppressLoad) {
//        var panel = this.getTargetView();
//        var menu = panel.down('#menuID');
//        menu.setText('全部类别');
//        menu.setTooltip('全部类别');

//        panel.getStore().filterMap.removeAtKey('CustomerTypeId');
//        if (suppressLoad) {
//            this.setChecked(suppressLoad);
//            return;
//        }

//        panel.loadGrid();
//    },
//    updateStatus: function (selection) {

//    }
//});
//Ext.create('chl.Action.CustomerGridAction', {
//    text: '购买人',
//    group: 'customerTypeGroup',
//    itemId: 'GMRCustomerType',
//    handler: function (btn, eve, suppressLoad) {
//        var panel = this.getTargetView();
//        var menu = panel.down('#menuID');
//        menu.setText('购买人');
//        menu.setTooltip('购买人');

//        if (panel.getStore().filterMap.containsKey('CustomerTypeId')) {
//            panel.getStore().filterMap.add('CustomerTypeId', 1);
//        } else {
//            panel.getStore().filterMap.replace('CustomerTypeId', 1);
//        }

//        if (suppressLoad) {
//            this.setChecked(suppressLoad);
//            return;
//        }
//        panel.loadGrid();
//    },
//    updateStatus: function (selection) {

//    }
//});

//Ext.create('chl.Action.CustomerGridAction', {
//    text: '埋葬者',
//    group: 'customerTypeGroup',
//    itemId: 'MZZCustomerType',
//    handler: function (btn, eve, suppressLoad) {
//        var panel = this.getTargetView();
//        var menu = panel.down('#menuID');
//        menu.setText('购买人');
//        menu.setTooltip('购买人');

//        if (panel.getStore().filterMap.containsKey('CustomerTypeId')) {
//            panel.getStore().filterMap.add('CustomerTypeId', 2);
//        } else {
//            panel.getStore().filterMap.replace('CustomerTypeId', 2);
//        }

//        if (suppressLoad) {
//            this.setChecked(suppressLoad);
//            return;
//        }
//        panel.loadGrid();
//    },
//    updateStatus: function (selection) {

//    }
//});

//添加客户
ActionManager.addCustomer = function (traget) {
    WindowManager.AddUpdateCustomerWin = Ext.create('chl.Grid.AddUpdateCustomerWin', {
        grid: traget,
        iconCls: 'add',
        action: 'create',
        record: false,
        title: "新建客户"
    });
    WindowManager.AddUpdateCustomerWin.show();
};
//编辑客户
ActionManager.editCustomer = function (traget, record) {
    WindowManager.AddUpdateCustomerWin = Ext.create('chl.Grid.AddUpdateCustomerWin', {
        grid: traget,
        iconCls: 'edit',
        record: record,
        action: 'update',
        title: "编辑客户"
    });
    WindowManager.AddUpdateCustomerWin.show(null, function () {
        WindowManager.AddUpdateCustomerWin.down("#formId").loadRecord(record);
        //为日期赋值
        WindowManager.AddUpdateCustomerWin.down("#BuryDateId").setValue(record.data.BuryDate.replace(" ", "T"), true);
        WindowManager.AddUpdateCustomerWin.down("#DeathDateID").setValue(record.data.DeathDate.replace(" ", "T"), true);
    });
};
//删除客户
ActionManager.delCustomer = function (traget) {
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
//刷新客户表
ActionManager.refreshCustomer = function (traget) {
    traget.loadGrid();
};
//客户查询
ActionManager.searchCustomer = function (traget) {
    if (WindowManager.searchCustomerWin && WindowManager.searchCustomerWin != '') {
        WindowManager.searchCustomerWin.show();
    } else {
        WindowManager.searchCustomerWin = Ext.create('Ext.window.Window', {
            modal: true,
            resizable: false,
            closeAction: 'hide',
            title: "客户查询",
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
                    layout: {
                        type: 'table',
                        columns: 2
                    },
                    collapsible: true,
                    defaultType: 'textfield',
                    defaults: {
                        labelAlign: 'right',
                        labelPad: 15,
                        width: 340,
                        labelWidth: 100,
                        maxLength: 100,
                        maxLengthText: '最大长度为100'
                    }
                },
                items: [{
                    title: '信息',
                    items: [{
                        width: 300,
                        colspan: 2,
                        fieldLabel: '姓名',
                        itemId: 'FullNameItemId'
                    }, {
                        xtype: 'fieldcontainer',
                        colspan: 2,
                        width: 490,
                        fieldLabel: '下葬日期',
                        defaultType: 'datetimefield',
                        layout: {
                            type: 'hbox'
                        },
                        defaults: {
                            labelAlign: 'right',
                            width: 100
                        },
                        items: [{
                            xtype: 'datefield',
                            format: 'Y-m-d',
                            itemId: 'dateStar',
                            vtype: 'daterange',
                            endDateField: 'dateEnd',
                            editable: false
                        }, {
                            xtype: 'label',
                            margin: '0 0 0 5',
                            width: 20,
                            text: '至'
                        }, {
                            xtype: 'datefield',
                            format: 'Y-m-d',
                            itemId: 'dateEnd',
                            vtype: 'daterange',
                            startDateField: 'dateStar',
                            editable: false
                        }]
                    }, {
                        fieldLabel: '客户类别',
                        width: 200,
                        xtype: 'combobox',
                        editable: false,
                        disabled: true,
                        name: 'CustomerTypeId',
                        itemId: 'CustomerTypeIdItemId',
                        store: 'CustomerTypeStoreId',
                        queryMode: 'local',
                        displayField: 'Name',
                        valueField: 'Id',
                        listeners: {
                            boxready: function (com) {
                                var w = com.up('window');
                                GlobalFun.comboSelectFirstOrDefaultVal(com);
                            }
                        }
                    }, {
                        xtype: 'checkbox',
                        itemId: 'cbCustomerTypeIdItemId',
                        margin: '0 0 4 8',
                        checked: true,
                        boxLabel: '忽略',
                        listeners: {
                            change: function (cb, nValue, oValue, opts) {
                                var customerTypeIdItemId = cb.up('form').down('#CustomerTypeIdItemId');
                                customerTypeIdItemId.setDisabled(nValue);
                                if (!nValue) {
                                    GlobalFun.comboSelectFirstOrDefaultVal(customerTypeIdItemId);
                                }
                            }
                        }
                    } , {
                        fieldLabel: '国籍',
                        width: 200,
                        xtype: 'combobox',
                        editable: false,
                        disabled: true,
                        name: 'NationalityId',
                        itemId: 'NationalityIdItemId',
                        store: 'NationalityStoreId',
                        queryMode: 'local',
                        displayField: 'Name',
                        valueField: 'Id',
                        listeners: {
                            boxready: function (com) {
                                var w = com.up('window');
                                GlobalFun.comboSelectFirstOrDefaultVal(com);
                            }
                        }
                    }, {
                        xtype: 'checkbox',
                        itemId: 'cbNationalityIdItemId',
                        margin: '0 0 4 8',
                        checked: true,
                        boxLabel: '忽略',
                        listeners: {
                            change: function (cb, nValue, oValue, opts) {
                                var nationalityIdItemId = cb.up('form').down('#NationalityIdItemId');
                                nationalityIdItemId.setDisabled(nValue);
                                if (!nValue) {
                                    GlobalFun.comboSelectFirstOrDefaultVal(nationalityIdItemId);
                                }
                            }
                        }
                    }
                    ]
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

                    var name = win.down('#FullNameItemId').getValue();
                    var searchFlag = false;
                    var store = traget.getStore();
                    var extraParams = store.getProxy().extraParams;

                    //下葬日期
                    var dateStarField = win.down('#dateStar');
                    var dateEndField = win.down('#dateEnd');
                    var dateStar = dateStarField.getValue();
                    var dateEnd = dateEndField.getValue();
                    GlobalFun.ValidDateStartEnd(dateStarField, dateEndField);

                    var form = win.down('#formId').getForm();
                    if (!form.isValid()) {
                        return;
                    }
                    //名称
                    if (name != '') {
                        //加入filterMap
                        GlobalFun.GridSearchInitFun('FullName', false, store, name);
                        searchFlag = true;
                    } else {
                        GlobalFun.GridSearchInitFun('FullName', true, store, false);
                    }
                  
                    //下葬时间
                    if (dateStar && dateEnd) {
                        //加入.getProxy().extraParams
                        extraParams.DateFilter = true;
                        extraParams.BuryDateQuery = Ext.Date.format(dateStar, 'Y-m-d') + ',' + Ext.Date.format(dateEnd, 'Y-m-d');
                        searchFlag = true;
                    } else {
                        extraParams.DateFilter = false;
                        extraParams.BuryDateQuery = '';
                    }
                    //客户类别
                    var cbCustomerTypeIdItemId = win.down('#cbCustomerTypeIdItemId');
                    var customerTypeIdItemId = win.down('#CustomerTypeIdItemId');
                    if (!cbCustomerTypeIdItemId.getValue()) {
                        //加入filterMap
                        GlobalFun.GridSearchInitFun('CustomerTypeId', false, store, customerTypeIdItemId.getValue());
                        searchFlag = true;
                    } else {
                        GlobalFun.GridSearchInitFun('CustomerTypeId', true, store, false);
                    }
                    //国籍
                    var cbNationalityIdItemId = win.down('#cbNationalityIdItemId');
                    var nationalityIdItemId = win.down('#NationalityIdItemId');
                    if (!cbNationalityIdItemId.getValue()) {
                        //加入filterMap
                        GlobalFun.GridSearchInitFun('NationalityId', false, store, nationalityIdItemId.getValue());
                        searchFlag = true;
                    } else {
                        GlobalFun.GridSearchInitFun('NationalityId', true, store, false);
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