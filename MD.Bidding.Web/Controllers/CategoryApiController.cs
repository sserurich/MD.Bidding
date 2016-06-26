using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using MD.Bidding.Models;
using MD.Bidding.BAL.Interfaces;
using MD.Bidding.BAL.Concrete;
using log4net;

namespace MD.Bidding.Web.Controllers
{
   
    public class CategoryApiController : ApiController
    {

        private IUserService _userService;
        private ICategoryService _categoryService;
        ILog logger = log4net.LogManager.GetLogger(typeof(CategoryApiController));
        private string userId = string.Empty;

        public CategoryApiController()
        {
        }

        public CategoryApiController(ICategoryService categoryService, IUserService userService)
        {
            this._userService = userService;
            this._categoryService = categoryService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
           
        }

        [HttpGet]
        [ActionName("GetCategory")]
        public Category GetCategory(int categoryId)
        {
            return _categoryService.GetCategory(categoryId);
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetAllCategories")]
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryService.GetAllCategories();
        }
         [Authorize]
        [HttpGet]
        [ActionName("DeleteCategory")]
        public void DeleteCategory(int categoryId)
        {
            _categoryService.MarkAsDeleted(categoryId, userId);
        }
         [Authorize]
        [HttpPost]
        [ActionName("SaveCategory")]
        public int SaveCategory(Category model)
        {
            var categoryId = _categoryService.SaveAsDraft(model, userId);
            return categoryId;
        }

       
        [HttpGet]
        [System.Web.Http.ActionName("GetAllMainCategories")]
        public IEnumerable<Category> GetAllMainCategories()
        {
            return _categoryService.GetAllMainCategories();
        }
    }
}
