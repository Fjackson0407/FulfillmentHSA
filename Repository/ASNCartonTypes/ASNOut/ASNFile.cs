using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ASNOut
{
    public class ASNFile : RepositoryBase<ASNFileOutBound> , IASNFile 
    {

        public ASNFile(EDIContext context )
            :base(context )
        { }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

    }
}
