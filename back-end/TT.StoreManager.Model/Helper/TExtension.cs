using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TT.StoreManager.Model
{
    public static class TExtension
    {
        /// <summary>
        /// Hàm thực hiện lấy property qua attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public static PropertyInfo GetField(this Type type, Type attribute)
        {
            var props = type.GetProperties();
            var find = props?.FirstOrDefault(p => p.GetCustomAttributes(attribute, true) != null);
            if (find != null)
                return find;

            return null;
        }

        /// <summary>
        /// Hàm thực hiện lấy tên property qua attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public static string GetFieldName(this Type type, Type attribute)
        {
            var props = type.GetProperties();
            var find = props?.FirstOrDefault(p => p.GetCustomAttributes(attribute, true) != null);
            if (find != null)
                return find.Name;

            return null;
        }

        /// <summary>
        /// Hàm thực hiện lấy loại property qua attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public static Type GetFieldType(this Type type, Type attribute)
        {
            var props = type.GetProperties();
            var find = props?.FirstOrDefault(p => p.GetCustomAttributes(attribute, true) != null);
            if (find != null)
                return find.PropertyType;

            return null;
        }

        /// <summary>
        /// Hàm thực hiện lấy giá trị property qua attribute
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public static object GetFieldValue(this BaseModel model, Type attribute)
        {
            var props = model.GetType().GetProperties();
            var find = props?.FirstOrDefault(p => p.GetCustomAttributes(attribute, true) != null);
            if (find != null)
                return find.GetValue(model);

            return null;
        }

        /// <summary>
        /// Hàm thực hiện set giá trị cho property
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// created by vdthang 17.11.2021
        public static void SetValue(this object source, string propertyName, object value)
        {
            var prop = source.GetType().GetProperty(propertyName, BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (prop != null)
            {
                var type = prop.PropertyType;
                if (value != DBNull.Value && prop.CanWrite)
                {
                    if (value != null)
                        prop.SetValue(source, Convert.ChangeType(value, Nullable.GetUnderlyingType(type) ?? type), null);
                    else
                        prop.SetValue(source, null, null);
                }
            }
        }

        /// <summary>
        /// Hàm thực hiện set giá trị khóa chính
        /// </summary>
        /// <param name="model"></param>
        /// created by vdthang 17.11.2021
        public static void SetUniqePrimaryKey(this BaseModel model)
        {
            var propInfo = model.GetPrimaryKey();
            if (propInfo.PropertyType == typeof(Guid))
            {
                if (propInfo.GetValue(model) == null || (Guid)propInfo.GetValue(model) == Guid.Empty)
                    model.SetValue(propInfo.Name, Guid.NewGuid());
            }
        }
    }
}
