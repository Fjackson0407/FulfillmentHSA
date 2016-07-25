using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Domain.XmlSerializer
{
    [XmlType(TypeName = "StoreOrderDetailSerializer")]
    public  class StoreOrderDetailSerializer
    {
        
        public int QtyOrdered { get; set; }
        public string DPCI { get; set; }

        public string ItemDescription { get; set; }

        public string UPC { get; set; }

        public Guid? SKUFK { get; set; }

        public Guid? CartonFK { get; set; }

        public virtual Carton Carton { get; set; }

        public int QtyPacked { get; set; }
        public virtual Store Store { get; set; }
        public Guid? StorFK { get; set; }





    }
}
