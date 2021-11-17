using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT.StoreManager.BL;
using TT.StoreManager.Model;

namespace TT.StoreManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BasesController<Product>
    {
        public ProductsController(IBaseBL baseBL, IProductBL productBL) : base(baseBL)
        {
            BL = productBL;
            CurrentModelType = typeof(Product);
        }
    }
}
