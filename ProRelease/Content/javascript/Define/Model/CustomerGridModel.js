
ModelInfoManager.GridModelInfo.CustomerGridModelInfo = {
    CustomerGridModelMap: new Ext.util.MixedCollection(),//当前model
    CustomerGridColMap: new Ext.util.MixedCollection(),//当前columns
    LoadDefault: function() {
        var me = this;
        me.CustomerGridModelMap.clear();
        me.CustomerGridColMap.clear();
        //初始化fields
        var CustomerGridModelArr = [{
                name: 'Id',
                type: 'string'
            }, {
                name: 'FullName',
                type: 'string'
            }, {
                name: 'LastName',
                type: 'string'
            }, {
                name: 'FirstName',
                type: 'string'
            }, {
                name: 'MiddleName',
                type: 'string'
            }, {
                name: 'Remark',
                type: 'string'
            }, {
                name: 'Telephone',
                type: 'string'
            }, {
                name: 'Phone',
                type: 'string'
            }, {
                name: 'OtherPhone',
                type: 'string'
            }, {
                name: 'Address',
                type: 'string'
            }, {
                name: 'CustomerTypeId',
                type: 'string'
            }, {
                name: 'CustomerTypeName'
            }, {
                name: 'LinkCustomerId',
                type: 'string'
            }, {
                name: 'LinkCustomerName'
            }, {
                name: 'BuryDate',
                mapping: 'BuryDateString',
                type: 'string'
            }, {
                name: 'DeathDate',
                mapping: 'DeathDateString',
                type: 'string'
            }, {
                name: 'CustomerStatusId',
                type: 'string'
            }, {
                name: 'CustomerStatusName'
            }, {
                name: 'NationalityId',
                type: 'string'
            }, {
                name: 'NationalityName'
            }, {
                name: 'IDNumber',
                type: 'string'
            }];

        Ext.Array.each(CustomerGridModelArr, function(item, index, alls) {
            me.CustomerGridModelMap.add(item.name, item);
        });
        //初始化columns
        var CustomerGridColArr = [{
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
                id: 'FullName',
                text: '姓名',
                dataIndex: 'FullName',
                renderer: function (value, metaData, record) {
                    var myVal = record.data.LastName + record.data.MiddleName + record.data.FirstName;
                    return GlobalFun.UpdateRecord(myVal, metaData, record);
                },
                width: 150
            }, {
                id: 'CustomerTypeName',
                text: '客户类别',
                dataIndex: 'CustomerTypeName',
                renderer: GlobalFun.UpdateRecord,
                width: 80
            }, {
                id: 'CustomerStatusName',
                text: '状态',
                dataIndex: 'CustomerStatusName',
                renderer: GlobalFun.UpdateRecord,
                width: 80
            }, {
                id: 'BuryDate',
                text: '下葬日期',
                dataIndex: 'BuryDate',
                renderer: GlobalFun.UpdateRecord,
                width: 150
            }, {
                id: 'DeathDate',
                text: '死亡日期',
                hidden:true,
                dataIndex: 'DeathDate',
                renderer: GlobalFun.UpdateRecord,
                width: 150
            }, {
                id: 'Telephone',
                text: '移动电话',
                dataIndex: 'Telephone',
                renderer: GlobalFun.UpdateRecord,
                width: 150
            }, {
                id: 'IDNumber',
                text: '身份证号',
                dataIndex: 'IDNumber',
                width: 250,
                renderer: GlobalFun.UpdateRecord
            }, {
                id: 'NationalityName',
                text: '国籍',
                dataIndex: 'NationalityName',
                width: 80,
                renderer: GlobalFun.UpdateRecord
            }];
        //可比state对排序,控制可见
        Ext.Array.each(CustomerGridColArr, function(item, index, alls) {
            item.id = '11-' + item.id;
            me.CustomerGridColMap.add(item.dataIndex, item);
        });
        //不用排序，取下width,hidden
        //GlobalFun.resumeGridColumns(myStates.infaxgridState.columns, me.infaxColMap);

    }
};

//创建model
ModelManager.GridModel.CretateCusotmerGridModel = function () {
    var tmpArr = [];
    ModelInfoManager.GridModelInfo
        .CustomerGridModelInfo
        .CustomerGridModelMap.each(function (item, index, alls) {
        tmpArr.push(item);
        });
  
    Ext.define('chl.Model.CustomerGridModel', {
        extend: 'Ext.data.Model',
        idProperty: 'Id',
        alternateClassName: 'CustomerGridModel',
        fields: tmpArr
    });

}