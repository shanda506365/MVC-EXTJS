//创建客户选择表Store
StoreManager.GridStore.CreateSelectCustomerGridStore = function () {
    Ext.define('chl.Model.SelectCustomerGridModel', {
        extend: 'Ext.data.Model',
        idProperty: 'Id',
        alternateClassName: 'SelectCustomerGridModel',
        fields: [{
            name: 'Id',
            type: 'string'
        }, {
            name: 'FullName',
            type: 'string'
        }, {
            name: 'LastName',
            type: 'string'
        }, {
            name: 'FirstName',
            type: 'string'
        }, {
            name: 'MiddleName',
            type: 'string'
        }, {
            name: 'Remark',
            type: 'string'
        }, {
            name: 'Telephone',
            type: 'string'
        }, {
            name: 'Phone',
            type: 'string'
        }, {
            name: 'OtherPhone',
            type: 'string'
        }, {
            name: 'Address',
            type: 'string'
        }, {
            name: 'CustomerTypeId',
            type: 'string'
        }, {
            name: 'CustomerType'
        }, {
            name: 'LinkCustomerId',
            type: 'string'
        }, {
            name: 'LinkCustomer'
        }, {
            name: 'BuryDate',
            mapping: 'BuryDateString',
            type: 'string'
        }, {
            name: 'DeathDate',
            type: 'string'
        }, {
            name: 'CustomerStatusId',
            type: 'string'
        }, {
            name: 'CustomerStatus'
        }, {
            name: 'NationalityId',
            type: 'string'
        }, {
            name: 'Nationality'
        }, {
            name: 'IDNumber',
            type: 'string'
        }]
    });


    Ext.StoreMgr.removeAtKey('SelectCustomerGridStoreId');
    var tmPty = 'Id', tmDre = 'DESC';

    Ext.create('Ext.data.Store', {
        model: 'chl.Model.SelectCustomerGridModel',
        storeId: 'SelectCustomerGridStoreId',
        filterMap: Ext.create('Ext.util.HashMap'),
        pageSize: GlobalConfig.GridPageSize,
        autoLoad: false,
        remoteSort: true,     //排序通过查询数据库
        sorters: [{
            property: tmPty,
            direction: tmDre
        }],
        autoSync: false,
        proxy: {
            type: 'ajax',
            api: GlobalConfig.Controllers.CustomerGrid,
            filterParam: 'filter',
            sortParam: 'sort',
            directionParam: 'dir',
            limitParam: 'limit',
            startParam: 'start',
            simpleSortMode: true,		//单一字段排序
            extraParams: {
                req: 'dataset',
                dataname: 'Customer',             //dataset名称，根据实际情况设置,数据库名
                restype: 'json',
                sessiontoken: GlobalFun.getSeesionToken(),
                folderid: -1,
                refresh: null,
                template: ''//当前模版
            },
            reader: {
                type: 'json',
                root: 'dataset',
                seccessProperty: 'success',
                messageProperty: 'msg',
                totalProperty: 'total'
            },
            writer: {
                type: 'json',
                writeAllFields: false,
                allowSingle: false
                //			root: 'dataset'
            },
            actionMethods: 'POST',
            listeners: {
                exception: function (proxy, response, operation) {
                    var json = Ext.JSON.decode(response.responseText);
                    var code = json.code;
                    GlobalFun.errorProcess(code);
                    if (operation.action != 'read') {
                        var grid = WindowManager.ShowSelectCustomerWin.down('CustomerGrid');
                        grid.loadGrid();
                    }
                }
            }
        },
        listeners: {
            load: function (store, records, suc, operation, opts) {
                var total = store.getTotalCount();
                if (total == 0) {
                    var grid = WindowManager.SelectCustomerWin.down('CustomerGrid');
                    if (grid) {
                        grid.down("#next").setDisabled(true);
                        grid.down("#last").setDisabled(true);
                    }
                }
                if (suc) {

                } else {
                    store.loadData([]);
                }
            }
        }
    });


};
StoreManager.GridStore.CreateSelectCustomerGridStore();
Ext.define('chl.Action.SelectCustomerGridAction', {
    extend: 'WS.action.Base',
    category: 'SelectCustomerGridAction'
});

Ext.create('chl.Action.SelectCustomerGridAction', {
    itemId: 'SelectdelCustomer',
    iconCls: 'delete',
    tooltip: '删除',
    text: '删除',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.delCustomer(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length == 0);
    }
});

Ext.create('chl.Action.SelectCustomerGridAction', {
    itemId: 'SelectrefreshCustomer',
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


//创建显示客户选择窗口
WindowManager.ShowSelectCustomerWin = function (callBack) {
    return Ext.create('Ext.window.Window', {
        title: "选择客户",
        iconCls: '',
        height: 400,
        width: 500,
        layout: 'fit',
        ReturnRecord: '',
        callComponent: '',
        modal: true,
        resizable: false,
        items: [{
            xtype: 'CustomerGrid',
            actionBaseName: 'SelectCustomerGridAction',
            store: 'SelectCustomerGridStoreId',
            listeners: {
                itemclick: function (grid, record, hitem, index, e, opts) {
                    var me = this;
                },
                itemdblclick: function (grid, record, hitem, index, e, opts) {
                    var me = this;
                    var w = me.up('window');
                    w.close();
                },
                itemcontextmenu: function (view, rec, item, index, e, opts) {
                    e.stopEvent();
                    //infax_RightMenu.showAt(e.getXY());
                },
                selectionchange: function (view, seles, op) {
                    if (!seles[0])
                        return;
                    var me = this;
                    ActionBase.updateActions(me.actionBaseName, seles);
                },
                show: function (grid, opts) {
                    grid.down('#pagingtoolbarID').bindStore(Ext.StoreMgr.lookup('SelectCustomerGridStoreId'));
                    grid.loadGrid();
                }
            },
            columns: [{
                text: '编号',
                dataIndex: 'Id',
                width: 100
            }, {
                text: '姓名',
                dataIndex: 'FullName',
                renderer: function (value, metaData, record) {
                    var myVal = record.data.LastName + record.data.MiddleName + record.data.FirstName;
                    return GlobalFun.UpdateRecord(myVal, metaData, record);
                },
                width: 150
            }, {
                text: '身份证号',
                dataIndex: 'IDNumber',
                width: 250,
                renderer: GlobalFun.UpdateRecord
            }],
            dockedItems: [{
                xtype: 'toolbar',
                itemId: 'toolbarID',
                dock: 'top',
                layout: {
                    overflowHandler: 'Menu'
                },
                items: [ActionBase.getAction('SelectrefreshCustomer'), ActionBase.getAction('SelectdelCustomer'),
                   '->', {
                       fieldLabel: '按名称查找',
                       text: '按名称查找',//用于控制工具栏使用
                       width: 300,
                       labelAlign: 'right',
                       labelWidth: 80,
                       xtype: 'searchfield',
                       itemId: 'selectCustomerGridSearchfieldId',
                       listeners: {
                           render: function () {
                               var me = this;
                               me.store = Ext.data.StoreManager.lookup('SelectCustomerGridStoreId');
                           }
                       }
                   }]
            }, {
                xtype: 'Pagingtoolbar',
                itemId: 'pagingtoolbarID',
                store: 'SelectCustomerGridStoreId',
                dock: 'bottom',
                items: [{
                    xtype: 'tbtext',
                    text: '过滤:'
                }, {
                    xtype: 'GridFilterMenuButton',
                    itemId: 'menuID',
                    text: '全部类别',
                    filterParam: {
                        group: 'customerTypeSelectGroup',
                        text: '全部类别',
                        filterKey: 'CustomerTypeId',
                        GridTypeName: 'CustomerGrid',
                        store: StoreManager.ComboStore.CustomerTypeStore
                    }
                }
                ]
            }]
        }],
        listeners: {
            beforeclose: function (component, eOpts) {
                var grid = component.down('CustomerGrid');
                var sm = grid.getSelectionModel();
                if (sm.hasSelection()) {
                    component.ReturnRecord = sm.getSelection()[0];
                    if (component.callComponent) {
                        component.callComponent.setValue(component.ReturnRecord.data.LastName + component.ReturnRecord.data.FirstName);
                        component.callComponent.hidId = component.ReturnRecord.data.Id;
                    }
                }
            },
            show: function (component, eOpts) {
                var grid = component.down('CustomerGrid');
                grid.loadGrid();
                component.ReturnRecord = '';
                component.callComponent = '';
            }
        },
        buttons: [{
            text: '确定',
            itemId: 'submit',
            handler: function () {
                var me = this;
                var w = me.up('window');

                w.close();
            }
        }, {
            text: '取消',
            handler: function () {
                var me = this;
                me.up('window').close();
            }
        }]
    }).show(null, callBack);
};


