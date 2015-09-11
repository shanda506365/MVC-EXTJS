Ext.define('chl.toolbar.Pagingtoolbar', {
    alias: 'widget.Pagingtoolbar',
    extend: 'Ext.toolbar.Paging',
    dock: 'bottom',
    displayInfo: true,
    //displayMsg : '第 {0} 到 {1} 条,共 {2}条',
    emptyMsg: '没有记录',
    beforePageText: '页数',
    afterPageText: '共' + ' {0}',
    refreshText: '刷新当前页',
    firstText: '首页',
    lastText: '末页',
    nextText: '下页',
    prevText: '前页',
    layout: {
        overflowHandler: 'Menu'
    },
    initComponent: function () {
        var me = this;
        var filter = '';
        me.callParent(arguments);	// 调用父类方法
        me.on('render', function () {
            me.down('#refresh').hide();
        });
    }
});