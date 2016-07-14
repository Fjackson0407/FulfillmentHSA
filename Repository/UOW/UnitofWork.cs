using Repository.Barcode;
using Repository.ContactData856;
using Repository.DataSource;
using Repository.DC;
using Repository.Inbound850;
using Repository.Shipping;
using Repository.SkuFolder;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UOW
{
    public class UnitofWork : IUnitofWork
    {
        protected string ConnectionString;
        private readonly EDIContext _EDIContext;
        public IAddEDI850 AddEDI850 { get; private set; }
        

        public ISkus Sku { get; private set;  }
        public ISSCCBarcode SSCCBarcode { get; private set; }

        public IDCInfo DCInfo { get; private set;  }

        public  IShipFrom ShipFrom { get; private set; }
        public IASNContact ASNContact { get; private set;  }
        public UnitofWork(EDIContext cEDIContext)
        {

            _EDIContext = cEDIContext;
            AddEDI850 = new AddEDI850(_EDIContext);
            
            Sku = new Skus(_EDIContext);
            SSCCBarcode = new SSCCBarcode(_EDIContext);
            DCInfo = new DCInfo(_EDIContext);
            ShipFrom = new ShipFrom(_EDIContext);
            ASNContact = new ASNContact(_EDIContext);
        }


        public int Complate()
        {
            return _EDIContext.SaveChanges();

        }

        public void Dispose()
        {
            _EDIContext.Dispose();
        }

    }
}
