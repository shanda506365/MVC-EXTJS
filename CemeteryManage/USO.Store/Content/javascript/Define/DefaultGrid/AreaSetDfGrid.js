Ext.create('Ext.data.ArrayStore', {
    storeId: 'AreaSetDfGridStoreId',
    fields: [{
        name: 'gridId',
        type: 'string'
    }, {
        name: 'gridName',
        type: 'string'
    }],
    autoLoad: false,
    pageSize: 10,
    data: [
	['3001', '区域管理'],
	["3002", '墓碑管理']
    ]
});

Ext.define('chl.dfgrid.AreaSetDfGrid', {
    alternateClassName: ['AreaSetDfGrid'],
    alias: 'widget.AreaSetDfGrid',
    extend: 'Ext.grid.Panel',
    store: 'AreaSetDfGridStoreId',
    actionBaseName: 'AreaSetDfGrid',
    itemId: 'AreaSetDfGrid',
    FirstLoad: true,
    hideHeaders:true,
    viewConfig: {
        loadMask: false
    },
    columns: [{
        cls: 'treepanel-bigFontSize',
        groupable: false,
        sortable:false,
        dataIndex: 'gridName',
        renderer: GlobalFun.UpdateRecord,
        flex: 1
    }],
    listeners: {
        itemclick: function (view, record) {
            var node = TreeManager.BaseEnumListTree.getStore().getNodeById(record.data.gridId);
            TreeManager.BaseEnumListTree.getSelectionModel().select(node, true);
        },
        itemmouseenter: function (view, record, item, index, e, eOpts) {
            item.style.cursor = 'pointer';
        },
        itemmouseleave: function (view, record, item, index, e, eOpts) {
            item.style.cursor = 'auto';
        }
    }
});