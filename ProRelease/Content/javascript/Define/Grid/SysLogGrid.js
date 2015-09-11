
//创建一个上下文菜单
var SysLogGrid_RightMenu = Ext.create('Ext.menu.Menu', {
    items: [ActionBase.getAction('searchSysLog'), '-',
            ActionBase.getAction('refreshSysLog')]
});

Ext.define('chl.gird.SysLogGrid', {
    alternateClassName: ['SysLogGrid'],
    alias: 'widget.SysLogGrid',
    extend: 'chl.grid.BaseGrid',
    store: 'SysLogGridStoreId',
    actionBaseName: 'SysLogGridAction',
    listeners: {
        itemclick: function (grid, record, hitem, index, e, opts) {
            var me = this;
        },
        itemdblclick: function (grid, record, hitem, index, e, opts) {
            ActionBase.getAction('editSysLog').execute();
        },
        itemcontextmenu: function (view, rec, item, index, e, opts) {
            e.stopEvent();
            SysLogGrid_RightMenu.showAt(e.getXY());
        },
        beforeitemmousedown: function (view, record, item, index, e, options) {
            var me = this;
        },
        selectionchange: function (view, seles, op) {
            if (!seles[0])
                return;
            ActionBase.updateActions(GridManager.SysLogGrid.actionBaseName, seles);
        }
    },
    columns: [],
    dockedItems: [{
        xtype: 'toolbar',
        itemId: 'toolbarID',
        dock: 'top',
        layout: {
            overflowHandler: 'Menu'
        },
        items: [ActionBase.getAction('searchSysLog'), '-',
            ActionBase.getAction('refreshSysLog'),
        '->', {
            fieldLabel: '按操作人查找',
            text: '按操作人查找',//用于控制工具栏使用
            width: 300,
            labelAlign: 'right',
            labelWidth: 80,
            xtype: 'searchfield',
            paramName: 'Name',
            paramObject: true,
            //minLength: 6,
            //minLengthText: '请输入6位编码',
            //maxLength: 6,
            //maxLengthText: '请输入6位编码',
            paramNameArr: ['User'],
            //store: searchStore,
            itemId: 'SysLogGridSearchfieldId',
            listeners: {
                render: function () {
                    var me = this;
                    me.store = GridManager.SysLogGrid.getStore();
                }
            }
        }]
    }, {
        xtype: 'Pagingtoolbar',
        itemId: 'pagingtoolbarID',
        store: 'SysLogGridStoreId',
        dock: 'bottom',
        items: [{
            xtype: 'tbtext',
            text: '过滤:'
        }, {
            xtype: 'GridFilterMenuButton',
            itemId: 'menuAreaID',
            text: '全部类型',
            filterParam: {
                group: 'areaGroup',
                text: '全部类型',
                filterKey: 'Type',
                GridTypeName: 'SysLogGrid',
                store: StoreManager.ComboStore.SysLogTypeStore
            }
        }, '-', {
            xtype: 'GridSelectCancelMenuButton',
            itemId: 'selectRecId',
            text: '选择',
            targetName: 'SysLogGrid'
        }
        ]
    }],
    initComponent: function () {
        var me = this;
        var filter = '';
        me.callParent(arguments);	// 调用父类方法

        ActionBase.setTargetView(me.actionBaseName, me);
        ActionBase.updateActions(me.actionBaseName, me.getSelectionModel().getSelection());
    },
    loadGrid: function (isSearch) {
        var me = this;
        var store = me.getStore();

        store.pageSize = GlobalConfig.GridPageSize;
        var sessiontoken = store.getProxy().extraParams.sessiontoken;
        if (!sessiontoken || sessiontoken.length == 0) {
            //return;
        }
        var filter = {};

        store.filterMap.each(function (key, value, length) {
            filter[key] = value;
        });
        store.getProxy().extraParams.filter = Ext.JSON.encode(filter);

        store.getProxy().extraParams.refresh = 1;

        store.loadPage(1);
        store.getProxy().extraParams.refresh = null;
        //store.filterMap.removeAtKey('filter');
        GlobalFun.SetGridTitle(me.up('#centerGridDisplayContainer'), store, "日志列表");
        ActionBase.updateActions(me.actionBaseName, me.getSelectionModel().getSelection());
    }
});

//根据传入参数创建客户表，返回自身
GridManager.CreateSysLogGrid = function (param) {
    ModelInfoManager.GridModelInfo.SysLogModelInfo.LoadDefault();
    ModelManager.GridModel.CretateSysLogGridModel();
    StoreManager.GridStore.CreateSysLogGridStore();

    var tmpArr = [];
    ModelInfoManager.GridModelInfo.SysLogModelInfo.SysLogColMap.each(function (item, index, alls) {
        tmpArr.push(item);
    });
    GridManager.SysLogGrid = Ext.create('chl.gird.SysLogGrid',
        GridManager.BaseGridCfg('SysLogGrid', 'SysLogGridState', tmpArr));
    if (param && param.needLoad) {
        GridManager.SysLogGrid.loadGrid();
    }
    return GridManager.SysLogGrid;
};

//加载SelectionChange事件
GridManager.SetSysLogGridSelectionChangeEvent = function (param) {
    GridManager.SysLogGrid.on('selectionchange', function (view, seles, op) {
        if (!seles[0])
            return;
        var southTab1 = GlobalConfig.ViewPort.down('#southTab1');
        southTab1.removeAll();
        southTab1.add([{
            xtype: 'form',
            itemId: 'formId',
            border: false,
            bodyPadding: 5,
            defaults: {
                xtype: 'fieldset',
                collapsible: true,
                defaultType: 'displayfield',
                defaults: {
                    labelAlign: 'right',
                    labelStyle: 'color:#04408C;font-weight:bold;',
                    labelPad: 15,
                    width: 280,
                    labelWidth: 100
                }
            },
            items: [{//基础信息fieldset
                title: '基础信息',
                layout: {
                    type: 'table',
                    columns: 3
                },
                items: [{
                    name: 'TypeString',
                    fieldLabel: '日志类型'
                }, {
                    name: 'ControlName',
                    fieldLabel: '操作记录'
                }, {
                    name: 'UserEntity',
                    itemId: 'UserEntityItemId',
                    fieldLabel: '操作人'
                }, {
                    name: 'Date',
                    colspan: 3,
                    fieldLabel: '操作时间'
                }, {
                    name: 'Content',
                    width: 800,
                    fieldStyle: GlobalConfig.Css.RemarkDisplay,
                    colspan: 3,
                    fieldLabel: '日志文本'
                }]
            }]
        }]);

        southTab1.down('#formId').getForm().loadRecord(seles[0]);
        var userEntityItem = southTab1.down('#UserEntityItemId');
        userEntityItem.setValue(seles[0].data.UserEntity.Name);
        southTab1.doLayout();
    });
};

