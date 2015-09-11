
//创建一个上下文菜单
var CustomerGrid_RightMenu = Ext.create('Ext.menu.Menu', {
    items: [ActionBase.getAction('refreshCustomer'), '-', ActionBase.getAction('addCustomer'),
           ActionBase.getAction('editCustomer'),ActionBase.getAction('delCustomer'),
	'-', {
	    //itemId: 'singID',
	    text: '按客户类别过滤',
	    tooltip: '按客户类别过滤',
	    menu: [ActionBase.getAction('allCustomerType'), ActionBase.getAction('GMRCustomerType')
                , ActionBase.getAction('MZZCustomerType')]
	}
    ]
});

Ext.define('chl.gird.CustomerGrid', {
    alternateClassName: ['CustomerGrid'],
    alias: 'widget.CustomerGrid',
    extend: 'chl.grid.BaseGrid',
    store: 'CustomerGridStoreId',
    actionBaseName: 'CustomerGridAction',
    listeners: {
        itemclick: function (grid, record, hitem, index, e, opts) {
            var me = this;
        },
        itemdblclick: function (grid, record, hitem, index, e, opts) {
            ActionBase.getAction('editCustomer').execute();
        },
        itemcontextmenu: function (view, rec, item, index, e, opts) {
            e.stopEvent();
            CustomerGrid_RightMenu.showAt(e.getXY());
        },
        beforeitemmousedown: function (view, record, item, index, e, options) {
            var me = this;
        },
        selectionchange: function (view, seles, op) {
            if (!seles[0])
                return;
            ActionBase.updateActions(GridManager.CustomerGrid.actionBaseName, seles);
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
        items: [ActionBase.getAction('refreshCustomer'), ActionBase.getAction('addCustomer'),
           ActionBase.getAction('editCustomer'), ActionBase.getAction('delCustomer'),
           '-', ActionBase.getAction('importCustomer'), ActionBase.getAction('exportCustomer'),
        '->',{
        fieldLabel: '按名称查找',
        text: '按名称查找',//用于控制工具栏使用
        width: 300,
        labelAlign:'right',
        labelWidth: 80,
        xtype: 'searchfield',
        //store: searchStore,
        itemId: 'customerGridSearchfieldId',
        listeners: {
            render: function() {
                var me = this;
                me.store = GridManager.CustomerGrid.getStore();
            }
        }
    }]
    }, {
        xtype: 'Pagingtoolbar',
        itemId: 'pagingtoolbarID',
        store: 'CustomerGridStoreId',
        dock: 'bottom',
        items: [{
            xtype: 'tbtext',
            text: '过滤:'
        }, {
            xtype: 'GridFilterMenuButton',
            itemId: 'menuID',
            text: '全部类别',
            filterParam: {
                group: 'customerTypeGroup',
                text: '全部类别',
                filterKey: 'CustomerTypeId',
                GridTypeName: 'CustomerGrid',
                store: StoreManager.ComboStore.CustomerTypeStore
            }
        }, '-', {
            xtype: 'GridSelectCancelMenuButton',
            itemId: 'selectRecId',
            text: '选择',
            targetName:'CustomerGrid'
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
    loadGrid: function () {
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
        
        if (store.filterMap.getCount() > 0) {
            me.up('#centerGridDisplayContainer').setTitle('查找结果 { 客户列表 }');
        } else {
            me.up('#centerGridDisplayContainer').setTitle('客户列表');
        }

        ActionBase.updateActions(me.actionBaseName, me.getSelectionModel().getSelection());
    }
});

//根据传入参数创建客户表，返回自身
GridManager.CreateCustomerGrid = function (param) {
    ModelInfoManager.GridModelInfo.CustomerGridModelInfo.LoadDefault();
    ModelManager.GridModel.CretateCusotmerGridModel();
    StoreManager.GridStore.CreateCustomerGridStore();

    var tmpArr = [];
    ModelInfoManager.GridModelInfo.CustomerGridModelInfo.CustomerGridColMap.each(function (item, index, alls) {
        tmpArr.push(item);
    });
    GridManager.CustomerGrid = Ext.create('chl.gird.CustomerGrid',
        GridManager.BaseGridCfg('CustomerGrid', 'CustomerGridState', tmpArr));
    if (param && param.needLoad) {
        GridManager.CustomerGrid.loadGrid();
    }
    return GridManager.CustomerGrid;
};

//加载SelectionChange事件
GridManager.SetCustomerGridSelectionChangeEvent = function (param) {
    GridManager.CustomerGrid.on('selectionchange', function (view, seles, op) {
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
                    width: 340,
                    labelWidth: 125
                }
            },
            items: [{//基础信息fieldset
                title: '基础信息',
                layout: {
                    type: 'table',
                    columns: 3
                },
                items: [{
                    name: 'LastName',
                    validateOnBlur: false,
                    fieldLabel: '姓氏',
                    itemId: 'LastNameId'
                }, {
                    name: 'FirstName',
                    fieldLabel: '名称',
                    itemId: 'FirstNameId'
                }, {
                    name: 'MiddleName',
                    fieldLabel: '中间名',
                    itemId: 'MiddleNameId'
                }, {
                    name: 'NationalityName',
                    fieldLabel: '国籍',
                    itemId: 'NationalityNameId'
                }, {
                    name: 'IDNumber',
                    colspan: 2,
                    fieldLabel: '身份证号',
                    itemId: 'IDNumberId'
                }]
            }]
        }]);

        southTab1.down('#formId').getForm().loadRecord(seles[0]);
    });
};



//客户添加编辑窗口
Ext.define('chl.Grid.AddUpdateCustomerWin', {
    extend: 'Ext.window.Window',
    title: "添加客户",
    defaultFocus: 'LastNameId',
    iconCls: '',
    record:false ,
    //border: false,
    height: 500,
    width: 500,
    layout: 'vbox',
    modal: true,
    resizable: false,
    items: [{
        xtype: 'form',
        itemId: 'formId',
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
        items: [{//基础信息fieldset
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
                maxLength: 50,
                maxLengthText: '最大长度为50'
            },
            items: [{
                name: 'LastName',
                fieldLabel: '姓氏',
                itemId: 'LastNameId',
                allowBlank: false,
                blankText: '姓氏不能为空'
            }, {
                name: 'FirstName',
                width: 140,
                fieldLabel: '名称',
                itemId: 'FirstNameId',
                allowBlank: false,
                blankText: '名称不能为空'
            }, {
                name: 'MiddleName',
                fieldLabel: '中间名',
                itemId: 'MiddleNameId'
            }, {
                fieldLabel: '国籍',
                xtype: 'combobox',
                editable: false,
                name: 'NationalityId',
                itemId: 'NationalityItemId',
                store: 'NationalityStoreId',
                queryMode: 'local',
                displayField: 'Name',
                valueField: 'Id',
                listeners: {
                    boxready: function (com) {
                        var w = com.up('window');
                        var record = w.record;
                        if (record) {
                            GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.NationalityId);
                        } else {
                            GlobalFun.comboSelectFirstOrDefaultVal(com);
                        }
                    }
                }
            }, {
                name: 'IDNumber',
                colspan: 2,
                fieldLabel: '身份证号',
                itemId: 'IDNumberId'
            }]
        }, {//联系方式fieldset
            title: '联系方式',
            layout: {
                type: 'table',
                columns: 2
            },
            defaults: {
                labelAlign: 'right',
                labelPad: 15,
                width: 200,
                labelWidth: 80
            },
            items: [{
                name: 'Telephone',
                fieldLabel: '移动电话',
                itemId: 'TelephoneId',
                allowBlank: false,
                blankText: '移动电话不能为空',
                regex: GlobalConfig.RegexController.regexTelePhoneNumber,
                regexText: '请输入正确的移动电话',
                maxLength: 11,
                maxLengthText: '最大长度为11'
            }, {
                name: 'Phone',
                fieldLabel: '电话',
                itemId: 'PhoneId',
                regex: GlobalConfig.RegexController.regexFaxNumber,
                regexText: '请输入正确的电话号码',
                maxLength: 50,
                maxLengthText: '最大长度为50'
            }, {
                name: 'OtherPhone',
                colspan: 2,
                fieldLabel: '其他联系电话',
                itemId: 'OtherPhoneId',
                regex: GlobalConfig.RegexController.regexFaxNumber,
                regexText: '请输入正确的联系电话',
                maxLength: 50,
                maxLengthText: '最大长度为50'
            }, {
                name: 'Address',
                colspan: 2,
                fieldLabel: '地址',
                itemId: 'AddressId',
                allowBlank: false,
                blankText: '地址不能为空',
                maxLength: 500,
                maxLengthText: '最大长度为500'
            }, {
                xtype: 'container',
                colspan: 2,
                width:450,
                layout: 'hbox',
                defaults: {
                    labelAlign: 'right',
                    labelPad: 10,
                    width:280,
                    margin:'0 0 5 5',
                    labelWidth: 80
                },
                items: [{
                    fieldLabel: '联系人',
                    xtype: 'textfield',
                    hidId:0,
                    readOnly: true,
                    submitValue:false,
                    name: 'LinkCustomerName',
                    itemId: 'LinkCustomerNameItemId'
                }, {
                    xtype: 'button',
                    width: 80,
                    iconCls: 'addUser',
                    text: '选择联系人',
                    handler: function () {
                        var w = this.up('window');
                        var field = w.down('#LinkCustomerNameItemId');
                        WindowManager.SelectCustomerWin = WindowManager.ShowSelectCustomerWin();
                        WindowManager.SelectCustomerWin.callComponent = field;
                    }
                }, {
                    xtype: 'hidden',
                    name: 'LinkCustomerId',
                    itemId: 'LinkCustomerIdItemId'
                }]
            }]
        }, {//其他fieldset
            title: '其他',
            layout: {
                type: 'table',
                columns: 2
            },
            defaults: {
                labelAlign: 'right',
                labelPad: 15,
                width: 200,
                labelWidth: 80
            },
            items: [{
                fieldLabel: '客户类型',
                xtype: 'combobox',
                editable: false,
                name: 'CustomerTypeId',
                itemId: 'CustomerTypeItemId',
                store: 'CustomerTypeStoreId',
                queryMode: 'local',
                displayField: 'Name',
                valueField: 'Id',
                listeners: {
                    boxready: function (com) {
                        var w = com.up('window');
                        var record = w.record;
                        if (record) {
                            GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.CustomerTypeId);
                        } else {
                            GlobalFun.comboSelectFirstOrDefaultVal(com);
                        }
                    },
                    change: function (cb, newVal, oldVal, opts) {
                        var w = cb.up('window');
                        if (newVal == 1) {
                            w.down('#BuryDateId').setDisabled(true);
                            w.down('#DeathDateID').setDisabled(true);
                        } else {
                            w.down('#BuryDateId').setDisabled(false);
                            w.down('#DeathDateID').setDisabled(false);
                        }
                    }
                }
            }, {
                fieldLabel: '客户状态',
                xtype: 'combobox',
                editable: false,
                name: 'CustomerStatusId',
                itemId: 'CustomerStatusItemId',
                store: 'CustomerStatusStoreId',
                queryMode: 'local',
                displayField: 'Name',
                valueField: 'Id',
                listeners: {
                    boxready: function (com) {
                        var w = com.up('window');
                        var record = w.record;
                        if (record) {
                            GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.CustomerStatusId);
                        } else {
                            GlobalFun.comboSelectFirstOrDefaultVal(com);
                        }
                    }
                }
            }, {
                fieldLabel: '下葬日期',
                colspan: 2,
                xtype: 'datetimefield',
                name: 'BuryDate',
                itemId: 'BuryDateId',
                width: 340,
                mySqlType: 'datetime',
                timeHCfg: {
                    value: new Date().getHours()
                },
                timeMCfg: {
                    value: new Date().getMinutes()
                },
                timeICfg: {
                    value: new Date().getSeconds()
                },
                value: new Date()
            }, {
                fieldLabel: '死亡日期',
                colspan: 2,
                xtype: 'datetimefield',
                width: 340,
                name: 'DeathDate',
                itemId: 'DeathDateID',
                mySqlType: 'datetime',
                value: new Date()
            }]

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
            //赋值给隐藏
            var LinkCustomerIdItemId = w.down('#LinkCustomerIdItemId');
            LinkCustomerIdItemId.setValue(w.down('#LinkCustomerNameItemId').hidId);
            var form = w.down('#formId').getForm();

            if (form.isValid()) {
                var url = w.action == "create" ? GlobalConfig.Controllers.CustomerGrid.addCustomer : GlobalConfig.Controllers.CustomerGrid.updateCustomer;
                form.submit({
                    url: url,
                    params: {
                        req: 'dataset',
                        dataname: 'SelectLinkCustomer', // dataset名称，根据实际情况设置,数据库名
                        restype: 'json',
                        Id: w.record?w.record.data.Id:0,
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
                        w.close();
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