using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TT.StoreManager.Model
{
    public static class TConvert
    {
        /// <summary>
        /// Hàm thực hiện parse object sang string json
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ignoreNull"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public static string Serialize(object source, bool ignoreNull = false)
        {
            var setting = GetSetting();

            if (ignoreNull)
                setting.NullValueHandling = NullValueHandling.Ignore;

            return JsonConvert.SerializeObject(source, setting);
        }

        /// <summary>
        /// Hàm thực hiện parse từ string json sang object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public static T Deserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, GetSetting());
            }
            catch (Exception ex)
            {
                if (typeof(T) == typeof(string))
                    return (T)((object)json);
                throw ex;
            }
        }

        /// <summary>
        /// Hàm thực hiện map object động sang 1 type định nghĩa cố định
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public static object DeserializeObj(object source, Type type, bool ignoreNull = false)
        {
            try
            {
                return JsonConvert.DeserializeObject(Serialize(source, ignoreNull), type, GetSetting());
            }
            catch (Exception ex)
            {
                if (type == typeof(string))
                    return Serialize(source, ignoreNull);
                throw ex;
            }
        }

        /// <summary>
        /// Hàm thực hiện lấy cấu hình setting jsonconvert
        /// </summary>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public static JsonSerializerSettings GetSetting()
        {
            return new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK",
                NullValueHandling = NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                
            };
        }
    }
}
