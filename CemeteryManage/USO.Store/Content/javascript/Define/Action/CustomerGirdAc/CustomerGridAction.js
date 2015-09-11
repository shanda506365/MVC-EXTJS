Ext.define('chl.Action.CustomerGridAction', {
    extend: 'WS.action.Base',
    category: 'CustomerGridAction'
});

Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'addCustomer',
    iconCls: 'add',
    tooltip: '添加',
    text: '添加',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        ActionManager.addCustomer(target);
    },
    updateStatus: function (selection) {
    }
});

Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'editCustomer',
    iconCls: 'edit',
    tooltip: '编辑',
    text: '编辑',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        var record = target.getSelectionModel().getSelection()[0];
        ActionManager.editCustomer(target, record);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1);
    }
});


Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'delCustomer',
    iconCls: 'delete',
    tooltip: '删除',
    text: '删除',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.delCustomer(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length == 0);
    }
});

Ext.create('chl.Action.CustomerGridAction', {
    itemId: 'refreshCustomer',
    iconCls: 'refresh',
    tooltip: '刷新',
    text: '刷新',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.refreshCustomer(target);
    },
    updateStatus: function (selection) {
    }
});


Ext.create('chl.Action.CustomerGridAction', {
    text: '全部类别',
    checked: true,
    group: 'customerTypeGroup',
    itemId: 'allCustomerType',
    handler: function (btn, eve, suppressLoad) {
        var panel = this.getTargetView();
        var menu = panel.down('#menuID');
        menu.setText('全部类别');
        menu.setTooltip('全部类别');
      
        panel.getStore().filterMap.removeAtKey('CustomerTypeId');
        if (suppressLoad) {
            this.setChecked(suppressLoad);
            return;
        }
        
        panel.loadGrid();
    },
    updateStatus: function (selection) {

    }
});
Ext.create('chl.Action.CustomerGridAction', {
    text: '购买人',
    group: 'customerTypeGroup',
    itemId: 'GMRCustomerType',
    handler: function (btn, eve, suppressLoad) {
        var panel = this.getTargetView();
        var menu = panel.down('#menuID');
        menu.setText('购买人');
        menu.setTooltip('购买人');

        if (panel.getStore().filterMap.containsKey('CustomerTypeId')) {
            panel.getStore().filterMap.add('CustomerTypeId', 1);
        } else {
            panel.getStore().filterMap.replace('CustomerTypeId', 1);
        }
        
        if (suppressLoad) {
            this.setChecked(suppressLoad);
            return;
        }
        panel.loadGrid();
    },
    updateStatus: function (selection) {

    }
});

Ext.create('chl.Action.CustomerGridAction', {
    text: '埋葬者',
    group: 'customerTypeGroup',
    itemId: 'MZZCustomerType',
    handler: function (btn, eve, suppressLoad) {
        var panel = this.getTargetView();
        var menu = panel.down('#menuID');
        menu.setText('购买人');
        menu.setTooltip('购买人');

        if (panel.getStore().filterMap.containsKey('CustomerTypeId')) {
            panel.getStore().filterMap.add('CustomerTypeId', 2);
        } else {
            panel.getStore().filterMap.replace('CustomerTypeId', 2);
        }

        if (suppressLoad) {
            this.setChecked(suppressLoad);
            return;
        }
        panel.loadGrid();
    },
    updateStatus: function (selection) {

    }
});


ActionManager.addCustomer = function (traget) {
    WindowManager.AddUpdateCustomerWin = Ext.create('chl.Grid.AddUpdateCustomerWin', {
        grid: traget,
        iconCls:'add',
        action: 'create',
        record:false,
        title: "新建客户"
    });
    WindowManager.AddUpdateCustomerWin.show();
};
ActionManager.editCustomer = function (traget,record) {
    WindowManager.AddUpdateCustomerWin = Ext.create('chl.Grid.AddUpdateCustomerWin', {
        grid: traget,
        iconCls: 'edit',
        record:record,
        action: 'update',
        title: "编辑客户"
    });
    WindowManager.AddUpdateCustomerWin.show(null, function () {
        WindowManager.AddUpdateCustomerWin.down("#formId").loadRecord(record);
        //为日期赋值
        WindowManager.AddUpdateCustomerWin.down("#BuryDateId").setValue(record.data.BuryDate.replace(" ", "T"),true);
        WindowManager.AddUpdateCustomerWin.down("#DeathDateID").setValue(record.data.DeathDate.replace(" ", "T"), true);
    });
};
ActionManager.delCustomer = function (traget) {
    var sm = traget.getSelectionModel();
    var records = sm.getSelection();
    if (!records[0])
        return;
    var ids = [];
    Ext.Array.each(records, function (rec) {
        ids.push(rec.data.Id);
    });
    var store = traget.getStore();
    store.getProxy().extraParams.idList = ids.join();
    GlobalConfig.newMessageBox.show({
        title: '提示',
        msg: '您确定要删除选定的项目吗？',
        buttons: Ext.MessageBox.YESNO,
        closable: false,
        fn: function (btn) {
            if (btn == 'yes') {
                store.remove(records);
                store.sync();
                store.getProxy().extraParams.idList = '';
                for (var i = 0; i < records.length; i++) {
                    records[i].commit(true);			//将之前选中的数据设置脏数据为false
                }
                (new Ext.util.DelayedTask(function () {
                    store.load();
                })).delay(500);
            }
        },
        icon: Ext.MessageBox.QUESTION
    });
};
ActionManager.refreshCustomer = function (traget) {
    traget.loadGrid();
};