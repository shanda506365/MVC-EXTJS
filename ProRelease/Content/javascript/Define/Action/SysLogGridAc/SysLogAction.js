Ext.define('chl.Action.SysLogGridAction', {
    extend: 'WS.action.Base',
    category: 'SysLogGridAction'
});


Ext.create('chl.Action.SysLogGridAction', {
    itemId: 'refreshSysLog',
    iconCls: 'refresh',
    tooltip: '刷新',
    text: '刷新',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.refreshSysLog(target);
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.SysLogGridAction', {
    itemId: 'searchSysLog',
    iconCls: 'search',
    tooltip: '查询',
    text: '查询',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.searchSysLog(target);
    },
    updateStatus: function (selection) {
    }
});

//刷新日志
ActionManager.refreshSysLog = function (traget) {
    traget.loadGrid();
};

//日志查询
ActionManager.searchSysLog = function (traget) {
    if (WindowManager.AddUpdateSysLogWin && WindowManager.AddUpdateSysLogWin != '') {
        WindowManager.AddUpdateSysLogWin.show();
    } else {
        WindowManager.AddUpdateSysLogWin = Ext.create('Ext.window.Window', {
            modal: true,
            resizable: false,
            closeAction: 'hide',
            title: "日志查询",
            defaultFocus: 'NameId',
            iconCls: '',
            record: false,
            height: 500,
            width: 500,
            layout: 'vbox',
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
                    title: '信息',
                    items: [{
                        name: 'Name',
                        width: 300,
                        colspan: 2,
                        fieldLabel: '名称',
                        itemId: 'NameItemId',
                        validateOnBlur: false,
                        allowBlank: false,
                        blankText: '名称不能为空'
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
                }
            }, {
                text: '确定',
                itemId: 'submit',
                handler: function () {
                    var me = this;
                    var win = me.up('window');

                    var name = win.down('#NameItemId').getValue();
                    var searchFlag = false;
                    var store = traget.getStore();
                    if (name != '') {
                        //加入filterMap
                        if (!store.filterMap.containsKey('Name')) {
                            store.filterMap.add('Name', name);
                        } else {
                            store.filterMap.replace('Name', name);
                        }
                        searchFlag = true;
                    } else {
                        store.filterMap.removeAtKey('Name');
                    }

                    if (searchFlag) {
                        win.close();
                        traget.loadGrid(true);
                    } else {
                        win.close();
                        traget.loadGrid();
                    }

                }
            }, {
                text: '取消',
                handler: function () {
                    var me = this;
                    me.up('window').close();
                }
            }]
        }).show();
    }

};