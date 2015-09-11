//角色Store
StoreManager.ComboStore.RoleStore = Ext.create('Ext.data.Store', {
    storeId: 'RoleStoreId',
    fields: ['Id', 'Name'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.LoadRoleStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'Role',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        },
        listeners: {
            exception: function (proxy, response, operation) {
                var json = Ext.JSON.decode(response.responseText);
                var code = json.code;
                GlobalFun.errorProcess(code);
            }
        }
    },
    autoLoad: false
});
//部门Sotre
StoreManager.ComboStore.DepartmentStore = Ext.create('Ext.data.Store', {
    storeId: 'DepartmentStoreId',
    fields: ['Id', 'Name'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.DepartmentStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'Department',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        },
        listeners: {
            exception: function (proxy, response, operation) {
                var json = Ext.JSON.decode(response.responseText);
                var code = json.code;
                GlobalFun.errorProcess(code);
            }
        }
    },
    autoLoad: false
});

//付款状态Sotre
StoreManager.ComboStore.PaymentStatusStore = Ext.create('Ext.data.Store', {
    storeId: 'PaymentStatusStoreId',
    fields: ['Id', 'Name', 'Alias', 'Remark'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.PaymentStatusStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'PaymentStatus',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        },
        listeners: {
            exception: function (proxy, response, operation) {
                var json = Ext.JSON.decode(response.responseText);
                var code = json.code;
                GlobalFun.errorProcess(code);
            }
        }
    },
    autoLoad: false
});

//墓碑类别Sotre
StoreManager.ComboStore.TombstoneTypeStore = Ext.create('Ext.data.Store', {
    storeId: 'TombstoneTypeStoreId',
    fields: ['Id', 'Name', 'Alias', 'Remark'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.TombstoneTypeStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'TombstoneType',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        }
    },
    autoLoad: false
});

//区域Sotre
StoreManager.ComboStore.AreaStore = Ext.create('Ext.data.Store', {
    storeId: 'AreaStoreId',
    fields: ['Id', 'Name', 'Alias', 'Remark'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.AreaStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'Area',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        }
    },
    autoLoad: false
});
//行Sotre
StoreManager.ComboStore.RowStore = Ext.create('Ext.data.Store', {
    storeId: 'RowStoreId',
    fields: ['Id', 'Name', 'Alias', 'Remark'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.RowStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'Row',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        }
    },
    autoLoad: false
});
//列Sotre
StoreManager.ComboStore.ColumnStore = Ext.create('Ext.data.Store', {
    storeId: 'ColumnStoreId',
    fields: ['Id', 'Name', 'Alias', 'Remark'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.ColumnStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'Column',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        }
    },
    autoLoad: false
});
//保密等级Sotre
StoreManager.ComboStore.SecurityLevelStore = Ext.create('Ext.data.Store', {
    storeId: 'SecurityLevelStoreId',
    fields: ['Id', 'Name', 'Alias', 'Remark'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.SecurityLevelStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'SecurityLevel',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        }
    },
    autoLoad: false
});
//服务等级Sotre
StoreManager.ComboStore.ServiceLevelStore = Ext.create('Ext.data.Store', {
    storeId: 'ServiceLevelStoreId',
    fields: ['Id', 'Name', 'Alias', 'Remark'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.ServiceLevelStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'ServiceLevel',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        }
    },
    autoLoad: false
});



//客户类型Sotre
StoreManager.ComboStore.CustomerTypeStore = Ext.create('Ext.data.Store', {
    storeId: 'CustomerTypeStoreId',
    fields: ['Id', 'Name', 'Description'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.CustomerTypeStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'CustomerType',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        }
    },
    autoLoad: false
});

//国籍Store
StoreManager.ComboStore.NationalityStore = Ext.create('Ext.data.Store', {
    storeId: 'NationalityStoreId',
    fields: ['Id', 'Name'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.NationalityStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'Nationality',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        }
    },
    autoLoad: false
});

//客户状态Store
StoreManager.ComboStore.CustomerStatusStore = Ext.create('Ext.data.Store', {
    storeId: 'CustomerStatusStoreId',
    fields: ['Id', 'Name'],
    proxy: {
        type: 'ajax',
        actionMethods: 'POST',
        url: GlobalConfig.Controllers.ComboStore.CustomerStatusStore,
        reader: {
            type: 'json'
        },
        extraParams: {
            req: 'dataset',
            dataname: 'CustomerStatus',
            restype: 'json',
            sessiontoken: GlobalFun.getSeesionToken()
        }
    },
    autoLoad: false
});


/*================静态Store==============*/
//用户状态
StoreManager.ComboStore.UserStatusStore= Ext.create('Ext.data.Store', {
    storeId: 'UserStatusStoreId',
    fields: ['Id', 'Name'],
    data: [
        { "Id": 1, "Name": "有效" },
        { "Id": 0, "Name": "无效" }
    ]
});
//日志类型
StoreManager.ComboStore.SysLogTypeStore = Ext.create('Ext.data.Store', {
    storeId: 'SysLogTypeStoreId',
    fields: ['Id', 'Name'],
    data: [
        { "Id": 1, "Name": "系统日志" },
        { "Id": 2, "Name": "操作日志" },
        { "Id": 3, "Name": "错误日志" },
        { "Id": 4, "Name": "警告日志" },
        { "Id": 5, "Name": "业务操作" }
    ]
});
//墓区排排序类型
StoreManager.ComboStore.AreaRowSortStore = Ext.create('Ext.data.Store', {
    storeId: 'AreaRowSortStoreId',
    fields: ['Id', 'Name'],
    data: [
        { "Id": 'ASC', "Name": "正序" },
        { "Id": 'DESC', "Name": "倒序" }
    ]
});