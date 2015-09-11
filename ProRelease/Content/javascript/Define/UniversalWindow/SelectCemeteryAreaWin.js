StoreManager.GridStore.CreateSelectCemeteryAreaGridStore = function () {
    Ext.define('chl.Model.SelectCemeteryAreaGridModel', {
        extend: 'Ext.data.Model',
        idProperty: 'Id',
        alternateClassName: 'SelectCemeteryAreaGridModel',
        fields: [{
            name: 'Id',
            type: 'string'
        }, {
            name: 'Name',
            type: 'string'
        }, {
            name: 'Alias',
            type: 'string'
        }, {
            name: 'Remark',
            type: 'string'
        }, {
            name: 'TotalCount'
        }, {
            name: 'OrderCount'
        }, {
            name: 'SaleCount'
        }, {
            name: 'BuryCount'
        }, {
            name: 'ElseCount'
        }, {
            name: 'RowSort'
        }, {
            name: 'RowSortString'
        }]
    });

    Ext.StoreMgr.removeAtKey('SelectCemeteryAreaGridStoreId');
    var tmPty = 'Id', tmDre = 'DESC';

    //if (myStates.infaxgridState.sort && myStates.infaxgridState.sort.property) {
    //    tmPty = myStates.infaxgridState.sort.property;
    //}
    //if (myStates.infaxgridState.sort && myStates.infaxgridState.sort.direction) {
    //    tmDre = myStates.infaxgridState.sort.direction;
    //}
    Ext.create('Ext.data.Store', {
        model: 'chl.Model.SelectCemeteryAreaGridModel',
        storeId: 'SelectCemeteryAreaGridStoreId',
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
            api: GlobalConfig.Controllers.CemeteryAreaGrid,
            filterParam: 'filter',
            sortParam: 'sort',
            directionParam: 'dir',
            limitParam: 'limit',
            startParam: 'start',
            simpleSortMode: true,		//单一字段排序
            extraParams: {
                req: 'dataset',
                dataname: 'CemeteryArea',             //dataset名称，根据实际情况设置,数据库名
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
                        var grid = WindowManager.ShowSelectCemeteryAreaWin.down('CemeteryAreaGrid');
                        grid.loadGrid();
                    }
                }
            }
        },
        listeners: {
            load: function (store, records, suc, operation, opts) {
                var total = store.getTotalCount();
                if (total == 0) {
                    var grid = WindowManager.ShowSelectCemeteryAreaWin.down('CemeteryAreaGrid');
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
StoreManager.GridStore.CreateSelectCemeteryAreaGridStore();
Ext.define('chl.Action.SelectCemeteryAreaGridAction', {
    extend: 'WS.action.Base',
    category: 'SelectCemeteryAreaGridAction'
});



//创建显示客户选择窗口
WindowManager.ShowSelectCemeteryAreaWin = function (callBack) {
    return Ext.create('Ext.window.Window', {
        title: "选择区域",
        iconCls: '',
        height: 400,
        width: 500,
        layout: 'fit',
        ReturnRecord: '',
        callComponent: '',
        modal: true,
        resizable: false,
        items: [{
            xtype: 'CemeteryAreaGrid',
            actionBaseName: 'SelectCemeteryAreaGridAction',
            store: 'SelectCemeteryAreaGridStoreId',
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
                    grid.down('#pagingtoolbarID').bindStore(Ext.StoreMgr.lookup('SelectCemeteryAreaGridStoreId'));
                    grid.loadGrid();
                }
            },
            columns: [{
                id: 'Name',
                text: '名称',
                dataIndex: 'Name',
                renderer: GlobalFun.UpdateRecord,
                width: 300
            }, {
                id: 'Alias',
                text: '编号',
                dataIndex: 'Alias',
                renderer: GlobalFun.UpdateRecord,
                flex:1
            }],
            dockedItems: [{
                xtype: 'toolbar',
                itemId: 'toolbarID',
                dock: 'top',
                layout: {
                    overflowHandler: 'Menu'
                },
                items: ['->', {
                    fieldLabel: '按编号查找',
                    text: '按编号查找',//用于控制工具栏使用
                    width: 300,
                    labelAlign: 'right',
                    labelWidth: 80,
                    xtype: 'searchfield',
                    paramName: 'Alias',
                    //store: searchStore,
                    itemId: 'SelectCemeteryAreaGridSearchfieldId',
                    regex: GlobalConfig.RegexController.regexAreaCode,
                    regexText: '请输入3位编码',
                    listeners: {
                        render: function () {
                            var me = this;
                            me.store = Ext.data.StoreManager.lookup('SelectCemeteryAreaGridStoreId');
                        }
                    }
                }]
            }, {
                xtype: 'Pagingtoolbar',
                itemId: 'pagingtoolbarID',
                store: 'SelectCemeteryAreaGridStoreId',
                dock: 'bottom',
                items: []
            }]
        }],
        listeners: {
            beforeclose: function (component, eOpts) {
                var grid = component.down('CemeteryAreaGrid');
                var sm = grid.getSelectionModel();
                if (sm.hasSelection()) {
                    component.ReturnRecord = sm.getSelection()[0];
                    if (component.callComponent) {
                        component.callComponent.setValue(component.ReturnRecord.data.Alias);
                        component.callComponent.hidId = component.ReturnRecord.data.Id;
                    }
                }
            },
            show: function (component, eOpts) {
                var grid = component.down('CemeteryAreaGrid');
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
