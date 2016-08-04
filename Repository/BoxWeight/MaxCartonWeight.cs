using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.BoxWeight
{
   public  class MaxCartonWeight : RepositoryBase<MaxWeight> , IMaxCartonWeight
    {

        public MaxCartonWeight(EDIContext context)
            : base(context) { }


        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }
    }
}
