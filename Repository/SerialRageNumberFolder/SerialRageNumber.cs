using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SerialRageNumberFolder
{
    public  class SerialRage :RepositoryBase<SerialRageNumber >, ISerialRage 
    {

        public SerialRage(EDIContext context )
            :base(context  ) { }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }
    }
}
