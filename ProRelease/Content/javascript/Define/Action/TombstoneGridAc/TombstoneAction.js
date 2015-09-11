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
        this.setDisabled(!GlobalFun.IsAllowFun('墓碑添加'));
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
        this.setDisabled(selection.length != 1 || !GlobalFun.IsAllowFun('墓碑编辑'));
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
        this.setDisabled(selection.length == 0 || !GlobalFun.IsAllowFun('墓碑编辑'));
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
        this.setDisabled(!GlobalFun.IsAllowFun('墓碑排序'));
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

Ext.create('chl.Action.TombstoneGridAction', {
    itemId: 'exportTombstone',
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
            ExpiryDateQuery: extraParams.ExpiryDateQuery,
            sessiontoken: GlobalFun.getSeesionToken()
        };
        WsCall.downloadFile(GlobalConfig.Controllers.TombstoneGrid.outPutExcelTombstone, 'download', param);
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.TombstoneGridAction', {
    itemId: 'buryPeopleTombstone',
    iconCls: 'buryPeople',
    tooltip: '落葬',
    text: '落葬',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.buryPeopleTombstone(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1 || !GlobalFun.IsAllowFun('墓碑编辑'));
    }
});

Ext.create('chl.Action.TombstoneGridAction', {
    itemId: 'unburyPeopleTombstone',
    iconCls: 'unburyPeople',
    tooltip: '解除落葬',
    text: '解除落葬',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.unburyPeopleTombstone(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1 || !GlobalFun.IsAllowFun('墓碑编辑'));
    }
});

Ext.create('chl.Action.TombstoneGridAction', {
    itemId: 'SearchTombstoneJobManageLog',
    iconCls: 'lookTombstoneJobLog',
    tooltip: '业务记录',
    text: '查看墓碑业务操作记录',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.SearchTombstoneJobManageLog(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1 || !GlobalFun.IsAllowFun('墓碑编辑'));
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
        msg: '删除墓碑后将自动删除该墓碑的所有相关信息,您确定要删除选定的墓碑吗？',
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
    var sWidth = Ext.getBody().getWidth(), sHeight = Ext.getBody().getHeight();

    var fullShowWin = Ext.create('Ext.window.Window', {
        title: '全屏查看',
        modal: true,
        iconCls: 'coverStamp',
        height: sHeight,
        width: sWidth,
        resizable: false,
        groupName: 'TombstoneDDGroup',
        pngClass: pngClass,
        items: [{
            xtype: 'MiniPngDDSortPanel',
            autoScroll: true,
            height: sHeight - 60,
            width: sWidth - 12,
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
                }, {
                    text: '落葬'
                }
            ]
        }]
    }).show();
};
//墓碑查询
ActionManager.searchTombstone = function (traget) {
    if (WindowManager.searchTombstoneWin && WindowManager.searchTombstoneWin != '') {
        WindowManager.searchTombstoneWin.show();
    } else {
        WindowManager.searchTombstoneWin = Ext.create('Ext.window.Window', {
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
                        fieldLabel: '名称',
                        itemId: 'NameItemId'
                    }, {
                        width: 300,
                        colspan: 2,
                        fieldLabel: '别名',
                        itemId: 'AliasItemId'
                    }, {
                        xtype: 'fieldcontainer',
                        colspan: 2,
                        width: 490,
                        fieldLabel: '补交日期',
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
                        fieldLabel: '区域',
                        width: 200,
                        xtype: 'combobox',
                        editable: false,
                        disabled: true,
                        name: 'AreaId',
                        itemId: 'AreaIdItemId',
                        store: 'AreaStoreId',
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
                        itemId: 'cbAreaIdItemId',
                        margin: '0 0 4 8',
                        checked: true,
                        boxLabel: '忽略',
                        listeners: {
                            change: function (cb, nValue, oValue, opts) {
                                var areaIdItemId = cb.up('form').down('#AreaIdItemId');
                                areaIdItemId.setDisabled(nValue);
                                if (!nValue) {
                                    GlobalFun.comboSelectFirstOrDefaultVal(areaIdItemId);
                                }
                            }
                        }
                    }, {
                        fieldLabel: '付款状态',
                        width: 200,
                        xtype: 'combobox',
                        editable: false,
                        disabled: true,
                        name: 'PaymentStatusId',
                        itemId: 'PaymentStatusIdItemId',
                        store: 'PaymentStatusStoreId',
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
                        itemId: 'cbPaymentStatusIdItemId',
                        margin: '0 0 4 8',
                        checked: true,
                        boxLabel: '忽略',
                        listeners: {
                            change: function (cb, nValue, oValue, opts) {
                                var paymentStatusIdItemId = cb.up('form').down('#PaymentStatusIdItemId');
                                paymentStatusIdItemId.setDisabled(nValue);
                                if (!nValue) {
                                    GlobalFun.comboSelectFirstOrDefaultVal(paymentStatusIdItemId);
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

                    var name = win.down('#NameItemId').getValue();
                    var searchFlag = false;
                    var store = traget.getStore();
                    var extraParams = store.getProxy().extraParams;

                    //使用时间
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
                        GlobalFun.GridSearchInitFun('Name', false, store, name);
                        searchFlag = true;
                    } else {
                        GlobalFun.GridSearchInitFun('Name', true, store, false);
                    }
                    //别名
                    var alias = win.down('#AliasItemId').getValue();
                    if (alias != '') {
                        //加入filterMap
                        GlobalFun.GridSearchInitFun('Alias', false, store, alias);
                        searchFlag = true;
                    } else {
                        GlobalFun.GridSearchInitFun('Alias', true, store, false);
                    }
                    //使用时间
                    if (dateStar && dateEnd) {
                        //加入.getProxy().extraParams
                        extraParams.DateFilter = true;
                        extraParams.LastPaymentDate = Ext.Date.format(dateStar, 'Y-m-d') + ',' + Ext.Date.format(dateEnd, 'Y-m-d');
                        searchFlag = true;
                    } else {
                        extraParams.DateFilter = false;
                        extraParams.LastPaymentDate = '';
                    }
                    //区域
                    var cbAreaIdItemId = win.down('#cbAreaIdItemId');
                    var areaIdItemId = win.down('#AreaIdItemId');
                    if (!cbAreaIdItemId.getValue()) {
                        //加入filterMap
                        GlobalFun.GridSearchInitFun('AreaId', false, store, areaIdItemId.getValue());
                        searchFlag = true;
                    } else {
                        GlobalFun.GridSearchInitFun('AreaId', true, store, false);
                    }
                    //付款状态
                    var cbPaymentStatusIdItemId = win.down('#cbPaymentStatusIdItemId');
                    var paymentStatusIdItemId = win.down('#PaymentStatusIdItemId');
                    if (!cbPaymentStatusIdItemId.getValue()) {
                        //加入filterMap
                        GlobalFun.GridSearchInitFun('PaymentStatusId', false, store, paymentStatusIdItemId.getValue());
                        searchFlag = true;
                    } else {
                        GlobalFun.GridSearchInitFun('PaymentStatusId', true, store, false);
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
//落葬
ActionManager.buryPeopleTombstone = function (traget) {
    var sm = traget.getSelectionModel();
    var records = sm.getSelection();
    var customerDtos = records[0].data.CustomerBuryDtos;
    var customers = [];
    Ext.Array.each(customerDtos, function (item, index) {
        customers.push({
            Id: item.Id,
            Name: item.FullNameString
        });
    });
    //弹出窗口选择用户
    var tempWin = Ext.create('Ext.window.Window', {
        title: "选择落葬人",
        defaultFocus: 'NameItemId',
        iconCls: '',
        record: false,
        //border: false,
        height: 300,
        width: 500,
        layout: 'vbox',
        modal: true,
        resizable: false,
        layout: {
            type: 'table',
            columns: 2
        },
        defaults: {
            labelAlign: 'right',
            labelPad: 15,
            width: 200,
            labelWidth: 80,
            margin: '5px 5px 5px 5px'
        },
        items: [{
            margin: '15px 5px 5px 5px',
            xtype: 'container',
            colspan: 2,
            width: 450,
            layout: 'hbox',
            defaults: {
                labelAlign: 'right',
                labelPad: 10,
                width: 280,
                margin: '0 0 5 5',
                labelWidth: 80
            },
            items: [{
                fieldLabel: '落葬人',
                xtype: 'textfield',
                hidId: 0,
                readOnly: true,
                submitValue: false,
                name: 'CustomerName',
                itemId: 'CustomerNameItemId'
            }, {
                xtype: 'button',
                width: 80,
                iconCls: 'addUser',
                text: '选择落葬人',
                handler: function () {
                    var w = this.up('window');
                    var field = w.down('#CustomerNameItemId');
                    WindowManager.SelectCustomerWin = WindowManager.ShowSelectCustomerWin();
                    WindowManager.SelectCustomerWin.callComponent = field;
                }
            }, {
                xtype: 'hidden',
                name: 'CustomerId',
                itemId: 'CustomerIdItemId'
            }]
        }, {
            xtype: 'container',
            colspan: 2,
            width: 450,
            layout: 'hbox',
            defaults: {
                labelAlign: 'right',
                labelPad: 10,
                width: 280,
                margin: '0 0 5 5',
                labelWidth: 80
            },
            items: [{
                fieldLabel: '落葬人2',
                xtype: 'textfield',
                hidId: 0,
                readOnly: true,
                submitValue: false,
                name: 'CustomerName2',
                itemId: 'CustomerNameItemId2'
            }, {
                xtype: 'button',
                width: 80,
                iconCls: 'addUser',
                text: '选择落葬人2',
                handler: function () {
                    var w = this.up('window');
                    var field = w.down('#CustomerNameItemId2');
                    WindowManager.SelectCustomerWin = WindowManager.ShowSelectCustomerWin();
                    WindowManager.SelectCustomerWin.callComponent = field;
                }
            }, {
                xtype: 'hidden',
                name: 'CustomerId2',
                itemId: 'CustomerIdItemId2'
            }]
        }],
        buttons: [{
            text: '重置',
            handler: function () {
                var me = this;
                var w = me.up('window');
                var f = w.down('#formId');
                //赋值给隐藏
                var customerIdItemId = w.down('#CustomerIdItemId');
                customerIdItemId.setValue(w.down('#CustomerNameItemId').hidId);
                var customerIdItemId2 = w.down('#CustomerIdItemId2');
                customerIdItemId2.setValue(w.down('#CustomerNameItemId2').hidId);

                var CustomerNameItemId = w.down('#CustomerNameItemId');
                var CustomerNameItemId2 = w.down('#CustomerNameItemId2');

                w.down('#CustomerNameItemId').hidId = 0;
                customerIdItemId.setValue(0);
                CustomerNameItemId.setValue("");
                w.down('#CustomerNameItemId2').hidId = 0;
                customerIdItemId2.setValue(0);
                CustomerNameItemId2.setValue("");
            }
        }, {
            text: '确定',
            itemId: 'submit',
            handler: function () {
                var me = this;
                var w = me.up('window');
                //赋值给隐藏
                var customerIdItemId = w.down('#CustomerIdItemId');
                customerIdItemId.setValue(w.down('#CustomerNameItemId').hidId);
                var customerIdItemId2 = w.down('#CustomerIdItemId2');
                customerIdItemId2.setValue(w.down('#CustomerNameItemId2').hidId);

                var CustomerNameItemId = w.down('#CustomerNameItemId');
                var CustomerNameItemId2 = w.down('#CustomerNameItemId2');

                var ids = [];
                if (customerIdItemId.getValue() != "" && customerIdItemId.getValue() > 0) {
                    ids.push(customerIdItemId.getValue());
                }
                if (customerIdItemId2.getValue() != "" && customerIdItemId2.getValue() > 0) {
                    ids.push(customerIdItemId2.getValue());
                }

                if (ids.length > 0) {
                    var param = {
                        tombstoneId: records[0].data.Id,
                        customerIds: ids.join(','),
                        sessiontoken: GlobalFun.getSeesionToken()
                    };
                    WsCall.call(GlobalConfig.Controllers.TombstoneGrid.BuryPeopleTombstone, 'BuryPeopleTombstone', param, function (response, opts) {
                        var data = response.dataset;
                        traget.loadGrid();
                        w.close();
                    }, function (response, opts) {
                        if (!GlobalFun.errorProcess(response.code)) {
                            Ext.Msg.alert('失败', response.msg);
                        }
                    }, true, "请稍候...");
                } else {
                    Ext.Msg.alert('信息', '请选择至少一位落葬人');
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
    tempWin.show(null, function () {
        //如果已有落葬则绑定数据
        if (customers.length > 0) {
            var customerIdItemId = tempWin.down('#CustomerIdItemId');
            var customerIdItemId2 = tempWin.down('#CustomerIdItemId2');

            var CustomerNameItemId = tempWin.down('#CustomerNameItemId');
            var CustomerNameItemId2 = tempWin.down('#CustomerNameItemId2');
            Ext.Array.each(customers, function (item, index) {
                if (index == 0) {
                    tempWin.down('#CustomerNameItemId').hidId = item.Id;
                    customerIdItemId.setValue(tempWin.down('#CustomerNameItemId').hidId);
                    CustomerNameItemId.setValue(item.Name);
                } else {
                    tempWin.down('#CustomerNameItemId2').hidId = item.Id;
                    customerIdItemId2.setValue(tempWin.down('#CustomerNameItemId2').hidId);
                    CustomerNameItemId2.setValue(item.Name);
                }
            });
        }
    });

};
//解除落葬
ActionManager.unburyPeopleTombstone = function (traget) {
    var sm = traget.getSelectionModel();
    var records = sm.getSelection();
    var customerDtos = records[0].data.CustomerBuryDtos;
    var customerIds = [];
    Ext.Array.each(customerDtos, function (item, index) {
        customerIds.push(item.Id);
    });
    if (customerIds.length <= 0) {
        Ext.Msg.alert('信息', '该墓碑尚未落葬');
        return;
    }

    GlobalConfig.newMessageBox.show({
        title: '提示',
        msg: '您确定要解除落葬选定的墓碑吗？',
        buttons: Ext.MessageBox.YESNO,
        closable: false,
        fn: function (btn) {
            if (btn == 'yes') {
                var param = {
                    tombstoneId: records[0].data.Id,
                    customerIds: customerIds.join(','),
                    sessiontoken: GlobalFun.getSeesionToken()
                };
                WsCall.call(GlobalConfig.Controllers.TombstoneGrid.UnBuryPeopleTombstone, 'UnBuryPeopleTombstone', param, function (response, opts) {
                    var data = response.dataset;
                    traget.loadGrid();
                }, function (response, opts) {
                    if (!GlobalFun.errorProcess(response.code)) {
                        Ext.Msg.alert('失败', response.msg);
                    }
                }, true, "请稍候...");
            }
        },
        icon: Ext.MessageBox.QUESTION
    });

};

//业务记录
ActionManager.SearchTombstoneJobManageLog = function (traget) {
    var sm = traget.getSelectionModel();
    var records = sm.getSelection();
    //取预订的值
    var param = {
        Id: records[0].data.Id,
        controllName: '',
        sessiontoken: GlobalFun.getSeesionToken()
    };

    WsCall.call(GlobalConfig.Controllers.JobManage.GetTombstoneJobInfoLog, 'GetTombstoneJobInfoLog', param, function (response, opts) {
        var data = response.ResultOutDtos;
        if (data) {
            var html = "<table>";
            html += '<th>操作时间</th>';
            html += '<th>操作名</th>';
            html += '<th>操作人</th>';
            html += '<th>墓碑名</th>';
            Ext.Array.each(data, function (item,index) {
                html += '<tr>';
                html += '<td>'+item.DateString+'</td>';
                html += '<td>' + item.ControlName + '</td>';
                html += '<td>' + item.Content.substring(2, item.Content.indexOf('--')) + '</td>';
                html += '<td>' + item.TombstoneEntity.Name + '</td>';
                html += '</tr>';
            });
            html += "</table>";
            Ext.create('Ext.window.Window', {
                title: "查看业务操作记录",
                defaultFocus: 'AliasItemId',
                iconCls: '',
                record: false,
                height: 300,
                width: 500,
                modal: true,
                resizable: false,
                layout: 'fit',
                items:[{
                    xtype: 'container',
                    html: html
                }]
            }).show();
        }
    }, function (response, opts) {
        if (!GlobalFun.errorProcess(response.code)) {
            Ext.Msg.alert('失败', response.msg);
        }
    }, true, "请稍候...");
};