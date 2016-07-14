using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Barcode
{
  public  class SSCCBarcode : RepositoryBase<SSCC>, ISSCCBarcode
    {

        public SSCCBarcode(EDIContext context )
            :base(context )
        {

        }
    }
}
