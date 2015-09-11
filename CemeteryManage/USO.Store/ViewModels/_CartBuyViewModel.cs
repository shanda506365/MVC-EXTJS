using System;
using System.Collections.Generic;
using System.Linq;
using USO.Dto.Products;
using System.Web;
using USO.Dto;
using USO.Dto.Customer;

namespace USO.Web
{
    public class _CartBuyViewModel
    {
        public _CartBuyViewModel()
        {

        }

        public ProductDTO product { get; set; }
        public CartDetailDTO cartDetail { get; set; }

        public List<R3CustomerPlaceDTO> R3CustomerPlaceList { get; set; }

        /// <summary>
        /// 是否审核
        /// </summary>
        public int RequestAudit { get; set; }

        public decimal ProductCountSum { get; set; }

        public decimal productSumPrice { get; set; }
        public string oName { get; set; }
        public string sName { get; set; }

    }
}