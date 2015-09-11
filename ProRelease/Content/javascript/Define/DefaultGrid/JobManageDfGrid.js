Ext.create('Ext.data.ArrayStore', {
    storeId: 'JobManageDfGridStoreId',
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
	['201', '墓碑预订'],
	["202", '墓碑维护'],
    ["203", '墓碑落葬']
    ]
});

Ext.define('chl.dfgrid.JobManageDfGrid', {
    alternateClassName: ['JobManageDfGrid'],
    alias: 'widget.JobManageDfGrid',
    extend: 'Ext.grid.Panel',
    store: 'JobManageDfGridStoreId',
    actionBaseName: 'JobManageDfGrid',
    itemId: 'JobManageDfGrid',
    FirstLoad: true,
    hideHeaders: true,
    viewConfig: {
        loadMask: false
    },
    columns: [{
        cls: 'treepanel-bigFontSize',
        groupable: false,
        sortable: false,
        dataIndex: 'gridName',
        renderer: GlobalFun.UpdateRecord,
        flex: 1
    }],
    listeners: {
        itemclick: function (view, record) {
            var node = TreeManager.MainItemListTree.getStore().getNodeById(record.data.gridId);
            TreeManager.MainItemListTree.getSelectionModel().select(node, true);
        },
        itemmouseenter: function (view, record, item, index, e, eOpts) {
            item.style.cursor = 'pointer';
        },
        itemmouseleave: function (view, record, item, index, e, eOpts) {
            item.style.cursor = 'auto';
        }
    }
});