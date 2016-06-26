using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD.Bidding.Models
{
    public class Bid
    {
        public int BidId { get; set; }       
        public decimal Amount { get; set; }
        public int ProductId { get; set; }
        public string BidderId { get; set; }
        public int Status { get; set; }
        public string BidStatusName { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public string Product { get; set; }
        public string BidderPhoneNumber { get; set; }
        public string SellerPhoneNumber { get; set; }
        public string SellerEmail { get; set; } 
    }
}
