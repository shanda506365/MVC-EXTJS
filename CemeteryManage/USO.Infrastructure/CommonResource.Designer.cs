﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18047
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace USO.Infrastructure {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CommonResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CommonResource() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("USO.Infrastructure.CommonResource", typeof(CommonResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 &quot;{0}&quot; 不能为空. 的本地化字符串。
        /// </summary>
        internal static string ArgumentCannotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("ArgumentCannotBeNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 信用额度不够 的本地化字符串。
        /// </summary>
        internal static string CreditIsNotEnough {
            get {
                return ResourceManager.GetString("CreditIsNotEnough", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 1001 的本地化字符串。
        /// </summary>
        internal static string DefaultStoreUserID {
            get {
                return ResourceManager.GetString("DefaultStoreUserID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 前台用户 的本地化字符串。
        /// </summary>
        internal static string DefaultStoreUserName {
            get {
                return ResourceManager.GetString("DefaultStoreUserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 验单失败 的本地化字符串。
        /// </summary>
        internal static string InvalidOrder {
            get {
                return ResourceManager.GetString("InvalidOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 在创建订单的过程中出现异常 的本地化字符串。
        /// </summary>
        internal static string OneOrderCreateFailure {
            get {
                return ResourceManager.GetString("OneOrderCreateFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户：&quot;{0}&quot;在:&quot;{1}&quot; 作废了订单:&quot;{2}&quot; 的本地化字符串。
        /// </summary>
        internal static string OrderLogAction_DeleteTemplate {
            get {
                return ResourceManager.GetString("OrderLogAction_DeleteTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户：&quot;{0}&quot;在:&quot;{1}&quot; 撤销作废订单:&quot;{2}&quot; 的本地化字符串。
        /// </summary>
        internal static string OrderLogAction_UndoDeleteTemplate {
            get {
                return ResourceManager.GetString("OrderLogAction_UndoDeleteTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 订单操作 的本地化字符串。
        /// </summary>
        internal static string OrderLogActionName {
            get {
                return ResourceManager.GetString("OrderLogActionName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 \r\n----------------------------------------------------------------------------------------------------------------\r\n 的本地化字符串。
        /// </summary>
        internal static string OrderLogBreakLine {
            get {
                return ResourceManager.GetString("OrderLogBreakLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户&quot;{0}&quot; 创建了订单号为&quot;{1}&quot;的订单，当前订单状态为&quot;{2}&quot;。 的本地化字符串。
        /// </summary>
        internal static string OrderLogMsgTemplate {
            get {
                return ResourceManager.GetString("OrderLogMsgTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 订单编号：&quot;{0}&quot; 订单金额：&quot;{1}&quot; 产品金额:&quot;{2}&quot; 折扣总金额：&quot;{3}&quot; 订单来源：&quot;{4}&quot; \r\n 的本地化字符串。
        /// </summary>
        internal static string OrderLogOrderMsgTemplate {
            get {
                return ResourceManager.GetString("OrderLogOrderMsgTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 商品编号：&quot;{0}&quot; 名称：&quot;{1} &quot;价格：&quot;{2} &quot;数量：&quot;{3}&quot; 返利金额：&quot;{4}&quot; \r\n 的本地化字符串。
        /// </summary>
        internal static string OrderLogProductMsgTemplate {
            get {
                return ResourceManager.GetString("OrderLogProductMsgTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户：&quot;{0}&quot;在&quot;{1}&quot; &quot;{2}&quot;订单\r\n 的本地化字符串。
        /// </summary>
        internal static string OrderLogUserMsgTemplate {
            get {
                return ResourceManager.GetString("OrderLogUserMsgTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 编号为&quot;{0}&quot;的产品不存在. 的本地化字符串。
        /// </summary>
        internal static string ProductNotExist {
            get {
                return ResourceManager.GetString("ProductNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 R3订单操作 的本地化字符串。
        /// </summary>
        internal static string R3OrderLogActionName {
            get {
                return ResourceManager.GetString("R3OrderLogActionName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 物料号:&quot;{0}&quot; 价格:&quot;{1}&quot; 数量:&quot;{2}&quot; \r\n 的本地化字符串。
        /// </summary>
        internal static string R3OrderLogOrderItemMsgTemplate {
            get {
                return ResourceManager.GetString("R3OrderLogOrderItemMsgTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 订单编号：&quot;{0}&quot; R3编码：&quot;{1}&quot; R3销售组织：&quot;{2}&quot; R3销售办事处：&quot;{3}&quot;  \r\n 的本地化字符串。
        /// </summary>
        internal static string R3OrderLogOrderMsgTemplate {
            get {
                return ResourceManager.GetString("R3OrderLogOrderMsgTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 返利编号:&quot;{0}&quot; 返利金额:&quot;{1}&quot; \r\n 的本地化字符串。
        /// </summary>
        internal static string R3OrderLogOrderRebateMsgTemplate {
            get {
                return ResourceManager.GetString("R3OrderLogOrderRebateMsgTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户：&quot;{0}&quot;在&quot;{1}&quot; &quot;{2}&quot;R3订单 \r\n 的本地化字符串。
        /// </summary>
        internal static string R3OrderLogUserMsgTemplate {
            get {
                return ResourceManager.GetString("R3OrderLogUserMsgTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 只有通过审核的产品才能修改销售状态 的本地化字符串。
        /// </summary>
        internal static string UpdateSalesStatusRule1 {
            get {
                return ResourceManager.GetString("UpdateSalesStatusRule1", resourceCulture);
            }
        }
    }
}
