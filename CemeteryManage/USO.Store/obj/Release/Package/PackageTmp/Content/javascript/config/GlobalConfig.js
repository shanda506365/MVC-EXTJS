
//55,24

var login_sessiontoken = Ext.util.Cookies.get("login_sessiontoken");
var login_username = Ext.util.Cookies.get("login_username");
var login_password = Ext.util.Cookies.get("login_password");
var login_checksavepass = Ext.util.Cookies.get("login_checksavepass");




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
Ext.namespace('DropDragControl', 'DropDragControl');


//全局主显示框架Component
GlobalConfig.ViewPort = '';

//全局表格分页控制
GlobalConfig.GridPageSize = 50;

//树管理
Ext.namespace('TreeManager', 'TreeManager');


//是否自动收缩详细
GlobalConfig.DetailAutoCollapse = false;

//全局用户信息
GlobalConfig.CurrUserInfo = "";

//全局Style String管理
GlobalConfig.Css = {
    RemarkDisplay: "white-space:normal; word-break:break-all;",
    RemarkReadOnlyDisplay: "white-space:normal; word-break:break-all;readonly:true;"
};
//全局心跳
GlobalConfig.HeartbeatRunner = new Ext.util.TaskRunner();
//全局消息对话框
//对话框
GlobalConfig.newMessageBox = Ext.create('Ext.window.MessageBox', {});

//全局Controller路径配置
GlobalConfig.Controllers = {
    BuryManGrid: {
        read: '/LoadSysLogGrid',
        addBuryMan: '/BuryTombstone',
        updateBuryMan: '/UpdateBuryMan'
    },
    RenewManageGrid: {
        read: '/GetTombstoneRenewManangeLog'
    },
    JobManage: {
        OrderTombstone: '/OrderTombstone',
        //MaintainTombstone: '/MaintainTombstone',
        BuryTombstone: '/BuryTombstone',
        ClearTombstone: '/ClearTombstone',
        GetOrderJobInfoTombstone: '/GetOrderJobInfoTombstone',//获取墓碑相关的预订信息
        GetTombstoneJobInfoLog: '/GetTombstoneJobInfoLog', //查询 墓碑相关业务信息 日志
        EditApplicanter: '/EditApplicanter',//落葬后的修改申请人
        RenewManageLimit: '/RenewManageLimit',//续交管理费
        GetTombstoneRenewManangeLog: '/GetTombstoneRenewManangeLog' //获取续交管理费记录
    },
    CemeteryAreaGrid: {//区域表
        create: '/LoadCemeteryAreaGrid',
        read: '/LoadCemeteryAreaGrid',
        update: '/LoadCemeteryAreaGrid',
        destroy: '/DelCemeteryArea',
        addCemeteryArea: '/AddCemeteryArea',
        updateCemeteryArea: '/UpdateCemeteryArea',
        delCemeteryArea: '/DelCemeteryArea'
    },
    Heartbeat: '/Heartbeat',//心跳
    RoleGrid: {//角色表
        create: '/LoadRoleGrid',
        read: '/LoadRoleGrid',
        update: '/LoadRoleGrid',
        destroy: '/DelRole',
        addRole: '/AddRole',
        updateRole: '/UpdateRole',
        delRole: '/DelRole',
        LoadFunctions: '/LoadFunctions'
    },
    SysLogGrid: {//日志表
        create: '/LoadSysLogGrid',
        read: '/LoadSysLogGrid',
        addUser: '/AddSysLog'
    },
    User: {//用户登录相关
        GetCurrUserInfo: '/GetCurrUserInfo',
        CheckUserPassword: '/CheckUserPassword',
        UserLoginOut: '/UserLoginOut'
    },
    UserGrid: {//用户表
        create: '/LoadUserGrid',
        read: '/LoadUserGrid',
        update: '/LoadUserGrid',
        destroy: '/DelUser',
        addUser: '/AddUser',
        updateUser: '/UpdateUser',
        delUser: '/DelUser'
    },
    CustomerGrid: {//客户表
        create: '/LoadCustomerGrid',
        read: '/LoadCustomerGrid',
        update: '/LoadCustomerGrid',
        destroy: '/DelCustomer',
        addCustomer: '/AddCustomer',
        updateCustomer: '/UpdateCustomer',
        delCustomer: '/DelCustomer',
        outPutExcelCustomer: '/OutPutExcelCustomer',
        uploadCustomerExcel: '/UploadCustomerExcel'
    },
    TombstoneGrid: {//墓碑表
        create: '/LoadTombstoneGrid',
        read: '/LoadTombstoneGrid',
        update: '/LoadTombstoneGrid',
        destroy: '/DelTombstone',
        addTombstone: '/AddTombstone',
        addTombstoneRowList: '/AddTombstoneRowList',
        updateTombstone: '/UpdateTombstone',
        delTombstone: '/DelTombstone',
        outPutExcelTombstone: '/OutPutExcelTombstone',
        SortTombstonePng: '/SortTombstonePng',//排序
        UnBuryPeopleTombstone: '/UnBuryPeopleTombstone',//解除落葬
        BuryPeopleTombstone: '/BuryPeopleTombstone'//落葬
    },
    MainItemListTree: '/LoadMainItemListTree',//主目录树
    ExReportListTree: '/LoadExReportListTree',//报表统计树
    BaseEnumListTree: '/LoadBaseEnumListTree',//基础数据维护树
    ComboStore: {
        LoadRoleStore: 'LoadRoleStore',//角色
        DepartmentStore: 'LoadDepartmentStore',//部门
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
    // 数字验证用正则表达式
    regexNumberF: /^([-]\d*|\d*)$/,
    //手机号码验证
    regexTelePhoneNumber: /^\d{11}$/,
    // 传真号码匹配的正则表达式
    regexFaxNumber: /(^sip\:[0-9a-zA-Z#-_]+@[0-9a-zA-Z.-_]+$)|(^\d+-(\d|\#|\*)*$)|(^\(\d+\)(\d|\#|\*)*$)|(^\+[\d][123457890](\d*) (\d|\#|\*)+$)|(^\+[123456790][\d](\d*) (\d|\#|\*)+$)|(^\+[1] (\d|\#|\*)+$)|(^\+[7] (\d|\#|\*)+$)|(^\+[\d][6](\d+) (\d|\#|\*)+$)|(^\+(\d+)\(\d+\)(\d|\#|\*)+$)|(^(0+[1-9]\d{2,})$)|(^\d+$)/,
    //文件夹名称验证
    regexFolderName: /^[^\\/?: *"<>|]+$/,
    //邮箱验证正则表达式
    regexEmail: /^(\w|\.)+\@\w+\.\w+$/,
    //墓区全编码验证
    regexTombstoneCode: /^(\d{3}|\d{5}|\d{7})$/,
    //墓区编码验证
    regexAreaCode: /^\d{3}$/,
    //金额验证，两位小数
    regexMoney2Fixed: /^[1-9]\d{0,8}((\.)?\d{1,2})?$/
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
    LoginFail: 0x0F000112,	//登录失败
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


var HomeIntroText = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
"成都市皇恩寺陵园是在2001年经四川省民政厅批准为永久性得墓地，在原皇恩寺得旧址上扩地修建了集休闲、园林于一体得现代大型公墓区“皇恩寺陵园”。" +
"成都市皇恩寺陵园系经四川省民政厅、成都市民政局批准注册，金牛区民政局主管合法社会公共墓地。" +
"陵园地处城北天回山，毗邻植物园，四季树木葱郁、鲜花簇拥。" +
"陵园扩地建于具有1200年历史包含深厚文化底蕴，从唐至清历朝御赐皇家陵园的皇恩寺遗址。陵园靠山向阳、树木葱笼、视野开阔。" +
"居高望远，气若君临天下，势如虎踞龙蟠。尽显浩浩皇恩，泽慧万千子民，乃极佳福祥之地。" +
"陵园建设者承吸明清建筑之精髓，融汇现代建筑之技艺，数十种墓穴款式彰出与山水相融，与花木相依，与天地相合之建园思想。" +
"神道长廊。天地牌坊、亭台楼阁、雕梁玉砌，依山而上，浑然一体。龟鹤池中数千锦鲤竞相畅游，龙庭泉眼翻滚万千珠玑，尽显山水之生机，天地之灵气。" +
"后辈亲朋时至此，凭吊先辈、追念亲友、缅怀故旧，于信步休闲，淡泊宁静中，悟人生之真谛，感亲情之珍贵。" +
"仙逝者安息于四季鲜花，绿荫苍翠之龙脉天山，伴岁月之悠悠，纳日月之精华，庇佑生者，赐福在世。" +
"诗曰：憩于苍山中，遥望锦官城，城与山相思，山与城相望，相思相望，想望相思，皇恩寺畔，天回山上。";