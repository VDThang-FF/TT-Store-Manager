using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TT.StoreManager.Model;

namespace TT.StoreManager.BL
{
    public interface IBaseBL
    {
        /// <summary>
        /// Interface thực hiện thêm mới dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        Task<BaseResponse> Insert<T>(BaseModel data, Type modelType) where T : class;
    }
}
