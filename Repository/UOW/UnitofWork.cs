using Repository.ASNCartonTypes;
using Repository.ASNOut;
using Repository.Barcode;
using Repository.BillOfLadingFolder;
using Repository.BoxWeight;
using Repository.BundleWeightForCardType;
using Repository.Cartons;
using Repository.ContactData856;
using Repository.DataSource;
using Repository.DC;
using Repository.EmptyBoxFolder;
using Repository.Inbound850;
using Repository.MinWeightFolder;
using Repository.OperatorFolder;
using Repository.PackSize;
using Repository.SerialRageNumberFolder;
using Repository.Shipping;
using Repository.ShipProduct;
using Repository.SkuFolder;
using Repository.StoreDetailFolder;
using Repository.UserOrderFolder;
using Repository.Users;
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
         public  ICartons856 _Cartons { get; private set; }

        public IOperator Operator { get; private set; }
        public ISerialRage SerialRage { get; private set; }
        public IBOL Bol { get; private set;  }

        public IUserLogin User { get; private set;  }

        public IUserOrderLog UserOrderLog { get; private set; }

        public IMaxCartonWeight MaxCartonWeight { get; private set;}

        public IPackSizeForBundles PackSizeForBundles { get; private set;  }

        public ICartonTypeForASN CartonType { get; private set;  }

        public IASNFile ASNFile { get; private set; }

        public IShipDateRequest ShipDateRequest { get; private set; }

         public  ICardWeight CardWeight { get; private set;  }
        public IEmptyBox EmptyBox { get; private set; }

        public  IMinWeightShipBox MinWeightShipBox { get; private set;  }
        public IStoreItemDetail StoreItemDetail { get; private set; }

        public UnitofWork(EDIContext cEDIContext)
        {

            _EDIContext = cEDIContext;
            AddEDI850 = new AddEDI850(_EDIContext);
            User = new UserLogin(_EDIContext);
            Sku = new Skus(_EDIContext);
            SSCCBarcode = new SSCCBarcode(_EDIContext);
            DCInfo = new DCInfo(_EDIContext);
            ShipFrom = new ShipFrom(_EDIContext);
            ASNContact = new ASNContact(_EDIContext);
            _Cartons = new Cartons856(_EDIContext);
            Operator = new Operator(_EDIContext);
            Bol = new BOL(_EDIContext);
            UserOrderLog = new UserOrderLog(_EDIContext);
            MaxCartonWeight = new MaxCartonWeight(_EDIContext);
            PackSizeForBundles = new PackSizeForBundles(_EDIContext);
            CartonType = new CartonTypeForASN(_EDIContext);
            ASNFile = new ASNFile(_EDIContext);
            ShipDateRequest = new ShipDateRequest(_EDIContext);
            CardWeight = new CardWeight(_EDIContext);
            EmptyBox = new EmptyBox(_EDIContext);
            MinWeightShipBox = new MinWeightShipBox(_EDIContext);
            StoreItemDetail = new StoreItemDetail(_EDIContext);
        }

        /// <summary>
        /// This function will save the data to theee 
        /// </summary>
        /// <returns></returns>
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
