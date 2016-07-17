using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
public  class Carton
    {

        public Carton()
        {
            StoreOrderDetail = new HashSet<StoreOrderDetail>();
        }
        public Guid  Id { get; set; }

        public string UCC128 { get; set; }

        public int Weight { get; set; }

        public virtual ICollection<StoreOrderDetail> StoreOrderDetail { get; set;  }

    }
}
