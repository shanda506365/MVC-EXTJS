
ModelInfoManager.GridModelInfo.SysLogModelInfo = {
    SysLogModelMap: new Ext.util.MixedCollection(),//当前model
    SysLogColMap: new Ext.util.MixedCollection(),//当前columns
    LoadDefault: function () {
        var me = this;
        me.SysLogModelMap.clear();
        me.SysLogColMap.clear();
        //初始化fields
        var SysLogModelArr = [{
            name: 'Id',
            type: 'string'
        }, {
            name: 'Type',
            type: 'string'
        }, {
            name: 'TypeString',
            type: 'string'
        }, {
            name: 'ControlName'
        }, {
            name: 'Content',
            type: 'string'
        }, {
            name: 'UserId'
        }, {
            name: 'UserEntity'
        }, {
            name: 'Date',
            mapping: 'DateString',
            type: 'string'
        }];

        Ext.Array.each(SysLogModelArr, function (item, index, alls) {
            me.SysLogModelMap.add(item.name, item);
        });
        //初始化columns
        var SysLogColArr = [{
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
            id: 'TypeString',
            text: '日志类型',
            dataIndex: 'TypeString',
            renderer: GlobalFun.UpdateRecord,
            width: 100
        }, {
            id: 'ControlName',
            text: '操作记录',
            dataIndex: 'ControlName',
            renderer: GlobalFun.UpdateRecord,
            width: 100
        }, {
            id: 'UserEntity',
            text: '操作人',
            dataIndex: 'UserEntity',
            renderer: GlobalFun.UpdateRecordForEntity,
            width:150
        }, {
            id: 'Date',
            text: '操作时间',
            dataIndex: 'Date',
            renderer: GlobalFun.UpdateRecord,
            width: 150
        }, {
            id: 'Content',
            text: '日志文本',
            dataIndex: 'Content',
            renderer: GlobalFun.UpdateRecord,
            width: 260
        }];
        //可比state对排序,控制可见
        Ext.Array.each(SysLogColArr, function (item, index, alls) {
            item.id = '44-' + item.id;
            me.SysLogColMap.add(item.dataIndex, item);
        });
        //不用排序，取下width,hidden
        //GlobalFun.resumeGridColumns(myStates.infaxgridState.columns, me.infaxColMap);

    }
};

//创建model
ModelManager.GridModel.CretateSysLogGridModel = function () {
    var tmpArr = [];
    ModelInfoManager.GridModelInfo
        .SysLogModelInfo
        .SysLogModelMap.each(function (item, index, alls) {
            tmpArr.push(item);
        });

    Ext.define('chl.Model.SysLogGridModel', {
        extend: 'Ext.data.Model',
        idProperty: 'Id',
        alternateClassName: 'SysLogGridModel',
        fields: tmpArr
    });

}