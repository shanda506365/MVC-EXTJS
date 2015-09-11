
ModelInfoManager.GridModelInfo.TombstoneModelInfo = {
    TombstoneModelMap: new Ext.util.MixedCollection(),//当前model
    TombstoneColMap: new Ext.util.MixedCollection(),//当前columns
    LoadDefault: function () {
        var me = this;
        me.TombstoneModelMap.clear();
        me.TombstoneColMap.clear();
        //初始化fields
        var TombstoneModelArr = [{
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
            name: 'TombstoneNumber'
        }, {
            name: 'CustomerBuryDtos'
        }, {
            name: 'CustomerBuryString'
        }];

        Ext.Array.each(TombstoneModelArr, function (item, index, alls) {
            me.TombstoneModelMap.add(item.name, item);
        });
        //初始化columns
        var TombstoneColArr = [{
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
        }, {
            id: 'SortNum',
            text: '排序字段',
            dataIndex: 'SortNum',
            renderer: GlobalFun.UpdateRecord,
            width: 60
        }, {
            id: 'LastPaymentDate',
            text: '补交日期',
            dataIndex: 'LastPaymentDate',
            renderer: GlobalFun.UpdateRecord,
            width: 150
        }, {
            id: 'BuyDate',
            text: '出售日期',
            dataIndex: 'BuyDate',
            renderer: GlobalFun.UpdateRecord,
            width: 150
        }, {
            id: 'BuryDate',
            text: '落葬日期',
            dataIndex: 'BuryDate',
            renderer: GlobalFun.UpdateRecord,
            width: 150
        }, {
            id: 'SecurityLevelName',
            text: '保密等级',
            dataIndex: 'SecurityLevelName',
            renderer: GlobalFun.UpdateRecord,
            width: 150
        }, {
            id: 'ServiceLevelName',
            text: '服务级别',
            dataIndex: 'ServiceLevelName',
            width: 250,
            renderer: GlobalFun.UpdateRecord
        }, {
            id: 'PaymentStatusEntity',
            text: '付款状态',
            dataIndex: 'PaymentStatusEntity',
            renderer: GlobalFun.UpdateRecordForEntity,
            width: 150
        }, {
            id: 'CustomerBuryString',
            text: '落葬人',
            dataIndex: 'CustomerBuryString',
            width: 150,
            renderer: GlobalFun.UpdateRecord
        }];
        //可比state对排序,控制可见
        Ext.Array.each(TombstoneColArr, function (item, index, alls) {
            item.id = '22-' + item.id;
            me.TombstoneColMap.add(item.dataIndex, item);
        });
        //不用排序，取下width,hidden
        //GlobalFun.resumeGridColumns(myStates.infaxgridState.columns, me.infaxColMap);

    }
};

//创建model
ModelManager.GridModel.CretateTombstoneGridModel = function () {
    var tmpArr = [];
    ModelInfoManager.GridModelInfo
        .TombstoneModelInfo
        .TombstoneModelMap.each(function (item, index, alls) {
            tmpArr.push(item);
        });

    Ext.define('chl.Model.TombstoneGridModel', {
        extend: 'Ext.data.Model',
        idProperty: 'Id',
        alternateClassName: 'TombstoneGridModel',
        fields: tmpArr
    });

}