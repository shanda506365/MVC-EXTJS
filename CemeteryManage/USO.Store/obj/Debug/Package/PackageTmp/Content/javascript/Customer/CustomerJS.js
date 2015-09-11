


Ext.onReady(function () {
    Ext.getBody().mask('正在加载...');
    //修正Bug
    GlobalFun.fixedBugs();
    
   
    
    //创建主数据界面
    var LoadMainView = function() {
        //创建客户管理
        GridManager.CreateCustomerGrid({ needLoad: false });
        GridManager.SetCustomerGridSelectionChangeEvent();
        //创建墓碑管理
        GridManager.CreateTombstoneGrid({ needLoad: false });
        GridManager.SetTombstoneGridSelectionChangeEvent();
        //创建主目录树
        TreeManager.CreateMainItemListTree({ needLoad: true });
        TreeManager.SetMainItemListTreeSelectionChangeEvent();
        //创建报表统计树
        TreeManager.CreateExReportListTree({ needLoad: true });
        TreeManager.SetExReportListTreeSelectionChangeEvent();
        //创建基础数据维护树
        TreeManager.CreateBaseEnumListTree({ needLoad: true });
        TreeManager.SetBaseEnumListTreeSelectionChangeEvent();
        //StoreManager.ComboStore.CustomerTypeStore.load();
        

        //GridManager.CustomerGrid.getStore().filterMap.add('Id', 12);

        GlobalConfig.ViewPort = Ext.create('Ext.container.Viewport', {
            layout: {
                type: 'border'
            },
            defaults: {
                split: true,
                collapsible: true
            },
            items: [{
                    region: 'north',
                    title: '公告栏',
                    collapsed: true,
                    height: 100
                }, {
                    region: 'west',
                    iconCls: 'viewportMainWestTitle',
                    preventHeader:true,
                    title: '管理菜单',
                    layout: {
                        type: 'accordion',
                        animate: false,
                        activeOnTop: false,
                        collapseFirst: false,
                        titleCollapse: true
                    },
                    width: 200,
                    items: [TreeManager.MainItemListTree,
                            TreeManager.BaseEnumListTree,
                            TreeManager.ExReportListTree]
                }, {
                    region: 'center',
                    //title: '主显示区域',
                    collapsible: false,
                    layout: {
                        type: 'border'
                    },
                    defaults: {
                        split: true,
                        collapsible: true,
                        border: false
                    },
                    items: [{
                            region: 'center',
                            itemId: 'centerGridDisplayContainer',
                            title: '信息',
                            layout: 'card',
                            height: 400,
                            items: [GridManager.CustomerGrid, GridManager.TombstoneGrid]
                        }, {
                            region: 'south',
                            title: '详情',
                            height: 200,
                            collapsed: false,
                            bodyBoder: false,
                            items: [{
                                xtype: 'tabpanel',
                                frame: false,
                                bodyBoder: false,
                                border: false,
                                defaults: {
                                    border: false
                                },
                                items: [{
                                        title: '详细信息',
                                        itemId: 'southTab1',
                                        items: []
                                    }, {
                                        title: '详细信息1',
                                        itemId: 'southTab2'
                                    }]
                            }]
                        }]
                }, {
                    region: 'south',
                    height: 30,
                    items: [{
                        xtype: 'toolbar'
                    }]
                }]
        });
    };
    //计数器
    var ComboSucCount = 0, ComboSucTotal = 10;
    //计数器回调方法
    var combInitCallBack = function(records, operation, success) {
        ComboSucCount++;
        if (ComboSucCount == ComboSucTotal) {
            Ext.getBody().unmask();
            LoadMainView();
        }
    };
    
    //加载基础数据
    var LoadBaseEnumStore = function () {
        //付款状态
        StoreManager.ComboStore.PaymentStatusStore.load(combInitCallBack);
        //墓碑类别Store
        StoreManager.ComboStore.TombstoneTypeStore.load(combInitCallBack);
        //区域Sotre
        StoreManager.ComboStore.AreaStore.load(combInitCallBack);
        //行Sotre
        StoreManager.ComboStore.RowStore.load(combInitCallBack);
        //列Sotre
        StoreManager.ComboStore.ColumnStore.load(combInitCallBack);
        //保密等级Sotre
        StoreManager.ComboStore.SecurityLevelStore.load(combInitCallBack);
        //服务等级Sotre
        StoreManager.ComboStore.ServiceLevelStore.load(combInitCallBack);
        //客户类别
        StoreManager.ComboStore.CustomerTypeStore.load(combInitCallBack);
        //国籍Store
        StoreManager.ComboStore.NationalityStore.load(combInitCallBack);
        //客户状态Store
        StoreManager.ComboStore.CustomerStatusStore.load(combInitCallBack);
    };

    LoadBaseEnumStore();

});


