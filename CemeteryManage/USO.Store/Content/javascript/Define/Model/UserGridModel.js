
ModelInfoManager.GridModelInfo.UserModelInfo = {
    UserModelMap: new Ext.util.MixedCollection(),//当前model
    UserColMap: new Ext.util.MixedCollection(),//当前columns
    LoadDefault: function () {
        var me = this;
        me.UserModelMap.clear();
        me.UserColMap.clear();
        //初始化fields
        var UserModelArr = [{
            name: 'Id',
            type: 'string'
        }, {
            name: 'Name',
            type: 'string'
        }, {
            name: 'DepartmentId',
            type: 'string'
        }, {
            name: 'DepartmentEntity'
        }, {
            name: 'LoginName',
            type: 'string'
        }, {
            name: 'Code'
        }, {
            name: 'Remark',
            type: 'string'
        }, {
            name: 'Password'
        }, {
            name: 'Position',
            type: 'string'
        }, {
            name: 'CreateDate',
            mapping: 'CreateDateString',
            type: 'string'
        }, {
            name: 'Status'
        }, {
            name: 'StatusString'
        }, {
            name: 'RoleDtos'
        }, {
            name: 'RoleDtosString'
        }];

        Ext.Array.each(UserModelArr, function (item, index, alls) {
            me.UserModelMap.add(item.name, item);
        });
        //初始化columns
        var UserColArr = [{
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
            id: 'DepartmentEntity',
            text: '部门',
            dataIndex: 'DepartmentEntity',
            renderer: GlobalFun.UpdateRecordForEntity,
            width: 100
        }, {
            id: 'LoginName',
            text: '登录名',
            dataIndex: 'LoginName',
            renderer: GlobalFun.UpdateRecord,
            width: 150
        }, {
            id: 'Code',
            text: '编号',
            dataIndex: 'Code',
            renderer: GlobalFun.UpdateRecord,
            width: 60
        }, {
            id: 'RoleDtosString',
            text: '角色',
            dataIndex: 'RoleDtosString',
            renderer: GlobalFun.UpdateRecord,
            width: 60 
        }, {
            id: 'StatusString',
            text: '状态',
            dataIndex: 'StatusString',
            renderer: GlobalFun.UpdateRecord,
            width: 60
        }, {
            id: 'Remark',
            text: '备注',
            sortable: false,
            dataIndex: 'Remark',
            renderer: GlobalFun.UpdateRecord,
            width: 150
        }, {
            id: 'Position',
            text: '职务',
            dataIndex: 'Position',
            width: 250,
            renderer: GlobalFun.UpdateRecord
        }, {
            id: 'CreateDate',
            text: '创建日期',
            dataIndex: 'CreateDate',
            renderer: GlobalFun.UpdateRecord,
            width: 150
        }];
        //可比state对排序,控制可见
        Ext.Array.each(UserColArr, function (item, index, alls) {
            item.id = '33-' + item.id;
            me.UserColMap.add(item.dataIndex, item);
        });
        //不用排序，取下width,hidden
        //GlobalFun.resumeGridColumns(myStates.infaxgridState.columns, me.infaxColMap);

    }
};

//创建model
ModelManager.GridModel.CretateUserGridModel = function () {
    var tmpArr = [];
    ModelInfoManager.GridModelInfo
        .UserModelInfo
        .UserModelMap.each(function (item, index, alls) {
            tmpArr.push(item);
        });

    Ext.define('chl.Model.UserGridModel', {
        extend: 'Ext.data.Model',
        idProperty: 'Id',
        alternateClassName: 'UserGridModel',
        fields: tmpArr
    });

}