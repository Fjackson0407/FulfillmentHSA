using Domain;
using Repository.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Helpers.EDIHelperFunctions;

namespace Repository.Barcode
{
public   interface ISSCCBarcode : IRepositoryBase<SSCC>
    {
        string GetNextSequenceNumber(SSCCStatus used);
    }
}
