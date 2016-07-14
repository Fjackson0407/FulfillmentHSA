using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class StoreOrderDetail
    {

        public StoreOrderDetail()
        {
            SkuItem = new HashSet<SkuItem>();
            SerialRageNumber = new HashSet<SerialRageNumber>();
        }

        public Guid  Id { get; set; }
        public decimal OrderAmount { get; set; }

        public int QtyOrdered { get; set; }

        public string UnitofMeasure { get; set; }

        public decimal UnitPrice { get; set; }

        public string CustomerLine { get; set; }

        public virtual ICollection<SerialRageNumber> SerialRageNumber { get; set; }
        public int  QtyPacked  { get; set; }

        public Guid? SKUFK { get; set;  }
        
        public Guid? CartonFK { get; set; }

        public virtual Carton Carton { get; set; }
        public string OrderCases { get; set; }


        public virtual Store Store { get; set;  }
        public Guid? StorFK { get; set; }

        public virtual ICollection<SkuItem> SkuItem { get; set; }

    }
}
