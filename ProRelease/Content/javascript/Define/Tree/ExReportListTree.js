// ExReportListTree类
Ext.define('chl.tree.ExReportListTree', {
    alias: 'widget.ExReportListTree',
    itemId: 'ExReportListTree',
    title: '报表统计',
    iconCls: 'faxTitle',
    extend: 'chl.tree.BaseTree',
    clearOnLoad: false,
    alternateClassName: ['ExReportListTree'],
    store: 'ExReportListTreeStoreId',
    ddGroup: 'ExReportListTreeDDGp',
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
        }

    },
    initComponent: function () {
        var me = this;
        me.callParent(arguments);

    }
});

//根据传入参数创建客户表，返回自身
TreeManager.CreateExReportListTree = function (param) {
    TreeManager.ExReportListTree = Ext.create('chl.tree.ExReportListTree');
    if (param && param.needLoad) {
        TreeManager.ExReportListTree.getStore().load({
            callback: function () {
                var rootNode = TreeManager.ExReportListTree.getRootNode();
                rootNode.expand();
            }
        });
    }
    return TreeManager.ExReportListTree;
};

TreeManager.SetExReportListTreeSelectionChangeEvent = function (param) {
    TreeManager.ExReportListTree.on('selectionchange', function (view, seles, op) {
        if (!seles[0])
            return;
        //统计1
        if (seles[0].data.id == 1) {
            var cardPanel = GlobalConfig.ViewPort.down('#centerGridDisplayContainer');
            //cardPanel.getLayout().setActiveItem(0);
            return;
        }
        //报表2
        if (seles[0].data.id == 2) {
            var cardPanel = GlobalConfig.ViewPort.down('#centerGridDisplayContainer');
            //cardPanel.getLayout().setActiveItem(1);
            //if (!GridManager.TombstoneGrid.FirstLoad) {
            //    GridManager.TombstoneGrid.loadGrid();
            //    GridManager.TombstoneGrid.FirstLoad = true;
            //}
            return;
        }
    });
};