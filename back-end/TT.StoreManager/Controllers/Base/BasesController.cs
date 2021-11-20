using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT.StoreManager.BL;
using TT.StoreManager.Model;
using static TT.StoreManager.Model.Enumarations;

namespace TT.StoreManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase where T : class
    {
        private IBaseBL _BL;
        protected IBaseBL BL
        {
            get
            {
                if (_BL == null)
                    throw new NotImplementedException("Chưa thực hiện gán BL cho controller");
                return _BL;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException("Giá trị gắn BL cho controller bị null");
                _BL = value;
            }
        }

        private Type _CurrentModelType;
        protected Type CurrentModelType
        {
            get
            {
                if (_CurrentModelType == null)
                    throw new NotImplementedException("Chưa thực hiện gán ModelType cho controller");
                return _CurrentModelType;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException("Giá trị gắn ModelType cho controller bị null");
                _CurrentModelType = value;
            }
        }

        public BasesController(IBaseBL baseBL)
        {
            this.BL = baseBL;
            this.CurrentModelType = typeof(BaseModel);
        }

        /// <summary>
        /// API thực hiện thêm mới 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        [HttpPost("")]
        public async Task<BaseResponse> Insert([FromBody] T request)
        {
            var res = new BaseResponse();

            try
            {
                var model = (BaseModel)TConvert.DeserializeObj(request, CurrentModelType);
                model.State = Enumarations.ModelState.Insert;
                res = await BL.Insert<T>(model, CurrentModelType);
            }
            catch (Exception ex)
            {
                res.OnError(Code.Exception, devMessage: ex.Message, exception: ex);
            }

            return res;
        }

        /// <summary>
        /// API thực hiện cập nhật 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        [HttpPut("")]
        public async Task<BaseResponse> Update([FromBody] T request)
        {
            var res = new BaseResponse();

            try
            {
                var model = (BaseModel)TConvert.DeserializeObj(request, CurrentModelType);
                model.State = Enumarations.ModelState.Insert;
                res = await BL.Update<T>(model, CurrentModelType);
            }
            catch (Exception ex)
            {
                res.OnError(Code.Exception, devMessage: ex.Message, exception: ex);
            }

            return res;
        }

        /// <summary>
        /// API thực hiện lấy dữ liệu phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// created by vdthang 19.11.2021
        [HttpPost("Paging")]
        public async Task<BaseResponse> GetPaging([FromBody] PagingRequest request)
        {
            var res = new BaseResponse();

            try
            {
                res = await BL.GetPaging<T>(request, CurrentModelType);
            }
            catch (Exception ex)
            {
                res.OnError(Code.Exception, devMessage: ex.Message, exception: ex);
            }

            return res;
        }
    }
}
