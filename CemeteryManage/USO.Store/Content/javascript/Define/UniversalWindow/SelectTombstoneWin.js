//创建墓碑选择表Store
StoreManager.GridStore.CreateSelectTombstoneGridStore = function () {
    Ext.define('chl.Model.SelectTombstoneGridModel', {
        extend: 'Ext.data.Model',
        idProperty: 'Id',
        alternateClassName: 'SelectTombstoneGridModel',
        fields: [{
            name: 'Id',
            type: 'string'
        }, {
            name: 'Name',
            type: 'string'
        }, {
            name: 'AreaId',
            type: 'string'
        }, {
            name: 'AreaEntity'
        }, {
            name: 'RowId',
            type: 'string'
        }, {
            name: 'RowEntity'
        }, {
            name: 'ColumnId',
            type: 'string'
        }, {
            name: 'ColumnEntity'
        }, {
            name: 'ParentId',
            type: 'string'
        }, {
            name: 'ParentName',
            type: 'string'
        }, {
            name: 'Alias',
            type: 'string'
        }, {
            name: 'Remark',
            type: 'string'
        }, {
            name: 'CustomerId',
            type: 'string'
        }, {
            name: 'CustomerName',
            type: 'string'
        }, {
            name: 'StoneText',
            type: 'string'
        }, {
            name: 'ExpiryDate',
            mapping: 'ExpiryDateString',
            type: 'string'
        }, {
            name: 'BuyDate',
            mapping: 'BuyDateString',
            type: 'string'
        }, {
            name: 'LastPaymentDate',
            mapping: 'LastPaymentDateString',
            type: 'string'
        }, {
            name: 'BuryDate',
            mapping: 'BuryDateString',
            type: 'string'
        }, {
            name: 'Width',
            type: 'string'
        }, {
            name: 'Height',
            type: 'string'
        }, {
            name: 'Acreage',
            type: 'string'
        }, {
            name: 'SecurityLevelId',
            type: 'string'
        }, {
            name: 'SecurityLevelName'
        }, {
            name: 'Image'
        }, {
            name: 'ServiceLevelId'
        }, {
            name: 'ServiceLevelName'
        }, {
            name: 'TypeId'
        }, {
            name: 'TypeName'
        }]
    });


    Ext.StoreMgr.removeAtKey('SelectTombstoneGridStoreId');
    var tmPty = 'Id', tmDre = 'DESC';

    Ext.create('Ext.data.Store', {
        model: 'chl.Model.SelectTombstoneGridModel',
        storeId: 'SelectTombstoneGridStoreId',
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
            api: GlobalConfig.Controllers.TombstoneGrid,
            filterParam: 'filter',
            sortParam: 'sort',
            directionParam: 'dir',
            limitParam: 'limit',
            startParam: 'start',
            simpleSortMode: true,		//单一字段排序
            extraParams: {
                req: 'dataset',
                dataname: 'Tombstone',             //dataset名称，根据实际情况设置,数据库名
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
                        var grid = WindowManager.ShowSelectTombstoneWin.down('TombstoneGrid');
                        grid.loadGrid();
                    }
                }
            }
        },
        listeners: {
            load: function (store, records, suc, operation, opts) {
                var total = store.getTotalCount();
                if (total == 0) {
                    var grid = WindowManager.SelectTombstoneWin.down('TombstoneGrid');
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
StoreManager.GridStore.CreateSelectTombstoneGridStore();
Ext.define('chl.Action.SelectTombstoneGridAction', {
    extend: 'WS.action.Base',
    category: 'SelectTombstoneGridAction'
});

Ext.create('chl.Action.SelectTombstoneGridAction', {
    itemId: 'SelectdelTombstone',
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

Ext.create('chl.Action.SelectTombstoneGridAction', {
    itemId: 'SelectrefreshTombstone',
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


//创建显示墓碑选择窗口
WindowManager.ShowSelectTombstoneWin = function (callBack) {
    return Ext.create('Ext.window.Window', {
        title: "选择墓碑",
        iconCls: '',
        height: 400,
        width: 500,
        layout: 'fit',
        ReturnRecord: '',
        callComponent: '',
        modal: true,
        resizable: false,
        items: [{
            xtype: 'TombstoneGrid',
            actionBaseName: 'SelectTombstoneGridAction',
            store: 'SelectTombstoneGridStoreId',
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
                    grid.down('#pagingtoolbarID').bindStore(Ext.StoreMgr.lookup('SelectTombstoneGridStoreId'));
                    grid.loadGrid();
                }
            },
            columns: [{
                text: '编号',
                dataIndex: 'Id',
                width: 100
            }, {
                text: '名称',
                dataIndex: 'Name',
                renderer: GlobalFun.UpdateRecord,
                width: 150
            }, {
                text: '别名',
                dataIndex: 'Alias',
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
                       paramName: 'Name',
                       listeners: {
                           render: function () {
                               var me = this;
                               me.store = Ext.data.StoreManager.lookup('SelectTombstoneGridStoreId');;
                           }
                       }
                   }]
            }, {
                xtype: 'Pagingtoolbar',
                itemId: 'pagingtoolbarID',
                store: 'SelectTombstoneGridStoreId',
                dock: 'bottom',
                items: [{
                    xtype: 'tbtext',
                    text: '过滤:'
                }, {
                    xtype: 'GridFilterMenuButton',
                    itemId: 'menuID',
                    text: '全部区域',
                    filterParam: {
                        group: 'areaSelectGroup',
                        text: '全部区域',
                        filterKey: 'AreaId',
                        GridItemId: 'TombstoneGrid',
                        store: StoreManager.ComboStore.AreaStore
                    }
                }
                ]
            }]
        }],
        listeners: {
            beforeclose: function (component, eOpts) {
                var grid = component.down('TombstoneGrid');
                var sm = grid.getSelectionModel();
                if (sm.hasSelection()) {
                    component.ReturnRecord = sm.getSelection()[0];
                    if (component.callComponent) {
                        component.callComponent.setValue(component.ReturnRecord.data.Name);
                        component.callComponent.hidId = component.ReturnRecord.data.Id;
                    }
                }
            },
            show: function (component, eOpts) {
                var grid = component.down('TombstoneGrid');
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


