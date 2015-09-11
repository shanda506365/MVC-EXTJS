using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using USO.Dto;

namespace USO.Store.ViewModels
{
    public class TombstoneViewModel
    {
        public TombstoneViewModel()
        {
            TombstoneList = new List<TombstoneDTO>();
        }

        public List<TombstoneDTO> TombstoneList;
    }
}