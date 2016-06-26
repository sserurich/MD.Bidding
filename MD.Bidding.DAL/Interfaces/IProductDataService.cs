using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.DTO;
using MD.Bidding.EF.Models;


namespace MD.Bidding.DAL.Interfaces
{
    public interface IProductDataService
    {
        Product GetProduct(int productId);
        IEnumerable<Product> GetAllProducts();
        int SaveAsDraft(ProductDTO productDTO, string userId);
        void MarkAsDeleted(int productId, string userId);

        int SaveProductBid(BidDTO bidDTO, string userId);
        IEnumerable<Bid> GetAllProductBids(int productId);
        IEnumerable<Bid> GetAllBids();
        Bid GetBid(int bidId);

        void UpdateBidStatus(int bidId,int status);
    }
}
