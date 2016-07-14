using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
   public  class SerialRageNumber
    {
        public Guid  ID { get; set; }
        public string  SerialNumber { get; set; }
        public Guid? StoreOrderDetailFK { get; set;  }
        public virtual StoreOrderDetail StoreOrderDetail { get; set; }

    }
}
