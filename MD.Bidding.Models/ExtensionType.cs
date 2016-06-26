using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD.Bidding.Models
{
    public class ExtensionType
    {
        public long ExtensionTypeId { get; set; }
        public string Ext { get; set; }
        public long MediaTypeId { get; set; }
    }
}
