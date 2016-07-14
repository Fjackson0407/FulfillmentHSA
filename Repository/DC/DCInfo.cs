using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DC
{
    public class  DCInfo : RepositoryBase<DCInformation>,  IDCInfo 
    {
        public DCInfo(EDIContext context )
            :base(context )
        { }

    }
}
