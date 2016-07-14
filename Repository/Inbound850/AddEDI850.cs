using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Inbound850
{
  public  class AddEDI850: RepositoryBase<Store>, IAddEDI850
    {

        public AddEDI850(EDIContext context)
            : base(context) 
        { }

        public int SaveChange()
        {
            return this.Context.SaveChanges();
        }

      

    }
}
