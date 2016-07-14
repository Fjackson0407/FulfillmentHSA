using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  public  class DCInformation
    {

        public Guid Id { get; set; }
        public string  Address { get; set; }
        public string City { get; set; }
        public string  State { get; set; }
        public string PostalCode { get; set; }
        public string BillAndShipToCodes { get; set; }
        public string  StoreID { get; set; }


    }
}
