Ext.define('chl.gird.RenewManageGrid', {
    alternateClassName: ['RenewManageGrid'],
    alias: 'widget.RenewManageGrid',
    extend: 'chl.grid.BaseGrid',
    store: 'RenewManageGridStoreId',
    stateful: false,
    actionBaseName: 'RenewManageGridAction',
    listeners: {
        itemclick: function (grid, record, hitem, index, e, opts) {
            var me = this;
        },
        itemdblclick: function (grid, record, hitem, index, e, opts) {
            //ActionBase.getAction('editRenewManage').execute();
            Ext.create('Ext.window.Window', {
                title: '查看历史记录',
                iconCls: '',
                height: 240,
                width: 700,
                modal: true,
                resizable: false,
                autoScroll:true,
                defaults: {
                    labelAlign: 'right',
                    labelStyle: 'color:#04408C;font-weight:bold;',
                    labelPad: 15,
                    width: 280,
                    labelWidth: 100
                },
                items: [{
                    xtype: 'displayfield',
                    fieldLabel: '时间',
                    value: record.data.Date
                }, {
                    width: 660,
                    xtype: 'displayfield',
                    fieldStyle: GlobalConfig.Css.RemarkDisplay,
                    fieldLabel: '操作',
                    value: record.data.Content
                }, {
                    name: 'Remark',
                    width: 660,
                    xtype: 'displayfield',
                    fieldStyle: GlobalConfig.Css.RemarkDisplay,
                    fieldLabel: '备注',
                    value: record.data.Remark
                }],
                buttons: [{
                    text: '关闭',
                    handler: function (com) {
                        var win = com.up('window');
                        win.close();
                    }
                }]
            }).show();
        },
        itemcontextmenu: function (view, rec, item, index, e, opts) {
            e.stopEvent();
        },
        beforeitemmousedown: function (view, record, item, index, e, options) {
            var me = this;
        },
        selectionchange: function (view, seles, op) {
            if (!seles[0])
                return;
            //ActionBase.updateActions('RenewManageGridAction', seles);
        }
    },
    columns: [{
        text: '时间',
        dataIndex: 'Date',
        sortable: false,
        renderer: GlobalFun.UpdateRecordForShortDate,
        width: 100
    }, {
        text: '操作',
        dataIndex: 'Content',
        sortable: false,
        renderer: GlobalFun.UpdateRecord,
        width: 500
    }, {
        text: '备注',
        dataIndex: 'Remark',
        sortable: false,
        renderer: GlobalFun.UpdateRecord,
        width: 400
    }],
    dockedItems: [{
        xtype: 'toolbar',
        itemId: 'toolbarID',
        dock: 'top',
        layout: {
            overflowHandler: 'Menu'
        },
        items: [ActionBase.getAction('refreshRenewManage'),
            ActionBase.getAction('addRenewManage')]//, ActionBase.getAction('editRenewManage')]
    }, {
        xtype: 'Pagingtoolbar',
        itemId: 'pagingtoolbarID',
        store: 'RenewManageGridStoreId',
        dock: 'bottom',
        items: []
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

        ActionBase.updateActions(me.actionBaseName, me.getSelectionModel().getSelection());
    }
});