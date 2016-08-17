using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace Domain
{
    public class ASNFileOutBound
    {
        public ASNFileOutBound()
        {
            Store = new HashSet<StoreInfoFromEDI850>();
        }
        public Guid Id { get; set; }
        public string File { get; set; }
        public DateTime  DTS { get; set; }

        public virtual ICollection<StoreInfoFromEDI850> Store { get; set;  }
    }
}
