using System;
using System.Collections.Generic;
using System.Text;

namespace TT.StoreManager.Model
{
    public class ConfigTableAttribute : Attribute
    {
        public string TableName { get; set; }

        public ConfigTableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }
}
