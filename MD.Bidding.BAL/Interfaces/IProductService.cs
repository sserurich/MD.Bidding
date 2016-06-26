using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.Models;

namespace MD.Bidding.BAL.Interfaces
{
    public interface IProductService
    {
        Product GetProduct(int productId);
        IEnumerable<Product> GetAllProducts();
        int SaveAsDraft(Product productDTO, string userId);
        void MarkAsDeleted(int productId, string userId);
        long GetMediaFolderId(int draftProductId);
        int SaveProductBid(Bid bid, string userId);
        IEnumerable<Bid> GetAllProductBids(int productId);
        IEnumerable<Bid> GetAllBids(string userId);
        Bid GetBid(int bidId);
        IEnumerable<Product> MapEFToModel(IEnumerable<MD.Bidding.EF.Models.Product> data);
        IEnumerable<Models.Product> GetAllUserProducts(string userId);
        IEnumerable<Models.Product> GetLatestProducts();
        IEnumerable<Models.Product> GetFeaturedProducts();
        IEnumerable<Models.Product> GetAllClosedProductsForAuctioning();
        IEnumerable<Models.Product> GetAllOpenProductsForAuctioning();
        IEnumerable<Models.Product> GetAllProductsSoldBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate);
        void UpdateBidStatus();

        IEnumerable<Product> GenerateSoldProductsReport(ProductSearch searchData);

        IEnumerable<Product> GenerateReportForProductsSoldInPastSevenDays();

        IEnumerable<Product> GenerateTodaysProductReport();

        IEnumerable<Product> GenerateYesterdayProductReport();

        IEnumerable<Product> GenerateCurrentWeekReport();

        IEnumerable<Product> GenerateCurrentMonthProductReport();

        IEnumerable<Bid> GenerateProductBidsReport(ProductSearch searchData);

        IEnumerable<Bid> GenerateCurrentWeekProductBidsReport();

        IEnumerable<Bid> GenerateCurrentMonthProductBidsReport();

        IEnumerable<Bid> GenerateYesterdayProductBidsReport();

        IEnumerable<Bid> GenerateTodaysProductBidsReport();

        IEnumerable<Bid> GenerateReportForProductBidsSoldInPastSevenDays();
    }
}
