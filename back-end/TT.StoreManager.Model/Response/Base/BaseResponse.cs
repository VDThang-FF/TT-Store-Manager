using System;
using System.Collections.Generic;
using System.Text;
using static TT.StoreManager.Model.Enumarations;

namespace TT.StoreManager.Model
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public Code Code { get; set; }
        public object Data { get; set; }
        public string UserMessage { get; set; }
        public string DevMessage { get; set; }

        public BaseResponse(bool success = true, Code code = Code.Success, object data = null, string userMessage = "Thành công!", string devMessage = "Done!")
        {
            Success = success;
            Code = code;
            Data = data;
            UserMessage = userMessage;
            DevMessage = devMessage;
        }

        /// <summary>
        /// Hàm thực hiện bắn lỗi response
        /// </summary>
        /// <param name="res"></param>
        /// created by vdthang 17.11.2021
        public BaseResponse OnError(Code code, string userMessage = "Hệ thống xảy ra lỗi!", string devMessage = "Exception!", Exception exception = null)
        {
            this.Code = code;
            this.UserMessage = userMessage;
            this.DevMessage = devMessage;
            this.Data = null;
            this.Success = false;
            return this;
        }

        /// <summary>
        /// Hàm thực hiện bắn thành công response
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <param name="userMessage"></param>
        /// <param name="devMessae"></param>
        /// created by vdthang 17.11.2021
        public BaseResponse OnSuccess(object data, string userMessage = "Thành công!", string devMessae = "Done!")
        {
            this.Code = Code.Success;
            this.Data = data;
            this.UserMessage = userMessage;
            this.DevMessage = devMessae;
            this.Success = true;
            return this;
        }
    }
}
