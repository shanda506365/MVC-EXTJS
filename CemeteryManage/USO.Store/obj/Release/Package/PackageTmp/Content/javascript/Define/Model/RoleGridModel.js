
ModelInfoManager.GridModelInfo.RoleModelInfo = {
    RoleModelMap: new Ext.util.MixedCollection(),//当前model
    RoleColMap: new Ext.util.MixedCollection(),//当前columns
    LoadDefault: function () {
        var me = this;
        me.RoleModelMap.clear();
        me.RoleColMap.clear();
        //初始化fields
        var RoleModelArr = [{
            name: 'Id'
        }, {
            name: 'Name'
        }, {
            name: 'FunctionDtos'
        }, {
            name: 'FunctionsString'
        }];

        Ext.Array.each(RoleModelArr, function (item, index, alls) {
            me.RoleModelMap.add(item.name, item);
        });
        //初始化columns
        var RoleColArr = [{
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
            id: 'FunctionsString',
            sortable: false,
            text: '所有功能',
            dataIndex: 'FunctionsString',
            renderer: GlobalFun.UpdateRecord,
            width: 450
        }];
        //可比state对排序,控制可见
        Ext.Array.each(RoleColArr, function (item, index, alls) {
            item.id = '55-' + item.id;
            me.RoleColMap.add(item.dataIndex, item);
        });
        //不用排序，取下width,hidden
        //GlobalFun.resumeGridColumns(myStates.infaxgridState.columns, me.infaxColMap);

    }
};

//创建model
ModelManager.GridModel.CretateRoleGridModel = function () {
    var tmpArr = [];
    ModelInfoManager.GridModelInfo
        .RoleModelInfo
        .RoleModelMap.each(function (item, index, alls) {
            tmpArr.push(item);
        });

    Ext.define('chl.Model.RoleGridModel', {
        extend: 'Ext.data.Model',
        idProperty: 'Id',
        alternateClassName: 'RoleGridModel',
        fields: tmpArr
    });

}