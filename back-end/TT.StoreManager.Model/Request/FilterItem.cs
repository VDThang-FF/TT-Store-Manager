using System;
using System.Collections.Generic;
using System.Text;

namespace TT.StoreManager.Model
{
    public class FilterItem
    {
        public string Prefix { get; set; }
        public string PropName { get; set; }
        public string Condition { get; set; }
        public object Value { get; set; }
    }
}
