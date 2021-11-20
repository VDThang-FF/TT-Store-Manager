using System;
using System.Collections.Generic;
using System.Text;

namespace TT.StoreManager.Model
{
    public class PagingResponse
    {
        public object PageData { get; set; }
        public int Total { get; set; }
    }
}
