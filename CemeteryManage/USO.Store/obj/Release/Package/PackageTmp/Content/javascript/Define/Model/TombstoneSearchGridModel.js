
ModelInfoManager.GridModelInfo.TombstoneSearchModelInfo = {
    TombstoneSearchModelMap: new Ext.util.MixedCollection(),//当前model
    TombstoneSearchColMap: new Ext.util.MixedCollection(),//当前columns
    LoadDefault: function () {
        var me = this;
        me.TombstoneSearchModelMap.clear();
        me.TombstoneSearchColMap.clear();
        //初始化fields
        var TombstoneSearchModelArr = [{
            name: 'Id',
            type: 'string'
        }, {
            name: 'Name',
            type: 'string'
        }, {
            name: 'AreaId',
            type: 'string'
        }, {
            name: 'AreaEntity'
        }, {
            name: 'RowId',
            type: 'string'
        }, {
            name: 'RowEntity'
        }, {
            name: 'ColumnId',
            type: 'string'
        }, {
            name: 'ColumnEntity'
        }, {
            name: 'ParentId',
            type: 'string'
        }, {
            name: 'ParentName',
            type: 'string'
        }, {
            name: 'Alias',
            type: 'string'
        }, {
            name: 'Remark',
            type: 'string'
        }, {
            name: 'CustomerId',
            type: 'string'
        }, {
            name: 'CustomerName',
            type: 'string'
        }, {
            name: 'StoneText',
            type: 'string'
        }, {
            name: 'ExpiryDate',
            mapping: 'ExpiryDateString',
            type: 'string'
        }, {
            name: 'BuyDate',
            mapping: 'BuyDateString',
            type: 'string'
        }, {
            name: 'LastPaymentDate',
            mapping: 'LastPaymentDateString',
            type: 'string'
        }, {
            name: 'BuryDate',
            mapping: 'BuryDateString',
            type: 'string'
        }, {
            name: 'Width',
            type: 'string'
        }, {
            name: 'Height',
            type: 'string'
        }, {
            name: 'Acreage',
            type: 'string'
        }, {
            name: 'SecurityLevelId',
            type: 'string'
        }, {
            name: 'SecurityLevelName'
        }, {
            name: 'Image'
        }, {
            name: 'ServiceLevelId'
        }, {
            name: 'ServiceLevelName'
        }, {
            name: 'TypeId'
        }, {
            name: 'TypeName'
        }, {
            name: 'PaymentStatusId'
        }, {
            name: 'PaymentStatusEntity'
        }, {
            name: 'SortNum'
        }, {
            name: 'TombstoneSearchNumber'
        }, {
            name: 'CustomerBuryDtos'
        }, {
            name: 'CustomerBuryString'
        }];

        Ext.Array.each(TombstoneSearchModelArr, function (item, index, alls) {
            me.TombstoneSearchModelMap.add(item.name, item);
        });
        //初始化columns
        var TombstoneSearchColArr = [{
            id: 'Id',
            text: '编号',
            dataIndex: 'Id',
            width: 60
        }, {
            id: 'Name',
            text: '名称',
            dataIndex: 'Name',
            renderer: GlobalFun.UpdateRecord,
            width: 150
        }, {
            id: 'Alias',
            text: '别名',
            dataIndex: 'Alias',
            renderer: GlobalFun.UpdateRecord,
            width: 150
        }, {
            id: 'AreaEntity',
            text: '区域',
            dataIndex: 'AreaEntity',
            renderer: GlobalFun.UpdateRecordForEntity,
            width: 150
        }];
        //可比state对排序,控制可见
        Ext.Array.each(TombstoneSearchColArr, function (item, index, alls) {
            item.id = '24-' + item.id;
            me.TombstoneSearchColMap.add(item.dataIndex, item);
        });
        //不用排序，取下width,hidden
        //GlobalFun.resumeGridColumns(myStates.infaxgridState.columns, me.infaxColMap);

    }
};

//创建model
ModelManager.GridModel.CretateTombstoneSearchGridModel = function () {
    var tmpArr = [];
    ModelInfoManager.GridModelInfo
        .TombstoneSearchModelInfo
        .TombstoneSearchModelMap.each(function (item, index, alls) {
            tmpArr.push(item);
        });

    Ext.define('chl.Model.TombstoneSearchGridModel', {
        extend: 'Ext.data.Model',
        idProperty: 'Id',
        alternateClassName: 'TombstoneSearchGridModel',
        fields: tmpArr
    });

}