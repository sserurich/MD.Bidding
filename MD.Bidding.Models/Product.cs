using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD.Bidding.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinimumPrice { get; set; }
        public Nullable<int> BiddingPeriod { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public string CategoryName { get; set; }
        public long MediaFolderId { get; set; }
        public System.DateTime BiddingPeriodEndsOn { get; set; }
        public string SellerPhoneNumber { get; set; }
        public IEnumerable<Media> ProductImages { get; set; }
        public decimal AmountSold { get; set; }
        public string SellerEmail { get; set; }
        public string CreatedByUserId { get; set; }
    }
}
