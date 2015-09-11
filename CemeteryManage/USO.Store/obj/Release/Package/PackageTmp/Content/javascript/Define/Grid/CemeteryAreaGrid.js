
//创建一个上下文菜单
var CemeteryAreaGrid_RightMenu = Ext.create('Ext.menu.Menu', {
    items: [ActionBase.getAction('refreshCemeteryArea'), '-',
            ActionBase.getAction('addCemeteryArea'), ActionBase.getAction('editCemeteryArea'),
            ActionBase.getAction('delCemeteryArea'),
        '-',
             ActionBase.getAction('exportCemeteryArea')]
});

Ext.define('chl.gird.CemeteryAreaGrid', {
    alternateClassName: ['CemeteryAreaGrid'],
    alias: 'widget.CemeteryAreaGrid',
    extend: 'chl.grid.BaseGrid',
    store: 'CemeteryAreaGridStoreId',
    actionBaseName: 'CemeteryAreaGridAction',
    listeners: {
        itemclick: function (grid, record, hitem, index, e, opts) {
            var me = this;
        },
        itemdblclick: function (grid, record, hitem, index, e, opts) {
            ActionBase.getAction('editCemeteryArea').execute();
        },
        itemcontextmenu: function (view, rec, item, index, e, opts) {
            e.stopEvent();
            CemeteryAreaGrid_RightMenu.showAt(e.getXY());
        },
        beforeitemmousedown: function (view, record, item, index, e, options) {
            var me = this;
        },
        selectionchange: function (view, seles, op) {
            if (!seles[0])
                return;
            ActionBase.updateActions(GridManager.CemeteryAreaGrid.actionBaseName, seles);
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
        items: [ActionBase.getAction('refreshCemeteryArea'), '-',
            ActionBase.getAction('addCemeteryArea'), ActionBase.getAction('addTombstoneRowList'), ActionBase.getAction('editCemeteryArea'),
            ActionBase.getAction('delCemeteryArea')
            , '-',
             ActionBase.getAction('exportCemeteryArea'),
        '->', {
            fieldLabel: '按编号查找',
            text: '按编号查找',//用于控制工具栏使用
            width: 300,
            labelAlign: 'right',
            labelWidth: 80,
            xtype: 'searchfield',
            paramName: 'Alias',
            //store: searchStore,
            itemId: 'CemeteryAreaGridSearchfieldId',
            regex: GlobalConfig.RegexController.regexAreaCode,
            regexText: '请输入3位编码',
            listeners: {
                render: function () {
                    var me = this;
                    me.store = GridManager.CemeteryAreaGrid.getStore();
                }
            }
        }]
    }, {
        xtype: 'Pagingtoolbar',
        itemId: 'pagingtoolbarID',
        store: 'CemeteryAreaGridStoreId',
        dock: 'bottom',
        items: [{
            xtype: 'GridSelectCancelMenuButton',
            itemId: 'selectRecId',
            text: '选择',
            targetName: 'CemeteryAreaGrid'
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
        var container = me.up('#centerGridDisplayContainer');
        if (container) {
            GlobalFun.SetGridTitle(me.up('#centerGridDisplayContainer'), store, "区域列表");
            ActionBase.updateActions(me.actionBaseName, me.getSelectionModel().getSelection());
        }
       
    }
});

//根据传入参数创建客户表，返回自身
GridManager.CreateCemeteryAreaGrid = function (param) {
    ModelInfoManager.GridModelInfo.CemeteryAreaModelInfo.LoadDefault();
    ModelManager.GridModel.CretateCemeteryAreaGridModel();
    StoreManager.GridStore.CreateCemeteryAreaGridStore();

    var tmpArr = [];
    ModelInfoManager.GridModelInfo.CemeteryAreaModelInfo.CemeteryAreaColMap.each(function (item, index, alls) {
        tmpArr.push(item);
    });
    GridManager.CemeteryAreaGrid = Ext.create('chl.gird.CemeteryAreaGrid',
        GridManager.BaseGridCfg('CemeteryAreaGrid', 'CemeteryAreaGridState', tmpArr));
    if (param && param.needLoad) {
        GridManager.CemeteryAreaGrid.loadGrid();
    }
    return GridManager.CemeteryAreaGrid;
};

//加载SelectionChange事件
GridManager.SetCemeteryAreaGridSelectionChangeEvent = function (param) {
    GridManager.CemeteryAreaGrid.on('selectionchange', function (view, seles, op) {
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
                    fieldLabel: '名称',
                    itemId: 'NameItemId'
                }, {
                    name: 'Alias',
                    fieldLabel: '编号',
                    colspan: 2,
                    itemId: 'AliasItemId'
                }, {
                    name: 'Remark',
                    width: 800,
                    fieldStyle: GlobalConfig.Css.RemarkDisplay,
                    colspan: 3,
                    fieldLabel: '备注'
                }]
            }]
        }]);

        southTab1.down('#formId').getForm().loadRecord(seles[0]);
        southTab1.doLayout();
    });
};


//区域添加编辑窗口
Ext.define('chl.Grid.AddUpdateCemeteryAreaWin', {
    extend: 'Ext.window.Window',
    title: "添加区域",
    defaultFocus: 'NameItemId',
    iconCls: '',
    record: false,
    //border: false,
    height: 500,
    width: 500,
    layout: 'vbox',
    modal: true,
    resizable: false,
    items: [{
        xtype: 'form',
        itemId: 'formId',
        autoScroll: true,
        height: 450,
        width: 490,
        border: false,
        bodyPadding: 5,
        defaults: {
            xtype: 'fieldset',
            collapsible: true,
            defaultType: 'textfield',
            defaults: {
                labelAlign: 'right',
                labelPad: 15,
                width: 340,
                labelWidth: 125,
                maxLength: 100,
                maxLengthText: '最大长度为100'
            }
        },
        items: [{
            title: '基础信息',
            layout: {
                type: 'table',
                columns: 2
            },
            defaults: {
                labelAlign: 'right',
                labelPad: 15,
                width: 200,
                labelWidth: 80,
                maxLength: 200,
                maxLengthText: '最大长度为200'
            },
            items: [{
                name: 'Name',
                width: 300,
                colspan: 2,
                fieldLabel: '名称',
                itemId: 'NameItemId',
                validateOnBlur: false,
                allowBlank: false,
                blankText: '名称不能为空'
            }, {
                name: 'Alias',
                width: 300,
                colspan: 2,
                fieldLabel: '编号',
                itemId: 'AliasItemId',
                allowBlank: false,
                blankText: '不能为空'
            }, {
                xtype: 'combobox',
                colspan: 2,
                name: 'RowSort',
                fieldLabel: '排序顺序',
                store: StoreManager.ComboStore.AreaRowSortStore,
                queryMode: 'local',
                displayField: 'Name',
                valueField: 'Id',
                value: 'ASC',
                editable: false
            }, {
                xtype: 'textareafield',
                colspan: 2,
                name: 'Remark',
                width: 400,
                fieldLabel: '备注',
                itemId: 'RemarkId'
            }]
        }//基础信息end
        ]
    }],
    buttons: [{
        text: '重置',
        handler: function () {
            var me = this;
            var w = me.up('window');
            var f = w.down('#formId');
            f.getForm().reset();
            if (w.action == 'update') {
                var sm = w.grid.getSelectionModel();
                if (sm.hasSelection()) {
                    f.getForm().loadRecord(sm.getSelection()[0]);
                }
            }
        }
    }, {
        text: '确定',
        itemId: 'submit',
        handler: function () {
            var me = this;
            var w = me.up('window');
           
            var form = w.down('#formId').getForm();

            if (form.isValid()) {
                var url = w.action == "create" ? GlobalConfig.Controllers.CemeteryAreaGrid.addCemeteryArea : GlobalConfig.Controllers.CemeteryAreaGrid.updateCemeteryArea;
                form.submit({
                    url: url,
                    params: {
                        req: 'dataset',
                        dataname: 'CemeteryArea', // dataset名称，根据实际情况设置,数据库名
                        restype: 'json',
                        Id: w.record ? w.record.data.Id : 0,
                        action: w.action,
                        sessiontoken: GlobalFun.getSeesionToken()

                    },
                    success: function (form, action) {
                        w.grid.loadGrid();
                        w.close();

                    },
                    failure: function (form, action) {
                        if (!GlobalFun.errorProcess(action.result.code)) {
                            Ext.Msg.alert('失败', action.result.msg);
                        }
                    }

                });
            }
        }
    }, {
        text: '取消',
        handler: function () {
            var me = this;
            me.up('window').close();
        }
    }]
});