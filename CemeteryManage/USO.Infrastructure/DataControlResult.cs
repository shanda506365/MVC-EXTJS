using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Infrastructure
{
    public class DataControlResult<T>
    {
        public DataControlResult()
        {
            ResultOutDto = default(T);
            ResultOutDtos = new List<T>();
            success = false;
            msg = "初始化失败";
            code = MyErrorCode.ResSystemError;
        }

        public T ResultOutDto { get; set;}
        public List<T> ResultOutDtos { get; set; }
        public bool success { get; set; }
        public string msg { get; set; }
        public MyErrorCode code { get; set; }
    }

    public enum MyErrorCode
    {
        RootFolderID = 0,
        RecycleFolderID = 0x3FFFFFFE,
        PublicRootFolderID = 0x7FFFFFF0,
        PublicRecycleFolderID = 0x7FFFFFFE,
        // 任何目录,在某些函数的调用中,此参数表示忽略目录ID
        DontCareFolderID = 0x3FFFFFFF, 
        // 操作返回错误码
        // 操作成功完成
        ResOK = 0x00000000,
        // 没有空闲的传真文档转换队列
        ResFullPrint = 0x00000018,
        // 操作部分成功完成,用于需要查询记录状态以确定操作结果
        ResPartialOK = 0x0F000001, 
        ResSessionTokenError = 0x0F000002, // 无效登录事务标识
        ResSessionTimeOut = 0x0F000004, // 此登录事务已超时,请重新登录
        ResOperationDeny = 0x0F000008, // 事务标识所属用户无权限
        ResDBError = 0x0F000009, // 服务器数据库异常
        /// <summary>
        /// 服务器内部异常
        /// </summary>
        ResInternalError = 0x0F000010, // 服务器内部异常
        ResParamError = 0x0F000011, // 参数错误
        ResSystemError = 0x0F000014, // 服务器操作系统异常
        ResInputParamsError = 0x0F000018, // 传入参数错误
        ResConnectServerFailed = 0x0F000019, // 连接服务器失败
        ResOperationNoSense = 0x0F000020, // 操作无意义
        ResHasNotImplement = 0x0F000021,	// 操作还未实现
        ConServerError = 0x6FFFFFFB,			//主控服务异常，连接失败

        CasLogoutCode = 0x6FFFFFFE,	//cas登出标识代码

        //登录失败错误码
        ResNoUserName = 0x0F000110,	//无效用户名
        ResPasswordError = 0x0F000111,	//密码错误
        LoginFail = 0x0F000112,	//登录失败
        ResUserInvalid = 0x0F000113,	//用户被冻结或无效
        ResDomainLoginFailed = 0x0F000114,	//映射域登陆验证失败

        ResInvalidFolderID = 0x0F000030,// 	无效目录ID
        ResFolderTypeNotMatch = 0x0F000032,// 	目录类型不匹配
        ResDupFolderName = 0x0F000033,// 	目录名称重复
        ResFolderNotEmpty = 0x0F000034,// 	目录非空
        ResTooManyFolders = 0x0F000038,// 	目录数量超过限制

        ResInvalidDocID = 0x0F000E31,//无效的归档ID
        ResInvalidDocInfo = 0x0F000E32,//无效的归档信息
        SeesionTimeOut= 0x0F000E33//登录信息失效
    }
}
