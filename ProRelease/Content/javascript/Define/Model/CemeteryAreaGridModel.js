
ModelInfoManager.GridModelInfo.CemeteryAreaModelInfo = {
    CemeteryAreaModelMap: new Ext.util.MixedCollection(),//当前model
    CemeteryAreaColMap: new Ext.util.MixedCollection(),//当前columns
    LoadDefault: function () {
        var me = this;
        me.CemeteryAreaModelMap.clear();
        me.CemeteryAreaColMap.clear();
        //初始化fields
        var CemeteryAreaModelArr = [{
            name: 'Id',
            type: 'string'
        }, {
            name: 'Name',
            type: 'string'
        }, {
            name: 'Alias',
            type: 'string'
        }, {
            name: 'Remark',
            type: 'string'
        }, {
            name: 'TotalCount'
        }, {
            name: 'OrderCount'
        }, {
            name: 'SaleCount'
        }, {
            name: 'BuryCount'
        }, {
            name: 'ElseCount'
        }, {
            name: 'RowSort'
        }, {
            name:'RowSortString'
        }];

        Ext.Array.each(CemeteryAreaModelArr, function (item, index, alls) {
            me.CemeteryAreaModelMap.add(item.name, item);
        });
        //初始化columns
        var CemeteryAreaColArr = [{
            id: 'Id',
            text: '编号',
            dataIndex: 'Id',
            width: 60//,
            //renderer: function (value, metaData, record) {
            //    if (record.data.attach.length > 0) {
            //        return "<span><img src='resources/images/pub/attach.png' style='margin-bottom: -4px;'>&nbsp;" + updateRecord(value, metaData, record) + '</span>';
            //    }
            //    return "<span><img src='resources/images/fax/inFax.png' style='margin-bottom: -4px;'>&nbsp;" + updateRecord(value, metaData, record) + '</span>';
            //}
        }, {
            id: 'Name',
            text: '名称',
            dataIndex: 'Name',
            renderer: GlobalFun.UpdateRecord,
            width: 100
        }, {
            id: 'Alias',
            text: '编号',
            dataIndex: 'Alias',
            renderer: GlobalFun.UpdateRecord,
            width: 100
        }, {
            id: 'TotalCount',
            text: '总墓碑数',
            sortable: false,
            groupable:false,
            dataIndex: 'TotalCount',
            renderer: GlobalFun.UpdateRecord,
            width: 80
        }, {
            id: 'OrderCount',
            text: '已订',
            sortable: false,
            groupable: false,
            dataIndex: 'OrderCount',
            renderer: GlobalFun.UpdateRecord,
            width: 80
        }, {
            id: 'SaleCount',
            text: '已售',
            sortable: false,
            groupable: false,
            dataIndex: 'SaleCount',
            renderer: GlobalFun.UpdateRecord,
            width: 80
        }, {
            id: 'BuryCount',
            text: '已落葬',
            sortable: false,
            groupable: false,
            dataIndex: 'BuryCount',
            renderer: GlobalFun.UpdateRecord,
            width: 80
        }, {
            id: 'ElseCount',
            text: '空置',
            sortable: false,
            groupable: false,
            dataIndex: 'ElseCount',
            renderer: GlobalFun.UpdateRecord,
            width: 80
        }, {
            id: 'RowSortString',
            text: '排序',
            sortable: false,
            groupable: false,
            dataIndex: 'RowSortString',
            renderer: GlobalFun.UpdateRecord,
            width: 80
        }, {
            id: 'Remark',
            text: '备注',
            sortable: false,
            groupable: false,
            dataIndex: 'Remark',
            renderer: GlobalFun.UpdateRecord,
            width: 400
        }];
        //可比state对排序,控制可见
        Ext.Array.each(CemeteryAreaColArr, function (item, index, alls) {
            item.id = '23-' + item.id;
            me.CemeteryAreaColMap.add(item.dataIndex, item);
        });
        //不用排序，取下width,hidden
        //GlobalFun.resumeGridColumns(myStates.infaxgridState.columns, me.infaxColMap);

    }
};

//创建model
ModelManager.GridModel.CretateCemeteryAreaGridModel = function () {
    var tmpArr = [];
    ModelInfoManager.GridModelInfo
        .CemeteryAreaModelInfo
        .CemeteryAreaModelMap.each(function (item, index, alls) {
            tmpArr.push(item);
        });

    Ext.define('chl.Model.CemeteryAreaGridModel', {
        extend: 'Ext.data.Model',
        idProperty: 'Id',
        alternateClassName: 'CemeteryAreaGridModel',
        fields: tmpArr
    });

}