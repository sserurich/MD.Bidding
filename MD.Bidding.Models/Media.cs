using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD.Bidding.Models
{
    public class Media
    {
        public long MediaId { get; set; }
        public System.Guid MediaGuid { get; set; }
        public string Name { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string Path { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<int> Filesize { get; set; }
        public Nullable<long> MediaTypeId { get; set; }
        public Nullable<long> ExtensionTypeId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public List<Media> Children { get; set; }
        public string ExtensionType { get; set; }
    }
}
