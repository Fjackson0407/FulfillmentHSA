using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
public  class Carton
    {
        public Guid  Id { get; set; }

        public int Qty { get; set; }

        public string UCC128 { get; set; }

        public int Weight { get; set; }

        public virtual  StoreInfoFromEDI850 StoreNumber { get; set; }
        public Guid? StoreNumberFK { get; set; }


    }
}
