Ext.define('chl.grid.BaseGrid', {
    alternateClassName: ['BaseGrid'],
    alias: 'widget.BaseGrid',
    extend: 'Ext.grid.Panel',
    columnLines: true,
    multiSelect: true,
    features: [{
        ftype: 'grouping',
        groupHeaderTpl: '{columnName}: {name} ({rows.length})' + '项',
        disabled: true,
        groupByText: '按当前列分组',
        showGroupsText: '显示分组'
    }],
    viewConfig: {
        loadingText: '<b>' + '正在加载数据...' + '</b>',
        plugins: {
            ddGroup: 'FileDDGp',
            ptype: 'gridviewdragdrop',
            dragText: '选中了 {0} 条记录',
            enableDrop: false
        }
    },
    initComponent: function () {
        var me = this;
        //改良选择方式
        me.on({
            itemclick: function (grid, record, hitem, index, e, opts) {
                if (!e.ctrlKey && !e.shiftKey) {
                    var sm = grid.getSelectionModel();
                    sm.deselectAll(true);
                    sm.select(record, true, false);
                }
            },
            beforeitemmousedown: function (view, record, item, index, e, options) {
                if (e.button == 2) {
                    var sm = view.getSelectionModel();
                    if (!sm.hasSelection()) {
                        sm.select(record, true, false);
                    } else {
                        var sels = sm.getSelection();
                        if (!Ext.Array.contains(sels, record)) {
                            sm.deselectAll(true);
                            sm.select(record, true, false);
                        }
                    }
                    return false;
                }
                return true;
            }
        });
        me.callParent(arguments);	// 调用父类方法
    }
});