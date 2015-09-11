

// BaseEnumListTree类
Ext.define('chl.tree.BaseEnumListTree', {
    alias: 'widget.BaseEnumListTree',
    itemId: 'BaseEnumListTree',
    title: '基础数据维护',
    iconCls: 'faxTitle',
    extend: 'chl.tree.BaseTree',
    clearOnLoad: false,
    alternateClassName: ['BaseEnumListTree'],
    store: 'BaseEnumListTreeStoreId',
    ddGroup: 'BaseEnumListTreeDDGp',
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
                if (!TreeManager.BaseEnumListTree.getSelectionModel().hasSelection()) {
                    TreeManager.BaseEnumListTree.getSelectionModel().select(0, true);
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
TreeManager.CreateBaseEnumListTree = function (param) {
    TreeManager.BaseEnumListTree = Ext.create('chl.tree.BaseEnumListTree');
    if (param && param.needLoad) {
        TreeManager.BaseEnumListTree.getStore().load({
            callback: function () {
                var rootNode = TreeManager.BaseEnumListTree.getRootNode();
                rootNode.expand();
            }
        });
    }
    return TreeManager.BaseEnumListTree;
};

TreeManager.SetBaseEnumListTreeSelectionChangeEvent = function (param) {
    TreeManager.BaseEnumListTree.on('selectionchange', function (view, seles, op) {
        if (!seles[0])
            return;
        //基础数据1
        if (seles[0].data.id == 1) {
            var cardPanel = GlobalConfig.ViewPort.down('#centerGridDisplayContainer');
            //cardPanel.getLayout().setActiveItem(0);
            return;
        }
       
    });
};