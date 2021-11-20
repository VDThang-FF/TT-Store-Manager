using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using static TT.StoreManager.Model.Enumarations;

namespace TT.StoreManager.Model
{
    public class BaseModel
    {
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        [NotMapped]
        public ModelState State { get; set; }

        /// <summary>
        /// Hàm thực hiện lấy khóa chính
        /// </summary>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public PropertyInfo GetPrimaryKey()
        {
            return this.GetType().GetField(typeof(KeyAttribute));
        }

        /// <summary>
        /// Hàm thực hiện lấy tên khóa chính
        /// </summary>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public string GetPrimaryKeyName()
        {
            return this.GetType().GetFieldName(typeof(KeyAttribute));
        }

        /// <summary>
        /// Hàm thực hiện lấy loại dữ liệu khóa chính
        /// </summary>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public Type GetPrimaryKeyType()
        {
            return this.GetType().GetFieldType(typeof(KeyAttribute));
        }

        /// <summary>
        /// Hàm thực hiện lấy loại dữ liệu khóa chính
        /// </summary>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public object GetPrimaryKeyValue()
        {
            return this.GetFieldValue(typeof(KeyAttribute));
        }

        /// <summary>
        /// Hàm thực hiện lấy tên table trong db của model
        /// </summary>
        /// <returns></returns>
        /// created by vdthang 19.11.2021
        public string GetTableName()
        {
            var config = (ConfigTableAttribute)this.GetType().GetCustomAttributes(typeof(ConfigTableAttribute), false).FirstOrDefault();
            return config?.TableName;
        }
    }
}
