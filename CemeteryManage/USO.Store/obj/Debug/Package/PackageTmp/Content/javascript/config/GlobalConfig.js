
//存储管理
Ext.namespace('GlobalFun', 'GlobalFun');
//存储管理
Ext.namespace('StoreManager', 'StoreManager.GridStore');//表格数据
Ext.namespace('StoreManager', 'StoreManager.ComboStore');//下拉列表数据
//模型管理
Ext.namespace('ModelManager', 'ModelManager.GridModel');
//模型初始化管理
Ext.namespace('ModelInfoManager', 'ModelInfoManager.GridModelInfo');
//表格管理
Ext.namespace('GridManager', 'GridManager');
//通用窗口管理
Ext.namespace('WindowManager', 'WindowManager');
//action 管理
Ext.namespace('ActionManager', 'ActionManager');

//全局数据管理
Ext.namespace('GlobalConfig', 'GlobalConfig');

//全局拖拽控件操作类
Ext.namespace('DropDragControl','DropDragControl');


//全局主显示框架Component
GlobalConfig.ViewPort = '';

//全局表格分页控制
GlobalConfig.GridPageSize = 50;

//树管理
Ext.namespace('TreeManager', 'TreeManager');

//全局消息对话框
//对话框
GlobalConfig.newMessageBox = Ext.create('Ext.window.MessageBox', {});

//全局Controller路径配置
GlobalConfig.Controllers = {
    CustomerGrid: {//客户表
        create: '/LoadCustomerGrid',
        read: '/LoadCustomerGrid',
        update: '/LoadCustomerGrid',
        destroy: '/DelCustomer',
        addCustomer: '/AddCustomer',
        updateCustomer: '/UpdateCustomer',
        delCustomer: '/DelCustomer',
        outPutExcelCustomer: '/OutPutExcelCustomer',
        uploadCustomerExcel: '/UploadCustomerExcel',
    },
    TombstoneGrid: {
        create: '/LoadTombstoneGrid',
        read: '/LoadTombstoneGrid',
        update: '/LoadTombstoneGrid',
        destroy: '/DelTombstone',
        addTombstone: '/AddTombstone',
        updateTombstone: '/UpdateTombstone',
        delTombstone: '/DelTombstone',
        outPutExcelTombstone: '/OutPutExcelTombstone',
        SortTombstonePng: '/SortTombstonePng'
    },
    MainItemListTree: '/LoadMainItemListTree',//主目录树
    ExReportListTree: '/LoadExReportListTree',//报表统计树
    BaseEnumListTree: '/LoadBaseEnumListTree',//基础数据维护树
    ComboStore: {
        PaymentStatusStore: 'LoadPaymentStatusStore',//付款状态
        TombstoneTypeStore: 'LoadTombstoneTypeStore',//墓碑类别
        AreaStore: '/LoadAreaStore',//区域
        RowStore: '/LoadRowStore',//行
        ColumnStore: '/LoadColumnStore',//列
        SecurityLevelStore: '/LoadSecurityLevelStore',//保密等级
        ServiceLevelStore: '/LoadServiceLevelStore',//服务等级
        CustomerTypeStore: '/LoadCustomerTypeStore',//客户类别
        NationalityStore: '/LoadNationalityStore',//国籍
        CustomerStatusStore: '/LoadCustomerStatusStore'//客户状态
    }
};
//全局正则表达式
GlobalConfig.RegexController = {
    // 数字验证用正则表达式
    regexNumber: /^\d*$/,
    //手机号码验证
    regexTelePhoneNumber: /^\d{11}$/,
    // 传真号码匹配的正则表达式
    regexFaxNumber: /(^sip\:[0-9a-zA-Z#-_]+@[0-9a-zA-Z.-_]+$)|(^\d+-(\d|\#|\*)*$)|(^\(\d+\)(\d|\#|\*)*$)|(^\+[\d][123457890](\d*) (\d|\#|\*)+$)|(^\+[123456790][\d](\d*) (\d|\#|\*)+$)|(^\+[1] (\d|\#|\*)+$)|(^\+[7] (\d|\#|\*)+$)|(^\+[\d][6](\d+) (\d|\#|\*)+$)|(^\+(\d+)\(\d+\)(\d|\#|\*)+$)|(^(0+[1-9]\d{2,})$)|(^\d+$)/,
    //文件夹名称验证
    regexFolderName: /^[^\\/?: *"<>|]+$/,
    //邮箱验证正则表达式
    regexEmail: /^(\w|\.)+\@\w+\.\w+$/
};


//全局错误定义
GlobalConfig.ErrorCode = {
    RootFolderID: 0,
    RecycleFolderID: 0x3FFFFFFE,
    PublicRootFolderID: 0x7FFFFFF0,
    PublicRecycleFolderID: 0x7FFFFFFE,
    DontCareFolderID: 0x3FFFFFFF, // 任何目录,在某些函数的调用中,此参数表示忽略目录ID
    // 操作返回错误码
    ResOK: 0x00000000, // 操作成功完成
    ResFullPrint: 0x00000018, // 没有空闲的传真文档转换队列
    ResPartialOK: 0x0F000001, // 操作部分成功完成,用于需要查询记录状态以确定操作结果
    ResSessionTokenError: 0x0F000002, // 无效登录事务标识
    ResSessionTimeOut: 0x0F000004, // 此登录事务已超时,请重新登录
    ResOperationDeny: 0x0F000008, // 事务标识所属用户无权限
    ResDBError: 0x0F000009, // 服务器数据库异常
    ResInternalError: 0x0F000010, // 服务器内部异常
    ResParamError: 0x0F000011, // 参数错误
    ResSystemError: 0x0F000014, // 服务器操作系统异常
    ResInputParamsError: 0x0F000018, // 传入参数错误
    ResConnectServerFailed: 0x0F000019, // 连接服务器失败
    ResOperationNoSense: 0x0F000020, // 操作无意义
    ResHasNotImplement: 0x0F000021,	// 操作还未实现
    ConServerError: 0x6FFFFFFB,			//主控服务异常，连接失败

    CasLogoutCode: 0x6FFFFFFE,	//cas登出标识代码

    //登录失败错误码
    ResNoUserName: 0x0F000110,	//无效用户名
    ResPasswordError: 0x0F000111,	//密码错误
    ResUserInvalid: 0x0F000113,	//用户被冻结或无效
    ResDomainLoginFailed: 0x0F000114,	//映射域登陆验证失败

    ResInvalidFolderID: 0x0F000030,// 	无效目录ID
    ResFolderTypeNotMatch: 0x0F000032,// 	目录类型不匹配
    ResDupFolderName: 0x0F000033,// 	目录名称重复
    ResFolderNotEmpty: 0x0F000034,// 	目录非空
    ResTooManyFolders: 0x0F000038,// 	目录数量超过限制

    ResInvalidDocID: 0x0F000E31,//无效的归档ID
    ResInvalidDocInfo: 0x0F000E32,//无效的归档信息
    
    SeesionTimeOut: 0x0F000E33//登录信息失效
};