using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SkuFolder
{
   public  class Skus : RepositoryBase<SkuItem>, ISkus 
    {
        public Skus(EDIContext context)
            :base(context )

        {

        }
    }
}
