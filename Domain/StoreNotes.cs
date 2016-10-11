using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
public   class StoreNotes
    {
        public Guid ID { get; set; }
        public string Notes { get; set; }
        public virtual StoreInfoFromEDI850 StoreInfoFromEDI850 { get; set; }
        public Guid? StoreInfoFromEDI850FK { get; set; }

    }
}
