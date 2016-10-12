using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class HoildayOrder
    {
        public Guid id { get; set; }
        public string DC { get; set; }
        public string Store { get; set; }
        public string UPC { get; set; }
        public int QtyOrdered { get; set; }
        public string ItemDescription { get; set; }
        public string CompanyCode { get; set; }
        public string VendorNumber { get; set; }
        public string  PO { get; set; }
        public string DocumentId { get; set; }
        public DateTime PODate { get; set; }
        public string   CustomerLineNumber { get; set; }
        public string  DPCI { get; set; }
        public string  SSCC { get; set; }

    }
}
