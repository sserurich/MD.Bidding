using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MD.Bidding.Models;
using MD.Bidding.BAL.Interfaces;
using MD.Bidding.BAL.Concrete;
using log4net;

namespace MD.Bidding.Web.Controllers
{
   
    public class ProductApiController : ApiController
    {

        private IUserService _userService;
        private IProductService _ProductService;
        ILog logger = log4net.LogManager.GetLogger(typeof(ProductApiController));
        private string userId = string.Empty;

        public ProductApiController()
        {
        }

        public ProductApiController(IProductService ProductService, IUserService userService)
        {
            this._userService = userService;
            this._ProductService = ProductService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);

        }



        [HttpGet]
        [ActionName("GetProduct")]
        public Product GetProduct(int productId)
        {
            return _ProductService.GetProduct(productId);
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetFeaturedProducts")]
        public IEnumerable<Product> GetFeaturedProducts()
        {
            return _ProductService.GetFeaturedProducts();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetLatestProducts")]
        public IEnumerable<Product> GetLatestProducts()
        {
            return _ProductService.GetLatestProducts();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetAllProducts")]
        public IEnumerable<Product> GetAllProducts()
        {
            return _ProductService.GetAllProducts();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetAllOpenProductsForAuctioning")]
        public IEnumerable<Product> GetAllOpenProductsForAuctioning()
        {
            return _ProductService.GetAllOpenProductsForAuctioning();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetAllClosedProductsForAuctioning")]
        public IEnumerable<Product> GetAllClosedProductsForAuctioning()
        {
            return _ProductService.GetAllClosedProductsForAuctioning();
        }

        [Authorize]
        [HttpPost]
        [ActionName("GenerateSoldProductsReport")]
        public IEnumerable<Product> GenerateSoldProductsReport(ProductSearch searchData)
        {
            return _ProductService.GenerateSoldProductsReport(searchData)
                .Where(u => u.CreatedByUserId == userId);
        }

        [Authorize]
        [HttpGet]
        [System.Web.Http.ActionName("GenerateReportForProductsSoldInPastSevenDays")]
        public IEnumerable<Product> GenerateReportForProductsSoldInPastSevenDays()
        {
            return _ProductService.GenerateReportForProductsSoldInPastSevenDays()
                .Where(u => u.CreatedByUserId == userId);
        }

        [Authorize]
        [HttpGet]
        [System.Web.Http.ActionName("GenerateTodaysProductReport")]
        public IEnumerable<Product> GenerateTodaysProductReport()
        {
            return _ProductService.GenerateTodaysProductReport()
                .Where(u => u.CreatedByUserId == userId);
        }

        [Authorize]
        [HttpGet]
        [System.Web.Http.ActionName("GenerateYesterdayProductReport")]
        public IEnumerable<Product> GenerateYesterdayProductReport()
        {
            return _ProductService.GenerateYesterdayProductReport()
                .Where(u => u.CreatedByUserId == userId);
        }

        [Authorize]
        [HttpGet]
        [System.Web.Http.ActionName("GenerateCurrentWeekReport")]
        public IEnumerable<Product> GenerateCurrentWeekReport()
        {
            return _ProductService.GenerateCurrentWeekReport()
                .Where(u => u.CreatedByUserId == userId);
        }

        [Authorize]
        [HttpGet]
        [System.Web.Http.ActionName("GenerateCurrentMonthProductReport")]
        public IEnumerable<Product> GenerateCurrentMonthProductReport()
        {
            return _ProductService.GenerateCurrentMonthProductReport()
                .Where(u => u.CreatedByUserId == userId);
        }

         [Authorize]
        [HttpGet]
        [System.Web.Http.ActionName("GetAllUserProducts")]
        public IEnumerable<Product> GetAllUserProducts()
        {
            return _ProductService.GetAllUserProducts(userId);
        }

        [Authorize]
        [HttpGet]
        [ActionName("DeleteProduct")]
        public void DeleteProduct(int ProductId)
        {
            _ProductService.MarkAsDeleted(ProductId, userId);
        }
        [Authorize]
        [HttpPost]
        [ActionName("SaveProduct")]
        public int SaveProduct(Product model)
        {
            var ProductId = _ProductService.SaveAsDraft(model, userId);
            return ProductId;
        }

        [Authorize]
        [HttpGet]
        [System.Web.Http.ActionName("GetAllBids")]
        public IEnumerable<Bid> GetAllBids()
        {
            return _ProductService.GetAllBids(userId);
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetAllProductBids")]
        public IEnumerable<Bid> GetAllProductBids(int productId)
        {
            return _ProductService.GetAllProductBids(productId);
        }

        [HttpGet]
        [ActionName("GetBid")]
        public Bid GetBid(int bidId)
        {
            return _ProductService.GetBid(bidId);
        }
         [Authorize]
        [HttpPost]
        [ActionName("SaveProductBid")]
        public int SaveProductBid(Bid model)
        {
            var bidId = _ProductService.SaveProductBid(model, userId);
            return bidId;
        }

        

        [HttpGet]
        [ActionName("GetUpdateBidStatus")]
         public void GetUpdateBidStatus()
        {
            _ProductService.UpdateBidStatus();
        }
    }
}
