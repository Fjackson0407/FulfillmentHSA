using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  public   class Bill
    {
        public Guid ID { get; set; }
        public int   BOLNumber { get; set; }
        public virtual Store Store { get; set; }
        public Guid? StorFK { get; set; }

    }
}
