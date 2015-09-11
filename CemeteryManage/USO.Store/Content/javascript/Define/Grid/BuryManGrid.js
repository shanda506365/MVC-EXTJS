Ext.define('chl.gird.BuryManGrid', {
    alternateClassName: ['BuryManGrid'],
    alias: 'widget.BuryManGrid',
    extend: 'chl.grid.BaseGrid',
    store: 'BuryManGridStoreId',
    stateful: false,
    actionBaseName: 'BuryManGridAction',
    listeners: {
        itemclick: function (grid, record, hitem, index, e, opts) {
            var me = this;
        },
        itemdblclick: function (grid, record, hitem, index, e, opts) {
            ActionBase.getAction('editBuryMan').execute();
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
            ActionBase.updateActions('BuryManGridAction', seles);
        }
    },
    columns: [{
        text: '落葬时间',
        dataIndex: 'BuryDate',
        renderer: GlobalFun.UpdateRecordForShortDate,
        flex: 1
    }, {
        text: '落葬人',
        dataIndex: 'BuryMan',
        renderer: GlobalFun.UpdateRecord,
        flex: 1
    }, {
        text: '备注',
        dataIndex: 'Remark2',
        renderer: GlobalFun.UpdateRecord,
        flex: 2
    }],
    dockedItems: [{
        xtype: 'toolbar',
        itemId: 'toolbarID',
        dock: 'top',
        layout: {
            overflowHandler: 'Menu'
        },
        items: [ActionBase.getAction('refreshBuryMan'),
            ActionBase.getAction('addBuryMan')]//, ActionBase.getAction('editBuryMan')]
    }, {
        xtype: 'Pagingtoolbar',
        itemId: 'pagingtoolbarID',
        store: 'BuryManGridStoreId',
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


//根据传入参数创建客户表，返回自身
GridManager.CreateBuryManGrid = function (param) {
    var tmpArr = [{
        text: '落葬时间',
        dataIndex: 'BuryDate',
        renderer: GlobalFun.UpdateRecordForShortDate,
        flex:1
    }, {
        text: '落葬人',
        dataIndex: 'BuryMan',
        renderer: GlobalFun.UpdateRecord,
        flex: 1
    }, {
        text: '备注',
        dataIndex: 'Remark2',
        renderer: GlobalFun.UpdateRecord,
        flex: 2
    }];
    GridManager.BuryManGrid = Ext.create('chl.gird.BuryManGrid',
        GridManager.BaseGridCfg('BuryManGrid', 'BuryManGridState', tmpArr));
    if (param && param.needLoad) {
        GridManager.BuryManGrid.loadGrid();
    }
    return GridManager.BuryManGrid;
};


//加载SelectionChange事件
GridManager.SetBuryManGridSelectionChangeEvent = function (param) {
    GridManager.BuryManGrid.on('selectionchange', function (view, seles, op) {
        if (!seles[0])
            return;

    });
};


//逝者添加编辑窗口
Ext.define('chl.Grid.AddUpdateBuryManWin', {
    extend: 'Ext.window.Window',
    title: "添加逝者",
    defaultFocus: 'BuryManItemId',
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
        defaultType: 'textfield',
        layout: {
            xtype: 'table',
            columns: 2
        },
        defaults: {
            labelAlign: 'right',
            labelPad: 15,
            width: 340,
            labelWidth: 125,
            maxLength: 100,
            maxLengthText: '最大长度为100'
        },
        items: [{
            name: 'BuryMan',
            fieldLabel: '逝者',
            itemId: 'BuryManItemId',
            validateOnBlur: false,
            allowBlank: false,
            blankText: '逝者不能为空'
        }, {
            fieldLabel: '落葬时间',
            colspan: 2,
            xtype: 'datetimefield',
            width: 340,
            name: 'BuryDate',
            itemId: 'BuryDateItemId',
            mySqlType: 'date',
            value: new Date()
        }, {
            xtype: 'textareafield',
            fieldLabel: '备注',
            name: 'Remark2',
            itemId: 'Remark2ItemId'
        }, {
            fieldLabel: '申请人',
            xtype: 'hidden',
            name: 'Applicanter',
            itemId: 'ApplicanterItemId',
            allowBlank: false,
            blankText: '申请人不能为空'
        }, {
            fieldLabel: '电话',
             xtype: 'hidden',
            name: 'Telephone',
            itemId: 'TelephoneItemId',
            allowBlank: false,
            blankText: '电话不能为空'
        }, {
            fieldLabel: '身份证号',
            xtype: 'hidden',
            name: 'IDNumber',
            itemId: 'IDNumberItemId'
        }, {
            fieldLabel: '管理年限',
            xtype: 'hidden',
            //xtype: 'numberfield',
            name: 'ManageLimit',
            itemId: 'ManageLimitItemId',
            allowBlank: false,
            blankText: '管理年限不能为空'
        }, {
            xtype: 'hidden',
            //xtype: 'checkbox',
            //boxLabel: '无限期管理年限',
            name: 'SupperManage',
            itemId: 'SupperManageItemId'
        }, {
            xtype: 'hidden',
            //xtype: 'checkbox',
            //boxLabel: '申请人备注',
            name: 'Remark',
            itemId: 'RemarkItemId'
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

                    var url = w.action == "create" ? GlobalConfig.Controllers.BuryManGrid.addBuryMan : GlobalConfig.Controllers.BuryManGrid.updateBuryMan;
                    form.submit({
                        url: url,
                        params: {
                            req: 'dataset',
                            dataname: 'AddUpdateBuryMan', // dataset名称，根据实际情况设置,数据库名
                            restype: 'json',
                            Id: w.record ? w.record.data.ControllTid : 0,
                            logId: w.record ? w.record.data.Id : 0,
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
    }]
});
