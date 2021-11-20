using System;
using System.Collections.Generic;
using System.Text;

namespace TT.StoreManager.Model
{
    public class PagingRequest
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public string Filter { get; set; }
    }
}
