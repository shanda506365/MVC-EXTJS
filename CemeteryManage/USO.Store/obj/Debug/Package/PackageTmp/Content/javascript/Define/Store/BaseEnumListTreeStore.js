

// BaseEnumListTree  Store
Ext.create('Ext.data.TreeStore', {
    model: 'chl.Model.BaseEnumListTreeModel',
    storeId: 'BaseEnumListTreeStoreId',
    autoLoad: false,
    // clearOnLoad:false,
    proxy: {
        type: 'ajax',
        url: GlobalConfig.Controllers.BaseEnumListTree,
        extraParams: {
            sessiontoken: GlobalFun.getSeesionToken(),
            req: 'treenodes',
            treename: 'BaseEnumListTree',
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
        text: '基础数据维护',
        iconCls: 'fax'
    }
});
