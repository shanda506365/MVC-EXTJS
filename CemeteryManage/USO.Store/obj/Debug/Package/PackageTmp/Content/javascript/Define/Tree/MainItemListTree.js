// MainItemListTree类
Ext.define('chl.tree.MainItemListTree', {
    alias: 'widget.MainItemListTree',
    itemId: 'MainItemListTree',
    title: '陵园系统',
    iconCls: 'faxTitle',
    extend: 'chl.tree.BaseTree',
    clearOnLoad: false,
    alternateClassName: ['MainItemListTree'],
    store: 'MainItemListTreeStoreId',
    ddGroup: 'MainItemListTreeDDGp',
    //width : 400,
    //height : 600,
    animate: false,
    rootVisible: false,
    frame: false,
    frameHeader: false,
    border: false,
    listeners: {
        itemcontextmenu: function (view, rec, item, index, e, opts) {
            alert(rec.data.parent.Text);
            e.stopEvent();
            //folderTree_RightMenu.showAt(e.getXY());
        },
        expand: function (tree, opts) {
            if (!tree.getSelectionModel().hasSelection()) {
                tree.getSelectionModel().select(0, true);
            }
        },

        boxready: function (com, width, height, opts) {
            (new Ext.util.DelayedTask()).delay(560, function () {
                if (!TreeManager.MainItemListTree.getSelectionModel().hasSelection()) {
                    TreeManager.MainItemListTree.getSelectionModel().select(0, true);
                }
            });
        }

    },
    initComponent: function () {
        var me = this;
        me.callParent(arguments);

    }
});

//根据传入参数创建客户表，返回自身
TreeManager.CreateMainItemListTree = function (param) {
    TreeManager.MainItemListTree = Ext.create('chl.tree.MainItemListTree');
    if (param && param.needLoad) {
        TreeManager.MainItemListTree.getStore().load({
            callback: function () {
                var rootNode = TreeManager.MainItemListTree.getRootNode();
                rootNode.expand();
            }
        });
    }
    return TreeManager.MainItemListTree;
};

TreeManager.SetMainItemListTreeSelectionChangeEvent = function (param) {
    TreeManager.MainItemListTree.on('selectionchange', function (view, seles, op) {
        if (!seles[0])
            return;
        //客户管理
        if (seles[0].data.id == 1) {
            var cardPanel = GlobalConfig.ViewPort.down('#centerGridDisplayContainer');
            cardPanel.getLayout().setActiveItem(0);
            if (!GridManager.CustomerGrid.FirstLoad) {
                GridManager.CustomerGrid.loadGrid();
                GridManager.CustomerGrid.FirstLoad = true;
            }
            return;
        }
        //墓碑管理
        if (seles[0].data.id == 2) {
            var cardPanel = GlobalConfig.ViewPort.down('#centerGridDisplayContainer');
            cardPanel.getLayout().setActiveItem(1);
            if (!GridManager.TombstoneGrid.FirstLoad) {
                GridManager.TombstoneGrid.loadGrid();
                GridManager.TombstoneGrid.FirstLoad = true;
            }
            return;
        }
    });
};