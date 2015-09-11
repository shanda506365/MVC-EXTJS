
//创建一个上下文菜单
var RoleGrid_RightMenu = Ext.create('Ext.menu.Menu', {
    items: [ActionBase.getAction('searchRole'), '-',
            ActionBase.getAction('refreshRole'), '-',
            ActionBase.getAction('addRole'), ActionBase.getAction('editRole')]
});

Ext.define('chl.gird.RoleGrid', {
    alternateClassName: ['RoleGrid'],
    alias: 'widget.RoleGrid',
    extend: 'chl.grid.BaseGrid',
    store: 'RoleGridStoreId',
    actionBaseName: 'RoleGridAction',
    listeners: {
        itemclick: function (grid, record, hitem, index, e, opts) {
            var me = this;
        },
        itemdblclick: function (grid, record, hitem, index, e, opts) {
            ActionBase.getAction('editRole').execute();
        },
        itemcontextmenu: function (view, rec, item, index, e, opts) {
            e.stopEvent();
            RoleGrid_RightMenu.showAt(e.getXY());
        },
        beforeitemmousedown: function (view, record, item, index, e, options) {
            var me = this;
        },
        selectionchange: function (view, seles, op) {
            if (!seles[0])
                return;
            ActionBase.updateActions(GridManager.RoleGrid.actionBaseName, seles);
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
        items: [ActionBase.getAction('searchRole'), '-',
            ActionBase.getAction('refreshRole'), '-',
            ActionBase.getAction('addRole'), ActionBase.getAction('editRole'),
        '->', {
            fieldLabel: '按名称查找',
            text: '按名称查找',//用于控制工具栏使用
            width: 300,
            labelAlign: 'right',
            labelWidth: 80,
            xtype: 'searchfield',
            paramName: 'Name',
            //paramObject: true,
            //minLength: 6,
            //minLengthText: '请输入6位编码',
            //maxLength: 6,
            //maxLengthText: '请输入6位编码',
            //paramNameArr: ['Area', 'Row', 'Column'],
            //store: searchStore,
            itemId: 'RoleGridSearchfieldId',
            listeners: {
                render: function () {
                    var me = this;
                    me.store = GridManager.RoleGrid.getStore();
                }
            }
        }]
    }, {
        xtype: 'Pagingtoolbar',
        itemId: 'pagingtoolbarID',
        store: 'RoleGridStoreId',
        dock: 'bottom',
        items: [{
            xtype: 'GridSelectCancelMenuButton',
            itemId: 'selectRecId',
            text: '选择',
            targetName: 'RoleGrid'
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
        GlobalFun.SetGridTitle(me.up('#centerGridDisplayContainer'), store, "角色列表");
        ActionBase.updateActions(me.actionBaseName, me.getSelectionModel().getSelection());
    }
});

//根据传入参数创建客户表，返回自身
GridManager.CreateRoleGrid = function (param) {
    ModelInfoManager.GridModelInfo.RoleModelInfo.LoadDefault();
    ModelManager.GridModel.CretateRoleGridModel();
    StoreManager.GridStore.CreateRoleGridStore();

    var tmpArr = [];
    ModelInfoManager.GridModelInfo.RoleModelInfo.RoleColMap.each(function (item, index, alls) {
        tmpArr.push(item);
    });
    GridManager.RoleGrid = Ext.create('chl.gird.RoleGrid',
        GridManager.BaseGridCfg('RoleGrid', 'RoleGridState', tmpArr));
    if (param && param.needLoad) {
        GridManager.RoleGrid.loadGrid();
    }
    return GridManager.RoleGrid;
};

//加载SelectionChange事件
GridManager.SetRoleGridSelectionChangeEvent = function (param) {
    GridManager.RoleGrid.on('selectionchange', function (view, seles, op) {
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
                    colspan:3,
                    fieldLabel: '名称'
                }, {
                    name: 'FunctionsString',
                    width: 800,
                    fieldStyle: GlobalConfig.Css.RemarkDisplay,
                    colspan: 3,
                    fieldLabel: '所有功能'
                }]
            }]
        }]);

        southTab1.down('#formId').getForm().loadRecord(seles[0]);
        southTab1.doLayout();
    });
};


//角色添加编辑窗口
Ext.define('chl.Grid.AddUpdateRoleWin', {
    extend: 'Ext.window.Window',
    title: "添加角色",
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
                fieldLabel: '名称',
                itemId: 'NameItemId',
                validateOnBlur: false,
                allowBlank: false,
                blankText: '名称不能为空'
            }]
        }, {
            title: '功能权限信息',
            itemId: 'functionItemContainer',
            checkboxToggle: true,
            onCheckChange: GlobalFun.onCheckChange,
            createCheckboxCmp: GlobalFun.createCheckboxCmp,
            //layout: {
            //    type: 'table',
            //    columns: 3
            //},
            layout: 'vbox',
            //defaults: {
            //    xtype:'checkbox',
            //    labelAlign: 'right',
            //    width: 120
            //},
            items: []
        }]
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
                var checkBoxList = WindowManager.AddUpdateRoleWin.down("#functionItemContainer").query('checkbox');
                var funIdList = [];
                Ext.Array.each(checkBoxList, function (item, index) {
                    if (item.getValue() == true) {
                        if (item.itemId && item.itemId.indexOf('functionItemId_') != -1) {
                            funIdList.push(item.itemId.replace('functionItemId_', ''));
                        }
                    }
                });
               
                var url = w.action == "create" ? GlobalConfig.Controllers.RoleGrid.addRole : GlobalConfig.Controllers.RoleGrid.updateRole;
                form.submit({
                    url: url,
                    params: {
                        req: 'dataset',
                        dataname: 'AddUpdateRole', // dataset名称，根据实际情况设置,数据库名
                        restype: 'json',
                        Id: w.record ? w.record.data.Id : 0,
                        FunId:funIdList.join(','),
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