using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
public   class Report
    {
        public  string PONumber { get; set; }
        public string  Dept { get; set; }
        public string  Vendor { get; set; }
        public string  VOP { get; set; }
        public int Items { get; set; }
        public int Units { get; set; }
        public int Cartons { get; set; }
        public double  Cost { get; set; }
        public double Weight { get; set; }


    }
}
