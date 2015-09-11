Ext.define('chl.Action.TombstoneSearchGridAction', {
    extend: 'WS.action.Base',
    category: 'TombstoneSearchGridAction'
});
Ext.create('chl.Action.TombstoneSearchGridAction', {
    itemId: 'searchTombstoneSearch',
    iconCls: 'search',
    tooltip: '查询',
    text: '查询',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.searchTombstoneSearch(target);
    },
    updateStatus: function (selection) {
    }
});
Ext.create('chl.Action.TombstoneSearchGridAction', {
    itemId: 'SearchTombstoneSearchJobManageLog',
    iconCls: 'lookTombstoneJobLog',
    tooltip: '业务记录',
    text: '查看墓碑业务操作记录',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.SearchTombstoneSearchJobManageLog(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1);
    }
});
Ext.create('chl.Action.TombstoneSearchGridAction', {
    itemId: 'refreshTombstoneSearch',
    iconCls: 'refresh',
    tooltip: '刷新',
    text: '刷新',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.refreshTombstoneSearch(target);
    },
    updateStatus: function (selection) {
    }
});
//墓碑查询
ActionManager.searchTombstoneSearch = function (traget) {
    if (WindowManager.searchTombstoneSearchWin && WindowManager.searchTombstoneSearchWin != '') {
        WindowManager.searchTombstoneSearchWin.show();
    } else {
        WindowManager.searchTombstoneSearchWin = Ext.create('Ext.window.Window', {
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
                        fieldLabel: '管理费续交日期',
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
                        extraParams.ExpiryDateQuery = Ext.Date.format(dateStar, 'Y-m-d') + ',' + Ext.Date.format(dateEnd, 'Y-m-d');
                        searchFlag = true;
                    } else {
                        extraParams.DateFilter = false;
                        extraParams.ExpiryDateQuery = '';
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

//业务记录
ActionManager.SearchTombstoneSearchJobManageLog = function (traget) {
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
            var html = "<table class='logInfoTable'>";
            html += '<th>操作时间</th>';
            html += '<th>操作名</th>';
            html += '<th>操作人</th>';
            html += '<th>墓碑名</th>';
            Ext.Array.each(data, function (item, index) {
                    html += '<tr>';
                    html += '<td>' + item.DateString + '</td>';
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
                autoScroll:true,
                items: [{
                    xtype: 'container',
                    style: {
                        'background-color':'#FFFFFF'
                    },
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

//刷新墓碑
ActionManager.refreshTombstoneSearch = function (traget) {
    traget.loadGrid();
};