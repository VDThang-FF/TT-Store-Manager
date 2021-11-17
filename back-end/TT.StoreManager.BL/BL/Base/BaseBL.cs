using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TT.StoreManager.Model;
using static TT.StoreManager.Model.Enumarations;

namespace TT.StoreManager.BL
{
    public class BaseBL : IBaseBL
    {
        private Context _context;
        public Context Context
        {
            get
            {
                if (_context == null)
                    throw new Exception("Context kết nối CSDL bị null");
                return _context;
            }
        }

        public BaseBL()
        {
            _context = new Context();
        }

        /// <summary>
        /// Hàm thực hiện thêm mới dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public async Task<BaseResponse> Insert<T>(BaseModel data, Type modelType) where T : class
        {
            var res = new BaseResponse();
            var table = Context.Set<T>();
            BeforeSave(ref data);
            table.Add(data as T);
            var effect = await Context.SaveChangesAsync();
            if (effect <= 0)
                return res.OnError(Code.ErrorCRUD, userMessage: "Thêm thất bại", devMessage: "Lỗi effect save change");

            return res;
        }

        /// <summary>
        /// Hàm thực hiện cấu hình thêm 1 vài giá trị property trước khi CRUD
        /// </summary>
        /// <param name="model"></param>
        /// created by vdthang 17.11.2021
        public void BeforeSave(ref BaseModel model)
        {
            if (model.State == ModelState.Insert || model.State == ModelState.Duplicate)
            {
                model.CreatedBy = "System";
                model.CreatedDate = DateTime.Now;
                model.SetUniqePrimaryKey();
            }
            else if (model.State == ModelState.Update)
            {
                model.ModifiedBy = "System";
                model.ModifiedDate = DateTime.Now;
            }
        }
    }
}
