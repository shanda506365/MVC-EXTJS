


Ext.onReady(function () {
    Ext.getBody().mask('正在加载...');
    //修正Bug
    GlobalFun.fixedBugs();
    //创建Cookie状态保存
    Ext.state.Manager.setProvider(Ext.create('Ext.state.CookieProvider', {
        expires: new Date(new Date().getTime() + (1000 * 60 * 60 * 24 * 30))
    }));
    //初始化快捷提示
    Ext.tip.QuickTipManager.init();
    //获取当前登录用户信息
    var LoadCurrUserInfo = function (callBack) {
        //获取当前登录用户信息
        var param = {
            sessiontoken: GlobalFun.getSeesionToken()
        };
        // 调用
        WsCall.call(GlobalConfig.Controllers.User.GetCurrUserInfo, 'GetCurrUserInfo', param, function (response, opts) {
            GlobalConfig.CurrUserInfo = response.ResultOutDto;
            callBack();
        }, function (response, opts) {
            if (!GlobalFun.errorProcess(response.code)) {
                Ext.Msg.alert('获取用户权限失败', response.msg);
            }
        }, false);
    };
    //创建主数据界面
    var LoadMainView = function () {

        if (GlobalConfig.CurrUserInfo == '') {
            Ext.Msg.alert('登录失败', '登录失败！无效的登录信息', function () {
                GlobalFun.ReDirectUrl("UserLogin");
            });
            return;
        }

        if (GlobalFun.IsAllowFun('陵园系统')) {
            //if (GlobalFun.IsAllowFun('客户管理')) {
            //    //创建客户管理
            //    GridManager.CreateCustomerGrid({ needLoad: false });
            //    GridManager.SetCustomerGridSelectionChangeEvent();
            //}
            //if (GlobalFun.IsAllowFun('业务管理')) {
            //创建Df表格
            GridManager.JobManageDfGrid = Ext.create('chl.dfgrid.JobManageDfGrid');
            var roleArr = [];
            if (GlobalFun.IsAllowFun('墓碑预订')) {
                roleArr.push(['201', '墓碑预订']);
            }
            if (GlobalFun.IsAllowFun('墓碑维护')) {
                roleArr.push(['202', '墓碑维护']);
            }
            if (GlobalFun.IsAllowFun('墓碑落葬')) {
                roleArr.push(['203', '墓碑落葬']);
            }
            GridManager.JobManageDfGrid.getStore().loadData(roleArr);
            if (GlobalFun.IsAllowFun('墓碑维护') || GlobalFun.IsAllowFun('墓碑预订')
                               || GlobalFun.IsAllowFun('墓碑落葬')) {
                //创建业务管理图形化展示
                GridManager.AreaTombstoneImagePanel = Ext.create("chl.exUnit.AreaTombstoneImagePanel");
            }
            //if (GlobalFun.IsAllowFun('墓碑落葬')) {//加载落葬人表
                //创建落葬人表
               // GridManager.CreateBuryManGrid({ needLoad: false });
                //GridManager.SetBuryManGridSelectionChangeEvent();
            //}
            //}
           
            //创建墓碑查询
            GridManager.CreateTombstoneSearchGrid({ needLoad: false });
            GridManager.SetTombstoneSearchGridSelectionChangeEvent();
            
            //创建主目录树
            TreeManager.CreateMainItemListTree({ needLoad: true });
            TreeManager.SetMainItemListTreeSelectionChangeEvent();
        }

        if (GlobalFun.IsAllowFun('基础数据维护')) {
            if (GlobalFun.IsAllowFun('用户管理')) {
                //创建用户管理
                GridManager.CreateUserGrid({ needLoad: false });
                GridManager.SetUserGridSelectionChangeEvent();
            }
            if (GlobalFun.IsAllowFun('日志管理')) {
                //创建日志管理
                GridManager.CreateSysLogGrid({ needLoad: false });
                GridManager.SetSysLogGridSelectionChangeEvent();
            }
            if (GlobalFun.IsAllowFun('角色管理')) {
                //创建角色管理
                GridManager.CreateRoleGrid({ needLoad: false });
                GridManager.SetRoleGridSelectionChangeEvent();
            }
            //创建Df表格
            GridManager.AreaSetDfGrid = Ext.create('chl.dfgrid.AreaSetDfGrid');
            if (GlobalFun.IsAllowFun('墓碑区域管理')) {
                //创建墓碑区域管理
                GridManager.CreateCemeteryAreaGrid({ needLoad: false });
                GridManager.SetCemeteryAreaGridSelectionChangeEvent();
            }
            if (GlobalFun.IsAllowFun('墓碑管理')) {
                //创建墓碑管理
                GridManager.CreateTombstoneGrid({ needLoad: false });
                GridManager.SetTombstoneGridSelectionChangeEvent();
            }

            //创建基础数据维护树
            TreeManager.CreateBaseEnumListTree({ needLoad: true });
            TreeManager.SetBaseEnumListTreeSelectionChangeEvent();
        }




        //创建报表统计树
        //TreeManager.CreateExReportListTree({ needLoad: true });
        //TreeManager.SetExReportListTreeSelectionChangeEvent();

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
                //stateId: 'northStateId',
                stateful: false,
                collapsed: true,
                height: 100,
                items: [{
                    xtype: 'container',
                    html: '暂无公告'
                }]
            }, {
                region: 'west',
                stateId: 'westStateId',
                stateful: true,
                iconCls: 'viewportMainWestTitle',
                preventHeader: true,
                title: '管理菜单',
                layout: {
                    type: 'accordion',
                    animate: false,
                    activeOnTop: false,
                    collapseFirst: false,
                    titleCollapse: true
                },
                width: 200,
                listeners: {
                    boxready: function (com, width, height, opts) {
                        if (GlobalFun.IsAllowFun('陵园系统')) {
                            com.add(TreeManager.MainItemListTree);
                        }
                        if (GlobalFun.IsAllowFun('基础数据维护')) {
                            com.add(TreeManager.BaseEnumListTree);
                        }
                    }
                },
                items: []
            }, {
                region: 'center',
                stateId: 'centerStateId',
                stateful: true,
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
                    stateId: 'centercenterStateId',
                    stateful: true,
                    itemId: 'centerGridDisplayContainer',
                    title: '信息',
                    layout: 'card',
                    height: 400,
                    listeners: {
                        boxready: function (com, width, height, opts) {
                            var LoadAreaSetDfGrid = false;
                            //if (GlobalFun.IsAllowFun('客户管理')) {
                            //    com.add(GridManager.CustomerGrid);
                            //}
                            //墓碑查询
                            com.add(GridManager.TombstoneSearchGrid);
                            if (GlobalFun.IsAllowFun('墓碑管理')) {
                                LoadAreaSetDfGrid = true;
                                com.add(GridManager.TombstoneGrid);
                            }
                            if (GlobalFun.IsAllowFun('用户管理')) {
                                com.add(GridManager.UserGrid);
                            }
                            if (GlobalFun.IsAllowFun('日志管理')) {
                                com.add(GridManager.SysLogGrid);
                            }
                            if (GlobalFun.IsAllowFun('角色管理')) {
                                com.add(GridManager.RoleGrid);
                            }
                            if (GlobalFun.IsAllowFun('墓碑区域管理')) {
                                LoadAreaSetDfGrid = true;
                                com.add(GridManager.CemeteryAreaGrid);
                            }
                            if (LoadAreaSetDfGrid) {
                                com.add(GridManager.AreaSetDfGrid);
                            }
                            //创建业务管理图形化展示
                            com.add(GridManager.JobManageDfGrid);//dfGrid
                            if (GlobalFun.IsAllowFun('墓碑维护') || GlobalFun.IsAllowFun('墓碑预订')
                                || GlobalFun.IsAllowFun('墓碑落葬')) {
                                com.add(GridManager.AreaTombstoneImagePanel);
                            }
                        }
                    },
                    items: []
                }, {
                    region: 'south',
                    stateId: 'centersouthStateId',
                    itemId: 'centerGridDetailContainer',
                    stateful: true,
                    //title: '详情',
                    preventHeader: true,
                    height: 200,
                    collapsed: false,
                    bodyBoder: false,
                    xtype: 'tabpanel',
                    frame: false,
                    border: false,
                    defaults: {
                        border: false
                    },
                    items: [{
                        title: '详细信息',
                        layout: 'auto',
                        itemId: 'southTab1',
                        autoScroll: true,
                        items: []
                    }, {
                        title: '陵园介绍',
                        autoScroll: true,
                        itemId: 'southTab2',
                        padding: '5px 5px 5px 20px',
                        items: [{
                            xtype: 'container',
                            width: 800,
                            html: HomeIntroText
                        }]
                    }]
                }]
            }, {
                region: 'south',
                stateId: 'southStateId',
                stateful: false,
                height: 30,
                xtype: 'toolbar',
                split: false,
                collapsible: false,
                items: ['->', '-', {
                    xtype: 'button',
                    width: 80,
                    iconCls: 'logout',
                    text: '退出登录',
                    tooltip: '退出登录当前用户',
                    handler: function () {
                        GlobalConfig.newMessageBox.confirm('退出登录', '确定要退出登录当前用户吗?', function (btn) {
                            if (btn == 'yes') {
                                var param = {};
                                // 调用
                                WsCall.call(GlobalConfig.Controllers.User.UserLoginOut, 'UserLoginOut', param, function (response, opts) {
                                    Ext.util.Cookies.clear("login_sessiontoken");
                                    //跳转页面
                                    Ext.getBody().mask("请稍候...");
                                    (new Ext.util.DelayedTask()).delay(20, function () {
                                        GlobalFun.ReDirectUrl("");
                                    });
                                }, function (response, opts) {
                                    if (!GlobalFun.errorProcess(response.code)) {
                                        Ext.Msg.alert('注销失败', response.msg);
                                    }
                                }, false);
                            }

                        });
                    }
                }]
            }]
        });
    };
    //计数器
    var ComboSucCount = 0, ComboSucTotal = 8;
    //计数器回调方法
    var combInitCallBack = function (records, operation, success) {
        if (success) {
            ComboSucCount++;
            if (ComboSucCount == ComboSucTotal) {
                //获取当前登录用户信息
                LoadCurrUserInfo(function () {
                    LoadMainView();
                    GlobalFun.Heartbeat();
                    (new Ext.util.DelayedTask()).delay(2000, function () {
                        Ext.getBody().unmask();
                    });

                });
            }
        }
    };

    //加载基础数据
    var LoadBaseEnumStore = function () {
        //部门
        StoreManager.ComboStore.DepartmentStore.load();
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
        StoreManager.ComboStore.SecurityLevelStore.load();
        //服务等级Sotre
        StoreManager.ComboStore.ServiceLevelStore.load();
        //客户类别
        StoreManager.ComboStore.CustomerTypeStore.load(combInitCallBack);
        //国籍Store
        StoreManager.ComboStore.NationalityStore.load(combInitCallBack);
        //客户状态Store
        StoreManager.ComboStore.CustomerStatusStore.load(combInitCallBack);
    };

    LoadBaseEnumStore();

});


