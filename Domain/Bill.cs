﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  public   class BOLForASN 
    {
        public Guid ID { get; set; }
        public int   BOLNumber { get; set; }
        public virtual StoreInfoFromEDI850 Store { get; set; }
        public Guid? StoreInfoFK { get; set; }

    }
}
