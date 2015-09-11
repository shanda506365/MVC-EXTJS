// ExReportListTree  Store
Ext.create('Ext.data.TreeStore', {
    model: 'chl.Model.ExReportListTreeModel',
    storeId: 'ExReportListTreeStoreId',
    autoLoad: false,
    // clearOnLoad:false,
    proxy: {
        type: 'ajax',
        url: GlobalConfig.Controllers.ExReportListTree,
        extraParams: {
            sessiontoken: GlobalFun.getSeesionToken(),
            req: 'treenodes',
            treename: 'ExReportListTree',
            restype: 'json',
            hadsel: false

        },
        reader: {
            type: 'json',
            root: 'dataset',
            successProperty: 'success',
            messageProperty: 'msg'
        },
        actionMethods: 'POST'
    },

    root: {
        expanded: false,
        text: '报表统计',
        iconCls: 'fax'
    }
});
