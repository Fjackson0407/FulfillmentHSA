using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.MinWeightFolder
{
   public  class MinWeightShipBox : RepositoryBase<MinWeightForShipping> , IMinWeightShipBox 
    {

        public MinWeightShipBox(EDIContext context)
            : base(context) 
        { }

        public int SaveChange()
        {
            return this.Context.SaveChanges();
        }


    }
}
