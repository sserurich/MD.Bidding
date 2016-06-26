using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD.Bidding.Models
{
    public class MediaFolder
    {
        public long id { get; set; }
        public long MediaId { get; set; }
        public System.Guid MediaGuid { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string label { get; set; }
        public List<MediaFolder> children { get; set; }
        public bool selected { get; set; }
        
    }
}
