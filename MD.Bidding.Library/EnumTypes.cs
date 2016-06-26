using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD.Bidding.Library
{
    public class EnumTypes
    {
        public enum MediaType : int
        {
            Folder = 1,
            Image = 2,
            Document = 3,
        }

        public enum ExtensionType : int
        {
            pdf = 1,
            doc = 2,
            docx = 3,
        }

        

        public enum Status : int
        {
            Open = 1,
            Closed = 2,
            OnGoing = 3
        }

      
    }
}
