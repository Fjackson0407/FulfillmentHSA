using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EmptyBoxFolder
{
 public  class EmptyBox: RepositoryBase<EmptyBoxWeight> , IEmptyBox 
    {
        public EmptyBox(EDIContext context)
            : base(context) { }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }
    }
}
