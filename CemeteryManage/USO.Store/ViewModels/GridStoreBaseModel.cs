using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using USO.Dto;

namespace USO.Store.ViewModels
{
    public class GridStoreBaseModel<T>
    {
      
        /// <summary>
        /// 成功标识
        /// </summary>
        public bool success;

        /// <summary>
        /// 消息
        /// </summary>
        public string msg;

        /// <summary>
        /// 总数量
        /// </summary>
        public int total;

        /// <summary>
        /// 数据集
        /// </summary>
        public List<T> dataset{get;set;}
    }
 
}