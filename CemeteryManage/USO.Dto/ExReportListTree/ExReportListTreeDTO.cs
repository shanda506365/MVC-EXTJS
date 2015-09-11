using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Dto
{
    public class ExReportListTreeDTO
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 是否叶节点
        /// </summary>
        public bool IsLeaf { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string IconCls { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool Expanded { get; set; }
        /// <summary>
        /// 连接地址
        /// </summary>
        public string LinkSrc { get; set; }

        public ExReportListTreeDTO Parent { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<ExReportListTreeDTO> Children { get; set; }

        /// <summary>
        /// 节点样式
        /// </summary>
        public string Cls { get; set; }
    }
}
