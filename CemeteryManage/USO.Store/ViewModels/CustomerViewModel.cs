using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using USO.Dto;

namespace USO.Store.ViewModels
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            CustomerList = new List<CustomerDTO>();
        }

        public List<CustomerDTO> CustomerList;
    }
}