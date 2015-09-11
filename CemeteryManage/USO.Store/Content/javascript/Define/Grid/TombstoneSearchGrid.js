
//创建一个上下文菜单
var TombstoneSearchGrid_RightMenu = Ext.create('Ext.menu.Menu', {
    items: [ActionBase.getAction('searchTombstoneSearch'), '-',
          ActionBase.getAction('refreshTombstoneSearch'),
        ActionBase.getAction('SearchTombstoneSearchJobManageLog')
    ]
});

Ext.define('chl.gird.TombstoneSearchGrid', {
    alternateClassName: ['TombstoneSearchGrid'],
    alias: 'widget.TombstoneSearchGrid',
    extend: 'chl.grid.BaseGrid',
    store: 'TombstoneSearchGridStoreId',
    actionBaseName: 'TombstoneSearchGridAction',
    features: [{
        ftype: 'grouping',
        myGroupName: '',
        groupHeaderTpl: ['{columnName:this.formatColumnName}: {name:this.formatName} ({rows.length})' + '项', {
            formatColumnName: function (columnName) {
                if (columnName == "AreaId") {
                    this.myGroupName = "AreaId";
                    return '区域';
                } else if (columnName == "PaymentStatusId") {
                    this.myGroupName = "PaymentStatusId";
                    return '付款状态';
                }
                return columnName;
            },
            formatName: function (name) {
                var dd = this.myGroupName;
                var record = null;
                if (this.myGroupName == 'AreaId') {
                    record = StoreManager.ComboStore.AreaStore.findRecord("Id", name);
                } else if (this.myGroupName == 'PaymentStatusId') {
                    record = StoreManager.ComboStore.PaymentStatusStore.findRecord("Id", name);
                }
                if (record != null) {
                    return record.data.Name;
                }
                return name;
            }
        }],
        disabled: true,
        groupByText: '按当前列分组',
        showGroupsText: '显示分组'
    }],
    listeners: {
        itemclick: function (grid, record, hitem, index, e, opts) {
            var me = this;
        },
        itemdblclick: function (grid, record, hitem, index, e, opts) {
            ActionBase.getAction('SearchTombstoneSearchJobManageLog').execute();
        },
        itemcontextmenu: function (view, rec, item, index, e, opts) {
            e.stopEvent();
            TombstoneSearchGrid_RightMenu.showAt(e.getXY());
        },
        beforeitemmousedown: function (view, record, item, index, e, options) {
            var me = this;
        },
        selectionchange: function (view, seles, op) {
            if (!seles[0])
                return;
            ActionBase.updateActions(GridManager.TombstoneSearchGrid.actionBaseName, seles);
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
        items: [ActionBase.getAction('searchTombstoneSearch'), '-',
           ActionBase.getAction('refreshTombstoneSearch') ,
        ActionBase.getAction('SearchTombstoneSearchJobManageLog'),
        '->', {
            fieldLabel: '按编号查找',
            text: '按编号查找',//用于控制工具栏使用
            width: 300,
            labelAlign: 'right',
            labelWidth: 80,
            xtype: 'searchfield',
            paramName: 'Alias',
            paramObject: true,
            regex: GlobalConfig.RegexController.regexTombstoneCode,
            regexText: '请输入3位或5位或7位编码',
            paramNameArr: ['Area', 'Row', 'Column'],
            itemId: 'TombstoneSearchGridSearchfieldId',
            listeners: {
                render: function () {
                    var me = this;
                    me.store = GridManager.TombstoneSearchGrid.getStore();
                }
            }
        }]
    }, {
        xtype: 'Pagingtoolbar',
        itemId: 'pagingtoolbarID',
        store: 'TombstoneSearchGridStoreId',
        dock: 'bottom',
        items: [{
            xtype: 'tbtext',
            text: '过滤:'
        }, {
            xtype: 'GridFilterMenuButton',
            itemId: 'menuAreaID',
            text: '全部区域',
            filterParam: {
                group: 'areaGroup',
                text: '全部区域',
                filterKey: 'AreaId',
                GridTypeName: 'TombstoneSearchGrid',
                store: StoreManager.ComboStore.AreaStore
            }
        }, {
            xtype: 'GridFilterMenuButton',
            itemId: 'menuPaymentID',
            text: '全部状态',
            filterParam: {
                group: 'areaGroup',
                text: '全部状态',
                filterKey: 'PaymentStatusId',
                GridTypeName: 'TombstoneSearchGrid',
                store: StoreManager.ComboStore.PaymentStatusStore
            }
        }, '-', {
            xtype: 'GridSelectCancelMenuButton',
            itemId: 'selectRecId',
            text: '选择',
            targetName: 'TombstoneSearchGrid'
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

        GlobalFun.SetGridTitle(me.up('#centerGridDisplayContainer'), store, "墓碑列表");
        ActionBase.updateActions(me.actionBaseName, me.getSelectionModel().getSelection());
    }
});

//根据传入参数创建客户表，返回自身
GridManager.CreateTombstoneSearchGrid = function (param) {
    ModelInfoManager.GridModelInfo.TombstoneSearchModelInfo.LoadDefault();
    ModelManager.GridModel.CretateTombstoneSearchGridModel();
    StoreManager.GridStore.CreateTombstoneSearchGridStore();

    var tmpArr = [];
    ModelInfoManager.GridModelInfo.TombstoneSearchModelInfo.TombstoneSearchColMap.each(function (item, index, alls) {
        tmpArr.push(item);
    });
    GridManager.TombstoneSearchGrid = Ext.create('chl.gird.TombstoneSearchGrid',
        GridManager.BaseGridCfg('TombstoneSearchGrid', 'TombstoneSearchGridState', tmpArr));
    if (param && param.needLoad) {
        GridManager.TombstoneSearchGrid.loadGrid();
    }
    return GridManager.TombstoneSearchGrid;
};

//加载SelectionChange事件
GridManager.SetTombstoneSearchGridSelectionChangeEvent = function (param) {
    GridManager.TombstoneSearchGrid.on('selectionchange', function (view, seles, op) {
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
                    name: 'Name',
                    validateOnBlur: false,
                    fieldLabel: '名称',
                    itemId: 'NameItemId'
                }, {
                    name: 'Alias',
                    fieldLabel: '别名',
                    itemId: 'AliasItemId'
                }, {
                    name: 'AreaEntity',
                    fieldLabel: '区域',
                    itemId: 'AreaEntityItemId'
                }]
            }]
        }]);

        southTab1.down('#formId').getForm().loadRecord(seles[0]);
        var areaEntityItem = southTab1.down('#AreaEntityItemId');
        areaEntityItem.setValue(seles[0].data.AreaEntity.Name);
        var paymentStatusEntityItem = southTab1.down('#PaymentStatusEntityItemId');
        paymentStatusEntityItem.setValue(seles[0].data.PaymentStatusEntity.Name);
        southTab1.doLayout();
    });
};


