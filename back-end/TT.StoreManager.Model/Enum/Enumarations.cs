using System;
using System.Collections.Generic;
using System.Text;

namespace TT.StoreManager.Model
{
    public static class Enumarations
    {
        /// <summary>
        /// Mã Code response
        /// </summary>
        public enum Code : int
        {
            Success = 0,
            ErrorCRUD = 1,
            Exception = 1000,
        }

        /// <summary>
        /// Mã state trạng thái model
        /// </summary>
        public enum ModelState : int
        {
            Insert = 0,
            Update = 1,
            Delete = 2,
            Duplicate = 3
        }
    }
}
