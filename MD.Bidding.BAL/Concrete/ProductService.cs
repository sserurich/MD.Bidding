using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.DTO;
using MD.Bidding.BAL.Interfaces;
using MD.Bidding.DAL.Interfaces;
using MD.Bidding.Models;
using System.Configuration;
using MD.Bidding.Helpers;


namespace MD.Bidding.BAL.Concrete
{
    public class ProductService : IProductService
    {
         private IProductDataService _dataService;
         private IMediaService _mediaService;

         private static string senderId = ConfigurationManager.AppSettings["SenderId"];
         private static string apiKey = ConfigurationManager.AppSettings["ApiKey"];

         private string baseUrl = "http://api.[sms].com/api/v1/sms/send/?apiKey=" + apiKey;
         private string sellerSmsNotificationMessage = ConfigurationManager.AppSettings["SellerSmsNotificationMessage"];

        public ProductService(IProductDataService dataService,IMediaService mediaService)
        {
            this._dataService = dataService;
            this._mediaService = mediaService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Product> GenerateCurrentMonthProductReport()
        {
            return GetAllClosedProductsForAuctioning()
                .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GenerateTodaysProductReport()
        {
            return GetAllClosedProductsForAuctioning()
                .Where(p => p.BiddingPeriodEndsOn.Day == DateTime.Now.Day 
                    && p.BiddingPeriodEndsOn.Month == DateTime.Now.Month 
                    && p.BiddingPeriodEndsOn.Year == DateTime.Now.Year);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GenerateYesterdayProductReport()
        {
            DateTime currentDate = DateTime.Today.AddDays(-1);
            return GetAllClosedProductsForAuctioning()
                .Where(p => p.BiddingPeriodEndsOn.Day == currentDate.Day 
                    && p.BiddingPeriodEndsOn.Month == DateTime.Now.Month 
                    && p.BiddingPeriodEndsOn.Year == DateTime.Now.Year);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GenerateCurrentWeekReport()
        {
            DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
            DateTime endDate = DateTime.Now;
            return GetAllClosedProductsForAuctioning()
                   .Where(p => p.BiddingPeriodEndsOn >= startOfWeek 
                    && p.BiddingPeriodEndsOn <= endDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GenerateReportForProductsSoldInPastSevenDays()
        {
            DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
            DateTime endDate = DateTime.Now;
            return GetAllClosedProductsForAuctioning()
                   .Where(p => p.BiddingPeriodEndsOn >= startOfWeek
                    && p.BiddingPeriodEndsOn <= endDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public long GetMediaFolderId(int productId)
        {
            var result = this._dataService.GetProduct(productId);
            if (result != null)
            {
               return Convert.ToInt64(result.MediaFolderId);
            }
            return 0;
        }

        public IEnumerable<Models.Product> GenerateSoldProductsReport(ProductSearch searchData)
        {
            IEnumerable<Models.Product>productsFound = new List<Models.Product>();
            if (searchData.FromDate > searchData.ToDate)
            {
                searchData.ToDate = null;
            }
            if (searchData.FromDate > DateTime.Now)
            {
                searchData.FromDate = DateTime.Now;
            }

            DateTime actualToDate = DateTime.Now;
            if (searchData.ToDate.HasValue)
            {
                DateTime endDate = searchData.ToDate.Value;
                actualToDate = endDate.AddDays(1);
            }


            if (searchData.FromDate != null && searchData.ToDate != null)
            {
               productsFound = GetAllProductsSoldBetweenTheSpecifiedDates(Convert.ToDateTime(searchData.FromDate), Convert.ToDateTime(actualToDate));
            }


            return productsFound;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lowerSpecifiedDate"></param>
        /// <param name="upperSpecifiedDate"></param>
        /// <returns></returns>
        public IEnumerable<Models.Product> GetAllProductsSoldBetweenTheSpecifiedDates
            (DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate)
        {
            var results = GetAllClosedProductsForAuctioning()
               .Where(m => m.BiddingPeriodEndsOn >= lowerSpecifiedDate 
                 && m.BiddingPeriodEndsOn <= upperSpecifiedDate);
            return results;
        }

        /// <summary>
        /// Gets for products randomly
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Product> GetFeaturedProducts()
        {
            var featuredProducts = new List<Product>();
            for (int i = 0; i <= 20; i++)
            {
                var pdt = GetRandomProduct();
                featuredProducts.Add(pdt);
            }

            return featuredProducts.GroupBy(p =>p.ProductId).Select(p =>p.First()).ToList().Take(4);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Product GetRandomProduct()
        {
            var results = _dataService.GetAllProducts().ToList();

            int count = results.Count();
            int index = new Random().Next(count);
            return MapEFToModel(results.Skip(index).FirstOrDefault());
        }

        /// <summary>
        /// Gets the top 6 latest products.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Product> GetLatestProducts()
        {
            var results = _dataService.GetAllProducts().OrderByDescending(p => p.CreatedOn).Take(6);
            return MapEFToModel(results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Product> GetAllOpenProductsForAuctioning()
        {
            var results = _dataService.GetAllProducts()
                .Where(p => p.BiddingPeriodEndsOn > DateTime.Now)
                .OrderByDescending(p => p.CreatedOn);
            return MapEFToModel(results);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateBidStatus()
        {
            List<Bid> closeOff = new List<Bid>();
            var products = GetAllClosedProductsForAuctioning();
            if (products != null)
            {
                if (products.Any())
                {
                    foreach (var product in products)
                    {
                        var productBids = _dataService.GetAllProductBids(product.ProductId)
                            .Where(p => p.Status != (int)MD.Bidding.Library.EnumTypes.Status.Closed);
                        if (productBids != null)
                        {
                            foreach (var productBid in productBids)
                            {
                                closeOff.Add(  new Bid() { 
                                    BidId = productBid.BidId, 
                                    Status = (int)MD.Bidding.Library.EnumTypes.Status.Closed,
                                    SellerPhoneNumber=product.SellerPhoneNumber,
                                    SellerEmail = product.SellerEmail
                                });
                            }
                        }
                    }
                }
            }

            if (closeOff.Count() > 0)
            {
                foreach (var productBid in closeOff)
                {
                    _dataService.UpdateBidStatus(productBid.BidId, (int)MD.Bidding.Library.EnumTypes.Status.Closed);
                    SendProductSellerEmailNotificationWhenBiddingEnds(sellerSmsNotificationMessage, productBid.SellerEmail);
                    SendProductSellerSMSNotificationWhenBiddingEnds(baseUrl, productBid.SellerPhoneNumber, sellerSmsNotificationMessage, senderId);
                }
                
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiBaseUrl"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="messageBody"></param>
        /// <param name="senderId"></param>
        public void SendProductSellerSMSNotificationWhenBiddingEnds(string apiBaseUrl, string phoneNumber, string messageBody, string senderId)
        {
            SmsHelper.SendSmsToOneContact(apiBaseUrl, phoneNumber, messageBody, senderId);
        }

        public void SendProductSellerEmailNotificationWhenBiddingEnds(string body, string emailAddress)
        {
            Helpers.Email email = new Helpers.Email();
            email.MailBodyHtml = body;
            email.MailToAddress = emailAddress;
            email.MailFromAddress = ConfigurationManager.AppSettings["EmailAddressFrom"];
            email.Subject = "BIDDING FOR YOUR PRODUCT HAS ENDED";


            try
            {
                email.SendMail();              
            }
            catch (Exception ex)
            {
                              
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Product> GetAllClosedProductsForAuctioning()
        {
            var results = _dataService.GetAllProducts()
                .Where(p => p.BiddingPeriodEndsOn <= DateTime.Now)
                .OrderByDescending(p => p.CreatedOn);
            return MapEFToModel(results);
        }

        public Product GetProduct(int productId)
        {
            var result = this._dataService.GetProduct(productId);
            if (result != null)
                return MapEFToModel(result);

            return null;
        }

        public IEnumerable<Models.Product> GetAllProducts()
        {
            var results = _dataService.GetAllProducts();
            return MapEFToModel(results);
        }

        public IEnumerable<Models.Product> GetAllUserProducts(string userId)
        {
            var results = _dataService.GetAllProducts().Where(u => u.CreatedBy==userId);
            return MapEFToModel(results);
        }


        public int SaveAsDraft(Product product, string userId)
        {
            var c = new DTO.ProductDTO()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                MinimumPrice = product.MinimumPrice,
                BiddingPeriod = product.BiddingPeriod,
                BiddingPeriodEndsOn= product.BiddingPeriodEndsOn,
                CategoryId = product.CategoryId,
                CreatedBy = userId,
                UpdatedBy = userId,
            };

            return this._dataService.SaveAsDraft(c, userId);
        }


        public void MarkAsDeleted(int ProductId, string userId)
        {
            _dataService.MarkAsDeleted(ProductId, userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public decimal GetProductSoldPrice(int productId)
        {
            decimal priceSold = 0;
            var bidWithHighestPrice = this.GetAllProductBids(productId)
                .OrderByDescending(b => b.Amount).FirstOrDefault();
            if (bidWithHighestPrice != null)
            {
                priceSold = bidWithHighestPrice.Amount;
            }
            return priceSold;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int SaveProductBid(Bid bid, string userId)
        {
            var c = new DTO.BidDTO()
            {
                ProductId = bid.ProductId,
                Amount = bid.Amount,               
                Status = bid.Status              
            };
            return this._dataService.SaveProductBid(c, userId);
        }

        public IEnumerable<Bid> GenerateProductBidsReport(ProductSearch searchData)
        {
            IEnumerable<Bid> bidsFound = new List<Bid>();
            if (searchData.FromDate > searchData.ToDate)
            {
                searchData.ToDate = null;
            }
            if (searchData.FromDate > DateTime.Now)
            {
                searchData.FromDate = DateTime.Now;
            }

            DateTime actualToDate = DateTime.Now;
            if (searchData.ToDate.HasValue)
            {
                DateTime endDate = searchData.ToDate.Value;
                actualToDate = endDate.AddDays(1);
            }

            if (searchData.FromDate != null && searchData.ToDate != null)
            {
                bidsFound = GetAllProductsBidsSoldBetweenTheSpecifiedDates(Convert.ToDateTime(searchData.FromDate), Convert.ToDateTime(actualToDate));
            }

            return bidsFound;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lowerSpecifiedDate"></param>
        /// <param name="upperSpecifiedDate"></param>
        /// <returns></returns>
        public IEnumerable<Models.Bid> GetAllProductsBidsSoldBetweenTheSpecifiedDates
            (DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate)
        {
            var results = _dataService.GetAllBids()
               .Where(m => m.CreatedOn >= lowerSpecifiedDate
                 && m.CreatedOn <= upperSpecifiedDate);
            return MapBidEFToModel(results);
        }

        public IEnumerable<Bid> GenerateCurrentWeekProductBidsReport()
        {
            DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
            DateTime endDate = DateTime.Now;
            var results = _dataService.GetAllBids()
                   .Where(p => p.CreatedOn >= startOfWeek
                    && p.CreatedOn <= endDate);
            return MapBidEFToModel(results);

        }

        public IEnumerable<Bid> GenerateCurrentMonthProductBidsReport()
        {
            var results = _dataService.GetAllBids()
                .Where(p => p.CreatedOn.Month == DateTime.Now.Month 
                    && p.CreatedOn.Year == DateTime.Now.Year);
            return MapBidEFToModel(results);
        }

        public IEnumerable<Bid> GenerateYesterdayProductBidsReport()
        {
            DateTime currentDate = DateTime.Today.AddDays(-1);
            var results = _dataService.GetAllBids()
                .Where(p => p.CreatedOn.Day == currentDate.Day
                    && p.CreatedOn.Month == DateTime.Now.Month
                    && p.CreatedOn.Year == DateTime.Now.Year);

            return MapBidEFToModel(results);
        }

        public IEnumerable<Bid> GenerateTodaysProductBidsReport()
        {
            var results = _dataService.GetAllBids()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day
                   && p.CreatedOn.Month == DateTime.Now.Month
                   && p.CreatedOn.Year == DateTime.Now.Year);
            return MapBidEFToModel(results);
        }

        public IEnumerable<Bid> GenerateReportForProductBidsSoldInPastSevenDays()
        {
            DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
            DateTime endDate = DateTime.Now;
            var results = _dataService.GetAllBids()
                   .Where(p => p.CreatedOn >= startOfWeek
                    && p.CreatedOn <= endDate);

            return MapBidEFToModel(results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Models.Bid> GetAllBids(string userId)
        {
            var results = _dataService.GetAllBids().Where(u => u.BidderId == userId);
            return MapBidEFToModel(results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IEnumerable<Models.Bid> GetAllProductBids(int productId)
        {
            var results = _dataService.GetAllProductBids(productId);
            return MapBidEFToModel(results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bidId"></param>
        /// <returns></returns>
        public Bid GetBid(int bidId)
        {
            var result = this._dataService.GetBid(bidId);
            if (result != null)
                return MapBidEFToModel(result);
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IEnumerable<Product> MapEFToModel(IEnumerable<MD.Bidding.EF.Models.Product> data)
        {
            var list = new List<Product>();
            if (data != null)
            {
                foreach (var result in data)
                {
                    list.Add(MapEFToModel(result));
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Product MapEFToModel(EF.Models.Product data)
        {
            if (data != null)
            {
                string createdBy = string.Empty;
                string updatedBy = string.Empty;
                string categoryName = string.Empty;
                string sellerPhoneNumber = string.Empty;
                string sellerEmail = string.Empty;
                string userId = string.Empty;

                if (data.AspNetUser != null)
                {
                    createdBy = data.AspNetUser.FirstName + " " + data.AspNetUser.LastName;
                    sellerPhoneNumber = data.AspNetUser.PhoneNumber;
                    sellerEmail = data.AspNetUser.Email;
                    userId = data.CreatedBy;
                }
                if (data.AspNetUser2 != null)
                {
                    updatedBy = data.AspNetUser2.FirstName + " " + data.AspNetUser2.LastName;
                }

                if (data.Category != null)
                {
                    categoryName = data.Category.Name;
                }

                var productImages = _mediaService.GetFilesInFolder(data.MediaFolderId, "2");
                var soldPrice = this.GetProductSoldPrice(data.ProductId);

                var Product = new Product()
                  {
                      ProductId = data.ProductId,
                      Name = data.Name,
                      Description = data.Description,
                      MinimumPrice = data.MinimumPrice,
                      BiddingPeriod = data.BiddingPeriod,
                      CategoryName = categoryName,
                      MediaFolderId = data.MediaFolderId,
                      CategoryId = data.CategoryId,
                      UpdatedBy = updatedBy,
                      TimeStamp = data.TimeStamp,
                      CreatedOn = data.CreatedOn,
                      CreatedBy = createdBy,
                      BiddingPeriodEndsOn = data.BiddingPeriodEndsOn,
                      SellerPhoneNumber = sellerPhoneNumber,
                      ProductImages = productImages,
                      SellerEmail = sellerEmail,
                      AmountSold = soldPrice,
                      CreatedByUserId = userId
                  };
                return Product;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IEnumerable<Bid> MapBidEFToModel(IEnumerable<MD.Bidding.EF.Models.Bid> data)
        {
            var list = new List<Bid>();
            if (data != null)
            {
                foreach (var result in data)
                {
                    list.Add(MapBidEFToModel(result));
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Bid MapBidEFToModel(EF.Models.Bid data)
        {
            if (data != null)
            {
                string createdBy = string.Empty;
                string updatedBy = string.Empty;
                string status = string.Empty;
                string phoneNumber = string.Empty;
                string email = string.Empty;
                if (data.AspNetUser != null)
                {
                    createdBy = data.AspNetUser.FirstName + " " + data.AspNetUser.LastName;
                    phoneNumber = data.AspNetUser.PhoneNumber;                   
                }
                if (data.AspNetUser2 != null)
                {
                    updatedBy = data.AspNetUser2.FirstName + " " + data.AspNetUser2.LastName;
                }

                if (data.Status == (int)MD.Bidding.Library.EnumTypes.Status.Open)
                {
                    status = "Open For Bidding";
                }
                else if( data.Status == (int)MD.Bidding.Library.EnumTypes.Status.OnGoing)
                {
                    status = "OnGoing";
                }
                else if (data.Status == (int)MD.Bidding.Library.EnumTypes.Status.Closed)
                {
                    status = "Bidding Closed";
                }

                var Bid = new Bid()
                {
                    BidId = data.BidId,
                    Amount= data.Amount,
                    ProductId = data.ProductId,
                    Status = data.Status,
                    UpdatedBy = updatedBy,
                    TimeStamp = data.TimeStamp,
                    CreatedOn = data.CreatedOn,
                    CreatedBy = createdBy,
                    BidStatusName = status,
                    Product = data.Product.Name,
                    BidderPhoneNumber = phoneNumber,
                    SellerEmail = email
                };
                return Bid;
            }

            return null;
        }
    }
}
