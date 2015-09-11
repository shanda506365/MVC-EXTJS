Ext.create('Ext.data.Store', {
    model: 'chl.Model.BuryManGridModel',
    storeId: 'BuryManGridStoreId',
    filterMap: Ext.create('Ext.util.HashMap'),
    pageSize: GlobalConfig.GridPageSize,
    autoLoad: false,
    remoteSort: true,     //排序通过查询数据库
    sorters: [{
        property: 'Id',
        direction: 'DESC'
    }],
    autoSync: false,
    proxy: {
        type: 'ajax',
        api: GlobalConfig.Controllers.BuryManGrid,
        filterParam: 'filter',
        sortParam: 'sort',
        directionParam: 'dir',
        limitParam: 'limit',
        startParam: 'start',
        simpleSortMode: true,		//单一字段排序
        extraParams: {
            req: 'dataset',
            dataname: 'User',             //dataset名称，根据实际情况设置,数据库名
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
                    //GridManager.UserGrid.loadGrid();
                }
            }
        }
    },
    listeners: {
        load: function (store, records, suc, operation, opts) {
            var total = store.getTotalCount();
            if (total == 0) {
                //if (GridManager.UserGrid) {
                //    GridManager.UserGrid.down("#next").setDisabled(true);
                //    GridManager.UserGrid.down("#last").setDisabled(true);
                //}
            }
            if (suc) {

            } else {
                store.loadData([]);
            }
        }
    }

});