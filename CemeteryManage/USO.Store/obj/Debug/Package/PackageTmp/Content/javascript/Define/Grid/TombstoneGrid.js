﻿
//创建一个上下文菜单
var TombstoneGrid_RightMenu = Ext.create('Ext.menu.Menu', {
    items: [ActionBase.getAction('searchTombstone'), '-',
            ActionBase.getAction('refreshTombstone'), '-',
            ActionBase.getAction('addTombstone'), ActionBase.getAction('editTombstone'),
            ActionBase.getAction('delTombstone'), '-',
         ActionBase.getAction('sortTombstone'), ]
});

Ext.define('chl.gird.TombstoneGrid', {
    alternateClassName: ['TombstoneGrid'],
    alias: 'widget.TombstoneGrid',
    extend: 'chl.grid.BaseGrid',
    store: 'TombstoneGridStoreId',
    actionBaseName: 'TombstoneGridAction',
    listeners: {
        itemclick: function (grid, record, hitem, index, e, opts) {
            var me = this;
        },
        itemdblclick: function (grid, record, hitem, index, e, opts) {
            ActionBase.getAction('editTombstone').execute();
        },
        itemcontextmenu: function (view, rec, item, index, e, opts) {
            e.stopEvent();
            TombstoneGrid_RightMenu.showAt(e.getXY());
        },
        beforeitemmousedown: function (view, record, item, index, e, options) {
            var me = this;
        },
        selectionchange: function (view, seles, op) {
            if (!seles[0])
                return;
            ActionBase.updateActions(GridManager.TombstoneGrid.actionBaseName, seles);
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
        items: [ActionBase.getAction('searchTombstone'), '-',
            ActionBase.getAction('refreshTombstone'), '-',
            ActionBase.getAction('addTombstone'), ActionBase.getAction('editTombstone'),
            ActionBase.getAction('delTombstone'),  '-',
         ActionBase.getAction('sortTombstone'),
        '->', {
            fieldLabel: '按编号查找',
            text: '按编号查找',//用于控制工具栏使用
            width: 300,
            labelAlign: 'right',
            labelWidth: 80,
            xtype: 'searchfield',
            //paramName: 'Column',
            paramObject: true,
            minLength: 6,
            minLengthText: '请输入6位编码',
            maxLength: 6,
            maxLengthText: '请输入6位编码',
            paramNameArr: ['Area', 'Row', 'Column'],
            //store: searchStore,
            itemId: 'tombstoneGridSearchfieldId',
            listeners: {
                render: function() {
                    var me = this;
                    me.store = GridManager.TombstoneGrid.getStore();
                }
            }
        }]
    }, {
        xtype: 'Pagingtoolbar',
        itemId: 'pagingtoolbarID',
        store: 'TombstoneGridStoreId',
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
                GridTypeName: 'TombstoneGrid',
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
                GridTypeName: 'TombstoneGrid',
                store: StoreManager.ComboStore.PaymentStatusStore
            }
        }, '-', {
            xtype: 'GridSelectCancelMenuButton',
            itemId: 'selectRecId',
            text: '选择',
            targetName: 'TombstoneGrid'
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
        if (store.filterMap.getCount() >0) {
            me.up('#centerGridDisplayContainer').setTitle('查找结果 { 墓碑列表 }');
        } else {
            me.up('#centerGridDisplayContainer').setTitle('墓碑列表');
        }

        ActionBase.updateActions(me.actionBaseName, me.getSelectionModel().getSelection());
    }
});

//根据传入参数创建客户表，返回自身
GridManager.CreateTombstoneGrid = function (param) {
    ModelInfoManager.GridModelInfo.TombstoneModelInfo.LoadDefault();
    ModelManager.GridModel.CretateTombstoneGridModel();
    StoreManager.GridStore.CreateTombstoneGridStore();

    var tmpArr = [];
    ModelInfoManager.GridModelInfo.TombstoneModelInfo.TombstoneColMap.each(function (item, index, alls) {
        tmpArr.push(item);
    });
    GridManager.TombstoneGrid = Ext.create('chl.gird.TombstoneGrid',
        GridManager.BaseGridCfg('TombstoneGrid', 'TombstoneGridState', tmpArr));
    if (param && param.needLoad) {
        GridManager.TombstoneGrid.loadGrid();
    }
    return GridManager.TombstoneGrid;
};

//加载SelectionChange事件
GridManager.SetTombstoneGridSelectionChangeEvent = function (param) {
    GridManager.TombstoneGrid.on('selectionchange', function (view, seles, op) {
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
                    name: 'Name',
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
                }, {
                    name: 'SortNum',
                    fieldLabel: '排序字段',
                    itemId: 'SortNumItemId'
                }, {
                    name: 'BuyDate',
                    colspan: 2,
                    fieldLabel: '购买日期',
                    itemId: 'BuyDateItemId'
                }]
            }]
        }]);

        southTab1.down('#formId').getForm().loadRecord(seles[0]);
        var areaEntityItem = southTab1.down('#AreaEntityItemId');
        areaEntityItem.setValue(seles[0].data.AreaEntity.Name);;
    });
};


//墓碑添加编辑窗口
Ext.define('chl.Grid.AddUpdateTombstoneWin', {
    extend: 'Ext.window.Window',
    title: "添加墓碑",
    defaultFocus: 'NameId',
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
                fieldLabel: '别名',
                itemId: 'AliasItemId',
                allowBlank: false,
                blankText: '不能为空'
            }, {
                fieldLabel: '区域',
                width: 200,
                //colspan: 2,
                xtype: 'combobox',
                editable: false,
                name: 'AreaId',
                itemId: 'AreaIdItemId',
                store: 'AreaStoreId',
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
                            GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.AreaId);
                        } else {
                            GlobalFun.comboSelectFirstOrDefaultVal(com);
                        }
                    }
                }
            }, {
                name: 'SortNum',
                width: 200,
                fieldLabel: '排序编号',
                itemId: 'SortNumItemId',
                allowBlank: false,
                blankText: '不能为空',
                maxLength: 10,
                maxLengthText: '最大长度为10',
                regex: GlobalConfig.RegexController.regexNumber,
                regexText: '请输入数字'
            }, {
                fieldLabel: '行',
                xtype: 'combobox',
                editable: false,
                name: 'RowId',
                itemId: 'RowIdItemId',
                store: 'RowStoreId',
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
                            GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.RowId);
                        } else {
                            GlobalFun.comboSelectFirstOrDefaultVal(com);
                        }
                    }
                }
            }, {
                fieldLabel: '列',
                xtype: 'combobox',
                editable: false,
                name: 'ColumnId',
                itemId: 'ColumnIdItemId',
                store: 'ColumnStoreId',
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
                            GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.ColumnId);
                        } else {
                            GlobalFun.comboSelectFirstOrDefaultVal(com);
                        }
                    }
                }
            }, {
                xtype: 'container',
                colspan: 2,
                width: 450,
                layout: 'hbox',
                defaults: {
                    labelAlign: 'right',
                    labelPad: 10,
                    width: 280,
                    margin: '0 0 5 5',
                    labelWidth: 80
                },
                items: [{
                    fieldLabel: '所属客户',
                    xtype: 'textfield',
                    hidId: 0,
                    readOnly: true,
                    submitValue: false,
                    name: 'CustomerName',
                    itemId: 'CustomerNameItemId'
                }, {
                    xtype: 'button',
                    width: 80,
                    iconCls: 'addUser',
                    text: '选择客户',
                    handler: function () {
                        var w = this.up('window');
                        var field = w.down('#CustomerNameItemId');
                        WindowManager.SelectCustomerWin = WindowManager.ShowSelectCustomerWin();
                        WindowManager.SelectCustomerWin.callComponent = field;
                    }
                }, {
                    xtype: 'hidden',
                    name: 'CustomerId',
                    itemId: 'CustomerIdItemId'
                }]
            }, {
                name: 'Width',
                fieldLabel: '宽度(mm)',
                itemId: 'WidthID',
                maxLength: 10,
                maxLengthText: '最大长度为10',
                regex: GlobalConfig.RegexController.regexNumber,
                regexText: '请输入数字'
            }, {
                name: 'Height',
                fieldLabel: '高度(mm)',
                itemId: 'HeightID',
                maxLength: 10,
                maxLengthText: '最大长度为10',
                regex: GlobalConfig.RegexController.regexNumber,
                regexText: '请输入数字'
            }, {
                name: 'Acreage',
                colspan: 2,
                width: 300,
                readOnly: true,
                fieldLabel: '面积(mm²)',
                itemId: 'AcreageID',
                regex: GlobalConfig.RegexController.regexNumber,
                regexText: '请输入数字'
            }, {
                fieldLabel: '墓碑类别',
                width: 300,
                colspan: 2,
                xtype: 'combobox',
                editable: false,
                name: 'TypeId',
                itemId: 'TypeIdItemId',
                store: 'TombstoneTypeStoreId',
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
                            GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.TypeId);
                        } else {
                            GlobalFun.comboSelectFirstOrDefaultVal(com);
                        }
                    }
                }
            }]
        }//基础信息end
         , {
             title: '扩展信息',
             layout: {
                 type: 'table',
                 columns: 2
             },
             defaults: {
                 labelAlign: 'right',
                 labelPad: 15,
                 width: 200,
                 labelWidth: 80,
                 maxLength: 2000,
                 maxLengthText: '最大长度为2000'
             },
             items: [{
                 fieldLabel: '保密等级',
                 width: 300,
                 colspan: 2,
                 xtype: 'combobox',
                 editable: false,
                 name: 'SecurityLevelId',
                 itemId: 'SecurityLevelIdItemId',
                 store: 'SecurityLevelStoreId',
                 queryMode: 'local',
                 displayField: 'Name',
                 valueField: 'Id',
                 listeners: {
                     boxready: function (com) {
                         var w = com.up('window');
                         var record = w.record;
                         if (record) {
                             GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.SecurityLevelId);
                         } else {
                             GlobalFun.comboSelectFirstOrDefaultVal(com);
                         }
                     }
                 }
             }, {
                 fieldLabel: '服务等级',
                 width: 300,
                 colspan: 2,
                 xtype: 'combobox',
                 editable: false,
                 name: 'ServiceLevelId',
                 itemId: 'ServiceLevelIdItemId',
                 store: 'ServiceLevelStoreId',
                 queryMode: 'local',
                 displayField: 'Name',
                 valueField: 'Id',
                 listeners: {
                     boxready: function (com) {
                         var w = com.up('window');
                         var record = w.record;
                         if (record) {
                             GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.ServiceLevelId);
                         } else {
                             GlobalFun.comboSelectFirstOrDefaultVal(com);
                         }
                     }
                 }
             }, {
                 fieldLabel: '付款状态',
                 width: 300,
                 colspan: 2,
                 xtype: 'combobox',
                 editable: false,
                 name: 'PaymentStatusId',
                 itemId: 'PaymentStatusIdItemId',
                 store: 'PaymentStatusStoreId',
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
                             GlobalFun.comboSelectFirstOrDefaultVal(com, record.data.PaymentStatusId);
                         } else {
                             GlobalFun.comboSelectFirstOrDefaultVal(com);
                         }
                     }
                 }
             }, {
                 fieldLabel: '到期日期',
                 colspan: 2,
                 xtype: 'datetimefield',
                 width: 340,
                 name: 'ExpiryDate',
                 itemId: 'ExpiryDateID',
                 mySqlType: 'datetime',
                 value: new Date()
             }, {
                 fieldLabel: '购买日期',
                 colspan: 2,
                 xtype: 'datetimefield',
                 width: 340,
                 name: 'BuyDate',
                 itemId: 'BuyDateID',
                 mySqlType: 'datetime',
                 value: new Date()
             }, {
                 fieldLabel: '上次缴费日期',
                 colspan: 2,
                 xtype: 'datetimefield',
                 width: 340,
                 name: 'LastPaymentDate',
                 itemId: 'LastPaymentDateID',
                 mySqlType: 'datetime',
                 value: new Date()
             }, {
                 fieldLabel: '使用日期',
                 colspan: 2,
                 xtype: 'datetimefield',
                 width: 340,
                 name: 'BuryDate',
                 itemId: 'BuryDateID',
                 mySqlType: 'datetime',
                 value: new Date()
             }, {
                 xtype: 'textareafield',
                 span: 2,
                 name: 'StoneText',
                 width: 400,
                 fieldLabel: '碑文',
                 itemId: 'RemarkId'
             }, {
                 xtype: 'textareafield',
                 span: 2,
                 name: 'Remark',
                 width: 400,
                 fieldLabel: '备注',
                 itemId: 'RemarkId'
             }]
         }//备注
        , {
            title: '所属墓碑信息',
            layout: {
                type: 'table',
                columns: 1
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
                xtype: 'container',
                colspan: 2,
                width: 450,
                layout: 'hbox',
                defaults: {
                    labelAlign: 'right',
                    labelPad: 10,
                    width: 280,
                    margin: '0 0 5 5',
                    labelWidth: 80
                },
                items: [{
                    fieldLabel: '所属墓碑',
                    xtype: 'textfield',
                    hidId: 0,
                    readOnly: true,
                    submitValue: false,
                    name: 'ParentName',
                    itemId: 'ParentNameItemId'
                }, {
                    xtype: 'button',
                    width: 80,
                    iconCls: 'addUser',
                    text: '选择墓碑',
                    handler: function () {
                        var w = this.up('window');
                        var field = w.down('#ParentNameItemId');
                        WindowManager.SelectTombstoneWin = WindowManager.ShowSelectTombstoneWin();
                        WindowManager.SelectTombstoneWin.callComponent = field;
                    }
                }, {
                    xtype: 'hidden',
                    name: 'ParentId',
                    itemId: 'ParentIdItemId'
                }]
            }]
        }//所属墓碑信息
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
            //赋值给隐藏
            var customerIdItemId = w.down('#CustomerIdItemId');
            customerIdItemId.setValue(w.down('#CustomerNameItemId').hidId);
            var form = w.down('#formId').getForm();

            if (form.isValid()) {
                var url = w.action == "create" ? GlobalConfig.Controllers.TombstoneGrid.addTombstone : GlobalConfig.Controllers.TombstoneGrid.updateTombstone;
                form.submit({
                    url: url,
                    params: {
                        req: 'dataset',
                        dataname: 'SelectCustomer', // dataset名称，根据实际情况设置,数据库名
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