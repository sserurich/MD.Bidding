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

namespace MD.Bidding.Admin.Controllers
{
    [Authorize]
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

        [HttpPost]
        [ActionName("GenerateProductBidsReport")]
        public IEnumerable<Bid> GenerateProductBidsReport(ProductSearch searchData)
        {
            return _ProductService.GenerateProductBidsReport(searchData);
        }

        [HttpGet]
        [System.Web.Http.ActionName("GenerateReportForProductBidsSoldInPastSevenDays")]
        public IEnumerable<Bid> GenerateReportForProductBidsSoldInPastSevenDays()
        {
            return _ProductService.GenerateReportForProductBidsSoldInPastSevenDays();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GenerateTodaysProductBidsReport")]
        public IEnumerable<Bid> GenerateTodaysProductBidsReport()
        {
            return _ProductService.GenerateTodaysProductBidsReport();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GenerateYesterdayProductBidsReport")]
        public IEnumerable<Bid> GenerateYesterdayProductBidsReport()
        {
            return _ProductService.GenerateYesterdayProductBidsReport();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GenerateCurrentWeekProductBidsReport")]
        public IEnumerable<Bid> GenerateCurrentWeekProductBidsReport()
        {
            return _ProductService.GenerateCurrentWeekProductBidsReport();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GenerateCurrentMonthProductBidsReport")]
        public IEnumerable<Bid> GenerateCurrentMonthProductBidsReport()
        {
            return _ProductService.GenerateCurrentMonthProductBidsReport();
        }


        [HttpPost]
        [ActionName("GenerateSoldProductsReport")]
        public IEnumerable<Product> GenerateSoldProductsReport(ProductSearch searchData)
        {
            return _ProductService.GenerateSoldProductsReport(searchData);
        }

        [HttpGet]
        [System.Web.Http.ActionName("GenerateReportForProductsSoldInPastSevenDays")]
        public IEnumerable<Product> GenerateReportForProductsSoldInPastSevenDays()
        {
            return _ProductService.GenerateReportForProductsSoldInPastSevenDays();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GenerateTodaysProductReport")]
        public IEnumerable<Product> GenerateTodaysProductReport()
        {
            return _ProductService.GenerateTodaysProductReport();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GenerateYesterdayProductReport")]
        public IEnumerable<Product> GenerateYesterdayProductReport()
        {
            return _ProductService.GenerateYesterdayProductReport();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GenerateCurrentWeekReport")]
        public IEnumerable<Product> GenerateCurrentWeekReport()
        {
            return _ProductService.GenerateCurrentWeekReport();
        }

        [HttpGet]
        [System.Web.Http.ActionName("GenerateCurrentMonthProductReport")]
        public IEnumerable<Product> GenerateCurrentMonthProductReport()
        {
            return _ProductService.GenerateCurrentMonthProductReport();
        }


        [HttpGet]
        [System.Web.Http.ActionName("GetAllProducts")]
        public IEnumerable<Product> GetAllProducts()
        {
            return _ProductService.GetAllProducts();
        }

        [HttpGet]
        [ActionName("DeleteProduct")]
        public void DeleteProduct(int ProductId)
        {
            _ProductService.MarkAsDeleted(ProductId, userId);
        }


        [HttpPost]
        [ActionName("SaveProduct")]
        public int SaveProduct(Product model)
        {
            var ProductId = _ProductService.SaveAsDraft(model, userId);
            return ProductId;
        }

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

        [HttpPost]
        [ActionName("SaveProductBid")]
        public int SaveProductBid(Bid model)
        {
            var bidId = _ProductService.SaveProductBid(model, userId);
            return bidId;
        }
    }
}
