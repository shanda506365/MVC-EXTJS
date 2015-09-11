// 主页面树Store
Ext.create('Ext.data.TreeStore', {
    model: 'chl.Model.MainItemListTreeModel',
    storeId: 'MainItemListTreeStoreId',
    autoLoad: false,
    // clearOnLoad:false,
    proxy: {
        type: 'ajax',
        url: GlobalConfig.Controllers.MainItemListTree,
        extraParams: {
            sessiontoken: GlobalFun.getSeesionToken(),
            req: 'treenodes',
            treename: 'MainItemListTree',
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
        text: '陵园系统',
        iconCls: 'fax'
    }
});
