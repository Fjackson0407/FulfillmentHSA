﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Domain
{
    public class SkuItem
    {

      public SkuItem()
        {
            Store = new HashSet<StoreInfoFromEDI850>();
        }
        /******************************************************************************
		* Member Variables
		******************************************************************************/
        public Guid Id { get; set; }
        public string  DPCI { get; set; }
        public string Brand  { get; set; }
        public string  Product { get; set; }
        public string  SubProduct { get; set; }
        public string  DENOM { get; set; }
        public string  BIN { get; set; }
        public string  GCCardType { get; set; }
        public string  GCProdId { get; set; }
        public string DCMSID { get; set; }
        public string EmbossedLine { get; set; }
        public string  DEPT { get; set; }

        public string  Class { get; set; }
        public string  Item { get; set; }
        public string  ProductUPC { get; set; }
        public string  PackageUPC { get; set; }

       public virtual ICollection<StoreInfoFromEDI850> Store { get; set; }

    }
}
