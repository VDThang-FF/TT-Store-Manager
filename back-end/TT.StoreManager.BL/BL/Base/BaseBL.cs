﻿using Microsoft.EntityFrameworkCore.Storage;
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
            var resValidate = ValidateRequest(data, modelType);
            if (!resValidate.Success)
                return resValidate;

            BeforeSave(ref data);

            IDbContextTransaction transaction = null;
            try
            {
                transaction = Context.Database.BeginTransaction();

                // Trước khi commit dữ liệu
                var resBeforeCM = BeforeCommit(data, modelType, transaction);
                if (!resBeforeCM.Success)
                {
                    transaction.Rollback();
                    return resBeforeCM;
                }

                // Bắt đầu commit dữ liệu
                var table = Context.Set<T>();
                table.Add(data as T);
                var effect = await Context.SaveChangesAsync();
                if (effect <= 0)
                {
                    transaction.Rollback();
                    return res.OnError(Code.ErrorCRUD, userMessage: "Thêm thất bại", devMessage: "Lỗi effect save change");
                }

                // Sau khi commit dữ liệu
                var resAfterCM = AfterCommit(data, modelType, data.GetPrimaryKeyValue());
                if (!resAfterCM.Success)
                {
                    transaction.Rollback();
                    return resAfterCM;
                }

                transaction.Commit();
                res.OnSuccess(data.GetPrimaryKeyValue());
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
            }
            finally
            {
                if (transaction != null)
                    transaction.Dispose();
            }

            return res;
        }

        /// <summary>
        /// Hàm thực hiện cập nhật dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        /// created by vdthang 18.11.2021
        public async Task<BaseResponse> Update<T>(BaseModel data, Type modelType) where T : class
        {
            var res = new BaseResponse();
            var resValidate = ValidateRequest(data, modelType);
            if (!resValidate.Success)
                return resValidate;

            BeforeSave(ref data);

            IDbContextTransaction transaction = null;
            try
            {
                transaction = Context.Database.BeginTransaction();

                // Trước khi commit dữ liệu
                var resBeforeCM = BeforeCommit(data, modelType, transaction);
                if (!resBeforeCM.Success)
                {
                    transaction.Rollback();
                    return resBeforeCM;
                }

                // Bắt đầu commit dữ liệu
                var table = Context.Set<T>();
                table.Update(data as T);
                var effect = await Context.SaveChangesAsync();
                if (effect < 0)
                {
                    transaction.Rollback();
                    return res.OnError(Code.ErrorCRUD, userMessage: "Cập nhật thất bại", devMessage: "Lỗi effect save change");
                }

                // Sau khi commit dữ liệu
                var resAfterCM = AfterCommit(data, modelType, data);
                if (!resAfterCM.Success)
                {
                    transaction.Rollback();
                    return resAfterCM;
                }

                transaction.Commit();
                res.OnSuccess(data.GetPrimaryKeyValue());
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
            }
            finally
            {
                if (transaction != null)
                    transaction.Dispose();
            }

            return res;
        }

        /// <summary>
        /// Hàm thực hiện validate request khi thực hiện CRUD
        /// </summary>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        /// created by vdthang 18.11.2021
        public BaseResponse ValidateRequest(BaseModel data, Type modelType)
        {
            var res = new BaseResponse();
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

        /// <summary>
        /// Hàm thực hiện trước khi commit dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// created by vdthang 18.11.2021
        public BaseResponse BeforeCommit(BaseModel data, Type modelType, IDbContextTransaction transaction = null)
        {
            var res = new BaseResponse();
            return res;
        }

        /// <summary>
        /// Hàm thực hiện sau khi commit dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <param name="resultCRUD"></param>
        /// <returns></returns>
        /// created by vdthang 18.11.2021
        public BaseResponse AfterCommit(BaseModel data, Type modelType, object resultCRUD = null)
        {
            var res = new BaseResponse();
            return res;
        }
    }
}
