using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class StoreReport
    {

        public int Number { get; set; }
        public string StoreNumber { get; set; }
        public string PONumber { get; set; }
        public string UPC { get; set; }
        public string DPCI { get; set; }
        public string DPCIDigit
        {
            get
            {
                return DPCI.Substring(DPCI.Length - 2, 2);

            }
        }
        public int QtyOrdered { get; set; }
        public int Budles
        {
            get
            {
                return QtyOrdered / 25;
            }
        }
        public double ItemWeight { get; set; }
        public int Boxes { get; set; }


    }
}
