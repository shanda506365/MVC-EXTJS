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
        //自动恢复收缩状态
        //预定、出售、落葬管理
        if (seles[0].data.id == 201 || seles[0].data.id == 202 || seles[0].data.id == 203) {
            
        } else {
            GlobalFun.RefreshDetailCollapseState();
        }
        
        ////客户管理
        //if (seles[0].data.id == 1) {
        //    GlobalFun.TreeSelChangeGrid('CustomerGrid', GridManager.CustomerGrid, '客户列表');
        //    return;
        //}
        
        //业务管理
        if (seles[0].data.id == 2) {
            GlobalFun.TreeSelChangeGrid('JobManageDfGrid', GridManager.AreaSetDfGrid, '业务管理', true);
            return;
        }
        
        //预定、出售、落葬管理
        if (seles[0].data.id == 201 || seles[0].data.id == 202 || seles[0].data.id == 203) {
            var title = "";
            if (seles[0].data.id == 201) {
                title = "墓碑预定";
            } else if (seles[0].data.id == 202) {
                title = "墓碑维护";
            }
            else if (seles[0].data.id == 203) {
                title = "墓碑落葬";
            }
            //自动收缩
            GlobalFun.DetailCollapse();
            GlobalFun.TreeSelChangeGrid('AreaTombstoneImagePanel', GridManager.AreaTombstoneImagePanel, title, true);
            GridManager.AreaTombstoneImagePanel.down('#tombCode').inputEl.focus(100);
            return;
        }
        
        //墓碑查询
        if (seles[0].data.id == 3) {
            GlobalFun.TreeSelChangeGrid('TombstoneSearchGrid', GridManager.TombstoneSearchGrid, '墓碑列表');
            return;
        }
    });
};