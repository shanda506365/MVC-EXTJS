Ext.define('chl.Action.BuryManGridAction', {
    extend: 'WS.action.Base',
    category: 'BuryManGridAction'
});


Ext.create('chl.Action.BuryManGridAction', {
    itemId: 'addBuryMan',
    iconCls: 'add',
    tooltip: '添加',
    text: '添加',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        //var record = target.getSelectionModel().getSelection()[0];
        ActionManager.addBuryMan(target);
    },
    updateStatus: function (selection) {
        this.setDisabled(!GlobalFun.IsAllowFun('墓碑落葬'));
    }
});

Ext.create('chl.Action.BuryManGridAction', {
    itemId: 'editBuryMan',
    iconCls: 'edit',
    tooltip: '编辑',
    text: '编辑',
    handler: function () {
        var me = this;
        var target = me.getTargetView();
        var record = target.getSelectionModel().getSelection()[0];
        ActionManager.editBuryMan(target, record);
    },
    updateStatus: function (selection) {
        this.setDisabled(selection.length != 1 || !GlobalFun.IsAllowFun('墓碑落葬'));
    }
});


Ext.create('chl.Action.BuryManGridAction', {
    itemId: 'refreshBuryMan',
    iconCls: 'refresh',
    tooltip: '刷新',
    text: '刷新',
    handler: function () {
        var target = this.getTargetView();
        ActionManager.refreshBuryMan(target);
    },
    updateStatus: function (selection) {
    }
});

//刷新逝者
ActionManager.refreshBuryMan = function (traget) {
    traget.loadGrid();
};
//新增逝者
ActionManager.addBuryMan = function (traget) {
    var record = traget.getStore().getAt(0);
    WindowManager.AddUpdateBuryManWin = Ext.create('chl.Grid.AddUpdateBuryManWin', {
        grid: traget,
        iconCls: 'add',
        action: 'create',
        record: record,
        title: "新增逝者"
    });
    WindowManager.AddUpdateBuryManWin.show(null, function () {
        WindowManager.AddUpdateBuryManWin.down("#SupperManageItemId").setDisabled(GlobalFun.IsAllowFun('无限期管理年限') ? false : true);
       
        WindowManager.AddUpdateBuryManWin.down("#formId").loadRecord(record);
    });
};
//编辑逝者
ActionManager.editBuryMan = function (traget, record) {
    WindowManager.AddUpdateBuryManWin = Ext.create('chl.Grid.AddUpdateBuryManWin', {
        grid: traget,
        iconCls: 'edit',
        record: record,
        action: 'update',
        title: "编辑逝者"
    });
    WindowManager.AddUpdateBuryManWin.show(null, function () {
        WindowManager.AddUpdateBuryManWin.down("#SupperManageItemId").setDisabled(GlobalFun.IsAllowFun('无限期管理年限') ? false : true);
        WindowManager.AddUpdateBuryManWin.down("#formId").loadRecord(record);
      
    });
};