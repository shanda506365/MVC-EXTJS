
//创建一个上下文菜单
var UserGrid_RightMenu = Ext.create('Ext.menu.Menu', {
    items: [ActionBase.getAction('searchUser'), '-',
            ActionBase.getAction('refreshUser'), '-',
            ActionBase.getAction('addUser'), ActionBase.getAction('editUser'),
            ActionBase.getAction('delUser')]
});

Ext.define('chl.gird.UserGrid', {
    alternateClassName: ['UserGrid'],
    alias: 'widget.UserGrid',
    extend: 'chl.grid.BaseGrid',
    store: 'UserGridStoreId',
    actionBaseName: 'UserGridAction',
    listeners: {
        itemclick: function (grid, record, hitem, index, e, opts) {
            var me = this;
        },
        itemdblclick: function (grid, record, hitem, index, e, opts) {
            ActionBase.getAction('editUser').execute();
        },
        itemcontextmenu: function (view, rec, item, index, e, opts) {
            e.stopEvent();
            UserGrid_RightMenu.showAt(e.getXY());
        },
        beforeitemmousedown: function (view, record, item, index, e, options) {
            var me = this;
        },
        selectionchange: function (view, seles, op) {
            if (!seles[0])
                return;
            ActionBase.updateActions(GridManager.UserGrid.actionBaseName, seles);
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
        items: [ActionBase.getAction('searchUser'), '-',
            ActionBase.getAction('refreshUser'), '-',
            ActionBase.getAction('addUser'), ActionBase.getAction('editUser'),
            ActionBase.getAction('delUser'),
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
            itemId: 'UserGridSearchfieldId',
            listeners: {
                render: function () {
                    var me = this;
                    me.store = GridManager.UserGrid.getStore();
                }
            }
        }]
    }, {
        xtype: 'Pagingtoolbar',
        itemId: 'pagingtoolbarID',
        store: 'UserGridStoreId',
        dock: 'bottom',
        items: [{
            xtype: 'tbtext',
            text: '过滤:'
        }, {
            xtype: 'GridFilterMenuButton',
            itemId: 'menuAreaID',
            text: '全部部门',
            filterParam: {
                group: 'areaGroup',
                text: '全部部门',
                filterKey: 'DepartmentId',
                GridTypeName: 'UserGrid',
                store: StoreManager.ComboStore.DepartmentStore
            }
        }, {
            xtype: 'GridFilterMenuButton',
            itemId: 'menuAreaID',
            text: '全部状态',
            filterParam: {
                group: 'areaGroup',
                text: '全部状态',
                filterKey: 'Status',
                GridTypeName: 'UserGrid',
                store: StoreManager.ComboStore.UserStatusStore
            }
        }, '-', {
            xtype: 'GridSelectCancelMenuButton',
            itemId: 'selectRecId',
            text: '选择',
            targetName: 'UserGrid'
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
        GlobalFun.SetGridTitle(me.up('#centerGridDisplayContainer'), store, "用户列表");
        ActionBase.updateActions(me.actionBaseName, me.getSelectionModel().getSelection());
    }
});

//根据传入参数创建客户表，返回自身
GridManager.CreateUserGrid = function (param) {
    ModelInfoManager.GridModelInfo.UserModelInfo.LoadDefault();
    ModelManager.GridModel.CretateUserGridModel();
    StoreManager.GridStore.CreateUserGridStore();

    var tmpArr = [];
    ModelInfoManager.GridModelInfo.UserModelInfo.UserColMap.each(function (item, index, alls) {
        tmpArr.push(item);
    });
    GridManager.UserGrid = Ext.create('chl.gird.UserGrid',
        GridManager.BaseGridCfg('UserGrid', 'UserGridState', tmpArr));
    if (param && param.needLoad) {
        GridManager.UserGrid.loadGrid();
    }
    return GridManager.UserGrid;
};

//加载SelectionChange事件
GridManager.SetUserGridSelectionChangeEvent = function (param) {
    GridManager.UserGrid.on('selectionchange', function (view, seles, op) {
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
                    fieldLabel: '名称'
                }, {
                    name: 'DepartmentEntity',
                    itemId: 'DepartmentEntityItemId',
                    fieldLabel: '部门'
                }, {
                    name: 'LoginName',
                    fieldLabel: '登录名'
                }, {
                    name: 'Code',
                    fieldLabel: '编号'
                }, {
                    name: 'Position',
                    fieldLabel: '职务'
                }, {
                    name: 'RoleDtosString',
                    fieldLabel: '角色'
                }, {
                    name: 'StatusString',
                    fieldLabel: '状态'
                }, {
                    name: 'CreateDate',
                    colspan: 2,
                    fieldLabel: '创建日期'
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
        var departmentEntityItem = southTab1.down('#DepartmentEntityItemId');
        departmentEntityItem.setValue(seles[0].data.DepartmentEntity.Name);
        southTab1.doLayout();
    });
};


//用户添加编辑窗口
Ext.define('chl.Grid.AddUpdateUserWin', {
    extend: 'Ext.window.Window',
    title: "添加用户",
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
            }, {
                name: 'Code',
                fieldLabel: '编号',
                itemId: 'CodeItemId'
            }, {
                name: 'LoginName',
                fieldLabel: '登录名',
                itemId: 'LoginNameItemId',
                allowBlank: false,
                blankText: '不能为空'
            }, {
                name: 'Position',
                fieldLabel: '职务',
                itemId: 'PositionItemId'
            }, {
                fieldLabel: '部门',
                xtype: 'combobox',
                editable: false,
                name: 'DepartmentId',
                itemId: 'DepartmentIdItemId',
                store: 'DepartmentStoreId',
                queryMode: 'local',
                displayField: 'Name',
                valueField: 'Id',
                allowBlank: false,
                blankText: '不能为空',
                listeners: {
                    boxready: function (com) {
                        var w = com.up('window');
                        var record = w.record;
                        if (record) {
                            GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.DepartmentId);
                        } else {
                            GlobalFun.comboSelectFirstOrDefaultVal(com);
                        }
                    }
                }
            }, {
                fieldLabel: '状态',
                xtype: 'combobox',
                editable: false,
                name: 'Status',
                itemId: 'StatusItemId',
                store: 'UserStatusStoreId',
                queryMode: 'local',
                displayField: 'Name',
                valueField: 'Id',
                allowBlank: false,
                blankText: '不能为空',
                listeners: {
                    boxready: function (com) {
                        var w = com.up('window');
                        var record = w.record;
                        if (record) {
                            GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.Status);
                        } else {
                            GlobalFun.comboSelectFirstOrDefaultVal(com);
                        }
                    }
                }
            }, {
                fieldLabel: '角色',
                colspan:2,
                xtype: 'combobox',
                editable: false,
                name: 'RoleId',
                itemId: 'RoleIdItemId',
                store: 'RoleStoreId',
                queryMode: 'local',
                displayField: 'Name',
                valueField: 'Id',
                allowBlank: false,
                blankText: '不能为空',
                listeners: {
                    boxready: function (com) {
                        var w = com.up('window');
                        var record = w.record;
                        if (record) {
                            GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.RoleDtos[0].Id);
                        } else {
                            GlobalFun.comboSelectFirstOrDefaultVal(com);
                        }
                    }
                }
            }, {
                xtype: 'textareafield',
                colspan: 2,
                name: 'Remark',
                width: 400,
                fieldLabel: '备注',
                itemId: 'RemarkId',
                maxLength: 800,
                maxLengthText: '最大长度为800'
            }]
        },//基础信息
            {
                xtype: 'checkbox',
                itemId: 'ifEditorPass',
                disabled: false,
                boxLabel: '修改用户帐户密码',
                name: 'modifiPass',
                margin: '0 0 5 10',
                colspan: 3,
                listeners: {
                    change: function (cb, nValue, oValue, opts) {
                        if (cb.getValue()) {
                            Ext.Array.each(cb.up('form').down('#passwordEditor').query('textfield'), function (item, index, allItems) {
                                item.setDisabled(false);
                            });
                        } else {
                            Ext.Array.each(cb.up('form').down('#passwordEditor').query('textfield'), function (item, index, allItems) {
                                item.setDisabled(true);
                            });
                        }
                    }
                }
            }, {
                //title:'修改用户帐户密码',
                xtype: 'fieldset',
                itemId: 'passwordEditor',
                padding: '4 10 4 10',
                defaults: {
                    xtype: Ext.isChrome ? 'MyPasswordField' : 'textfield',
                    disabled: true,
                    width: 300,
                    labelPad: 15,
                    labelWidth: 80,
                    labelAlign: 'right',
                    margin: '5 0 0 0',
                    enableKeyEvents: true,
                    allowBlank:false,
                    maxLength: 20,
                    maxLengthText: '最大长度为20',
                    minLength: 6,
                    minLengthText: '最小长度为6'
                },
                items: [{
                    fieldLabel: '旧密码',
                    submitValue: false,
                    itemId: 'oldPassword',
                    name: 'oldPassword',
                    inputType: Ext.isChrome ? 'text' : 'password'
                }, {
                    fieldLabel: '新密码',
                    submitValue: false,
                    name: 'newPassword',
                    inputType: Ext.isChrome ? 'text' : 'password',
                    itemId: 'newPassword',
                    validateOnBlur: false,
                    validateOnChange: false
                }, {
                    fieldLabel: '确认新密码',
                    submitValue: false,
                    name: 'repeatPassword',
                    inputType: Ext.isChrome ? 'text' : 'password',
                    itemId: 'repeatPassword',
                    validateOnBlur: true,
                    validateOnChange: false,
                    validator: function (value) {
                        var myVal = value;
                        if (Ext.isChrome) {
                            myVal = this.getValue();
                        }
                        var password1 = this.previousSibling('[name=newPassword]');
                        return (myVal === password1.getValue()) ? true : '两次输入的新密码不一致，请重新输入';
                    }
                }]//修改用户帐户密码  items
            }]//个人信息 items

    }
    ],
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
                var newPassword = w.down("#newPassword").getValue();
                var oldPassword = w.down("#oldPassword").getValue();
                var roleIdItemId = w.down('#RoleIdItemId').getValue();
                
                var url = w.action == "create" ? GlobalConfig.Controllers.UserGrid.addUser : GlobalConfig.Controllers.UserGrid.updateUser;
                form.submit({
                    url: url,
                    params: {
                        req: 'dataset',
                        dataname: 'AddUpdateUser', // dataset名称，根据实际情况设置,数据库名
                        restype: 'json',
                        Id: w.record ? w.record.data.Id : 0,
                        newpassHd: encryptedString(RASkey, newPassword),
                        oldpassHd: encryptedString(RASkey, oldPassword),
                        roleIdItemId:roleIdItemId,
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