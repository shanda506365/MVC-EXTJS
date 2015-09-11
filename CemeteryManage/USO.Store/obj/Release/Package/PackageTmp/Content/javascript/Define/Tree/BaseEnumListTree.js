

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
            //alert(rec.data.parent.Text);
            e.stopEvent();
            //folderTree_RightMenu.showAt(e.getXY());
        },
        expand: function (tree, opts) {
            if (!tree.getSelectionModel().hasSelection()) {
                tree.getSelectionModel().select(0, true);
            } else {
                tree.fireEvent("selectionchange", tree, tree.getSelectionModel().getSelection(), opts);
            }
        },
        boxready: function (com, width, height, opts) {
            (new Ext.util.DelayedTask()).delay(2000, function () {
                if (TreeManager.MainItemListTree) {
                    return;
                }
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
        //自动恢复收缩状态
        GlobalFun.RefreshDetailCollapseState();
        //用户管理
        if (seles[0].data.id == 1) {
            GlobalFun.TreeSelChangeGrid('UserGrid', GridManager.UserGrid, '用户列表');
            return;
        }
        //角色管理
        if (seles[0].data.id == 2) {
            GlobalFun.TreeSelChangeGrid('RoleGrid', GridManager.RoleGrid, '角色列表');
            return;
        }
        //墓区设置Df
        if (seles[0].data.id == 30) {
            GlobalFun.TreeSelChangeGrid('AreaSetDfGrid', GridManager.AreaSetDfGrid, '墓区设置',true);
            return;
        }
        //墓碑区域管理
        if (seles[0].data.id == 3001) {
            GlobalFun.TreeSelChangeGrid('CemeteryAreaGrid', GridManager.CemeteryAreaGrid, '墓碑区域列表');
            return;
        }
        //墓碑管理
        if (seles[0].data.id == 3002) {
            GlobalFun.TreeSelChangeGrid('TombstoneGrid', GridManager.TombstoneGrid, '墓碑列表');
            return;
        }
        //日志管理
        if (seles[0].data.id == 4) {
            GlobalFun.TreeSelChangeGrid('SysLogGrid', GridManager.SysLogGrid, '日志列表');
            return;
        }

    });
};