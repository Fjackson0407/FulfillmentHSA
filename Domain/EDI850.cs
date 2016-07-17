using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Store
    {

        public Store()
        {
            StoreOrderDetail = new HashSet<StoreOrderDetail>();
        }
        public Guid Id { get; set; }

        public DateTime DTS { get; set; }

        public string CompanyCode { get; set; }

        public string  UPCode  { get; set; }
        public string CustomerNumber { get; set; }

        public string PONumber { get; set; }

        public string ShippingLocationNumber { get; set; }

        public string VendorNumber { get; set; }

        public DateTime   PODate { get; set; }

        public string  ShipDate { get; set; }

        public string  CancelDate { get; set; }

        public string DCNumber { get; set; }

        public int ASNStatus { get; set; }

        public int PickStatus { get; set; }

        public string OrderStoreNumber { get; set; }

        public string BillToAddress { get; set; }

        public int QtyOrdered { get; set; }

        public string DocumentId { get; set; }

        public string OriginalLine { get; set; }

        public  ICollection<StoreOrderDetail> StoreOrderDetail { get; set; }
    }
}

