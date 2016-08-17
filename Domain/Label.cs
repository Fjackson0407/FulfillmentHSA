using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
public  class Label
    {
        public int Count { get; set; }
        public string   OrderStoreNumber { get; set; }
        public string  DcNumber { get; set; }
        public string PONumber { get; set; }

        public string  To { get; set; }
        public string  SSCC { get; set; }
        
        public string  From { get; set; }

        public string  Faddress { get; set; }
        public string  Fcity { get; set; }
        public string  Fstate { get; set; }
        public string  FZip { get; set; }


        public string Taddress { get; set; }
        public string Tcity { get; set; }
        public string Tstate { get; set; }
        public string Tzip { get; set; }


    }
}
