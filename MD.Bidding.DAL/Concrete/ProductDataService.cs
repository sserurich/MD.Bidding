using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.DAL.Interfaces;
using MD.Bidding.DTO;
using MD.Bidding.EF.UnitOfWork;
using MD.Bidding.EF.Models;

namespace MD.Bidding.DAL.Concrete
{
    public class ProductDataService : DataServiceBase, IProductDataService
    {
        public ProductDataService(IUnitOfWork<BiddingEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

        /// <summary>
        /// Gets Product with the specified ProductId, and 
        /// returns that Product if found. 
        /// </summary>
        /// <param name="ProductId">ProductId of the Product to get.</param>
        /// <returns>Product with the specified ProductId.</returns>
        public Product GetProduct(int ProductId)
        {
            return this.UnitOfWork.Get<Product>().AsQueryable() 
                .FirstOrDefault(m =>
                    m.ProductId == ProductId &&
                    m.Deleted == false
                );
        }


        public IEnumerable<Product> GetAllProducts()
        {
            return this.UnitOfWork.Get<Product>().AsQueryable()
                .Where(m => m.Deleted == false);
        }

        public int SaveAsDraft(ProductDTO productDTO, string userId)
        {
            int productId = 0;

            if (productDTO.ProductId == 0)
            {
                long mediaFolderId = 0;

                var media = new Media
                {
                    MediaGuid = Guid.NewGuid(),
                    //ParentId = rootGalleryId,
                    Name = productDTO.Name,
                    MediaTypeId = (long)MD.Bidding.Library.EnumTypes.MediaType.Folder,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    Deleted = false
                };

                this.UnitOfWork.Get<Media>().AddNew(media);
                this.UnitOfWork.SaveChanges();
                mediaFolderId = media.MediaId;

                var n = new Product()
                {
                    Name = productDTO.Name,
                    Description = productDTO.Description,
                    MinimumPrice = productDTO.MinimumPrice,
                    BiddingPeriod = productDTO.BiddingPeriod,
                    CategoryId = productDTO.CategoryId,
                    TimeStamp = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,
                    MediaFolderId =mediaFolderId,
                    BiddingPeriodEndsOn = productDTO.BiddingPeriodEndsOn
                };

                this.UnitOfWork.Get<Product>().AddNew(n);
                this.UnitOfWork.SaveChanges();
                productId = n.ProductId;

            }
            else
            {
                var product = this.UnitOfWork.Get<Product>().AsQueryable()
                    .FirstOrDefault(m => m.ProductId == productDTO.ProductId);
                if (product != null)
                {
                    product.Name = productDTO.Name;
                    product.Description = productDTO.Description;  
                    product.MinimumPrice = productDTO.MinimumPrice;
                    product.CategoryId = productDTO.CategoryId;
                    product.BiddingPeriod = productDTO.BiddingPeriod;
                    product.UpdatedBy = userId;
                    product.BiddingPeriodEndsOn = productDTO.BiddingPeriodEndsOn;
                    product.TimeStamp = DateTime.Now;

                    this.UnitOfWork.Get<Product>().Update(product);
                    this.UnitOfWork.SaveChanges();
                }
                return productDTO.ProductId;
            }

            return productId;
        }

        public void MarkAsDeleted(int ProductId, string userId)
        {

            var Product = (from n in this.UnitOfWork.Get<Product>().AsQueryable()
                            where n.ProductId == ProductId
                            select n
                            ).FirstOrDefault();
            if (Product != null)
            {
                Product.DeletedBy = userId;
                Product.DeletedOn = DateTime.Now;
                Product.Deleted = true;
                this.UnitOfWork.Get<Product>().Update(Product);
                this.UnitOfWork.SaveChanges();
            }
        }

        public int SaveProductBid(BidDTO bidDTO, string userId)
        {
            int bidId = 0;

            if (bidDTO.BidId == 0)
            {

                var n = new Bid()
                {
                    Amount = bidDTO.Amount,
                    Status = (int)MD.Bidding.Library.EnumTypes.Status.Open,
                    ProductId = bidDTO.ProductId,
                    TimeStamp = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    BidderId = userId,
                    Deleted = false
                };

                this.UnitOfWork.Get<Bid>().AddNew(n);
                this.UnitOfWork.SaveChanges();
                bidId = n.BidId;

            }
            else
            {
                var bid = this.UnitOfWork.Get<Bid>().AsQueryable()
                    .FirstOrDefault(m => m.BidId == bidDTO.BidId);
                if (bid != null)
                {
                    bid.Amount = bidDTO.Amount;
                    bid.Status = bidDTO.Status;
                    bid.UpdatedBy = userId;
                    bid.TimeStamp = DateTime.Now;

                    this.UnitOfWork.Get<Bid>().Update(bid);
                    this.UnitOfWork.SaveChanges();
                }
                return bidDTO.BidId;
            }
            return bidId;
        }

        public IEnumerable<Bid> GetAllProductBids(int productId)
        {
            return this.UnitOfWork.Get<Bid>().AsQueryable()
                .Where(m => m.Deleted == false && m.ProductId == productId);
        }

        public IEnumerable<Bid> GetAllBids()
        {
            return this.UnitOfWork.Get<Bid>().AsQueryable()
                .Where(m => m.Deleted == false);
        }

        public Bid GetBid(int bidId)
        {
            return this.UnitOfWork.Get<Bid>().AsQueryable()
                .FirstOrDefault(m =>
                    m.BidId == bidId &&
                    m.Deleted == false
                );
        }

        public void UpdateBidStatus(int bidId,int status)
        {
            var bid = GetBid(bidId);
            if (bid != null)
            {
                bid.Status = status;
                bid.TimeStamp = DateTime.Now;

                this.UnitOfWork.Get<Bid>().Update(bid);
                this.UnitOfWork.SaveChanges();
            }
        }
       
    }
}
