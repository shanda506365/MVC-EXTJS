using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Domain;

namespace USO.Dto
{
    public class FunctionDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 父操作Id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 父操作功能
        /// </summary>
        public Function Parent { get; set; }
    }
}
