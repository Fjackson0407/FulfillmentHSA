using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ASNSettings
{
 public     class ASN856Settings : RepositoryBase<ASNConfig> , IASN856Settings
    {

        public ASN856Settings(EDIContext context)
            : base(context)
        { }

    }
}
