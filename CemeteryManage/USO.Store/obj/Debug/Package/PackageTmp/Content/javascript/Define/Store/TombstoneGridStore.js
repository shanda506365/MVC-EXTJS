
StoreManager.GridStore.CreateTombstoneGridStore = function () {
    Ext.StoreMgr.removeAtKey('TombstoneGridStoreId');
    var tmPty = 'Id', tmDre = 'DESC';

    //if (myStates.infaxgridState.sort && myStates.infaxgridState.sort.property) {
    //    tmPty = myStates.infaxgridState.sort.property;
    //}
    //if (myStates.infaxgridState.sort && myStates.infaxgridState.sort.direction) {
    //    tmDre = myStates.infaxgridState.sort.direction;
    //}
    Ext.create('Ext.data.Store', {
        model: 'chl.Model.TombstoneGridModel',
        storeId: 'TombstoneGridStoreId',
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
                        GridManager.TombstoneGrid.loadGrid();
                    }
                }
            }
        },
        listeners: {
            load: function (store, records, suc, operation, opts) {
                var total = store.getTotalCount();
                if (total == 0) {
                    if (GridManager.TombstoneGrid) {
                        GridManager.TombstoneGrid.down("#next").setDisabled(true);
                        GridManager.TombstoneGrid.down("#last").setDisabled(true);
                    }
                }
                if (suc) {

                } else {
                    store.loadData([]);
                }
            }
        }

    });

    //刷新paging
    if (GridManager.TombstoneGrid) {
        GridManager.TombstoneGrid.down('#pagingtoolbarID').bindStore(Ext.StoreMgr.lookup('TombstoneGridStoreId'));
    }

}