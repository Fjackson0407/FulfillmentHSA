using Repository.Barcode;
using Repository.Cartons;
using Repository.ContactData856;
using Repository.DC;
using Repository.Inbound850;
using Repository.OperatorFolder;
using Repository.SerialRageNumberFolder;
using Repository.Shipping;
using Repository.SkuFolder;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UOW
{
    public interface IUnitofWork : IDisposable
    {

        IAddEDI850 AddEDI850 { get; }
        ISkus Sku { get; }

        ISSCCBarcode SSCCBarcode { get; }
        ISerialRage SerialRage { get; }
        IDCInfo DCInfo { get; }
        IShipFrom ShipFrom {get;  }
        IASNContact ASNContact { get; }
        IOperator Operator { get; }
        ICartons856 _Cartons { get;  }
        int Complate();
    }
}
