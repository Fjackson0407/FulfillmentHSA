using Domain;
using EDIException;
using Helpers;
using Repository.DataSource;
using Repository.Shipping;
using Repository.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static Helpers.EDIHelperFunctions;

namespace ASNService
{
    public class ASNBuild
    {
        public string path { get; set; }
        public string ConnectionString { get; set; }
        public string PO { get; set; }
        public string CurrentStore { get; set; }
        public StoreOrderDetail CurentStoreOrderDetail { get; set; }
        public Guid CurrentID { get; set; }
        public string CurrentDCNumber { get; private set; }

        public ASNBuild(string _path, string _ConnectionString, string _PO, string _Store, string _DCNumber)
        {
            PO = _PO;
            path = _path;
            ConnectionString = _ConnectionString;
            CurrentStore = _Store;
            CurrentDCNumber = _DCNumber;

        }


        #region Xml code 
        XElement BuildHeader()
        {
            return new XElement(HEADER,
                new XElement(CompanyCode, GetCompanyCode()),
                new XElement(CUSTOMERNUMBER, GetCustomerNumber()),
                new XElement(Direction, EDIHelperFunctions.Outbound),
                new XElement(DocumentType, EDIHelperFunctions.EDI856),
               new XElement(EDIHelperFunctions.Version, VersionNumber),
              new XElement(EDIHelperFunctions.Footprint, ASN),
              new XElement(ShipmentID, GetNewShipmentID()),
             new XElement(EDIHelperFunctions.ShippingLocationNumber,   new XCData(CurrentStore)    ),
             new XElement(InternalDocumentNumber, GetDocID() ));

        }


        XElement BuildShipmentLevel()
        {
            return new XElement(EDIHelperFunctions.ShipmentLevel,
                   new XElement(TransactionSetPurposeCode, "00"),
                                new XElement(EDIHelperFunctions.DateLoop,
                                new XElement(EDIHelperFunctions.DateQualifier, ShipDateDateQualifierNumber,
                                new XAttribute(EDIHelperFunctions.Desc,
                                EDIHelperFunctions.ShipDateString)),
                                new XElement(EDIHelperFunctions.Date, ShipDate)),
                                new XElement(EDIHelperFunctions.ManifestCreateTime,
                                DateTime.Now.ToLongTimeString()),
                                new XElement(EDIHelperFunctions.ShipmentTotals,
                                new XElement(EDIHelperFunctions.ShipmentTotalCube, ShipmentTotalCubeValue),
                                new XElement(EDIHelperFunctions.ShipmentPackagingCode, EDIHelperFunctions.CartonType),
                                new XElement(ShipmentTotalCases, GetShippingWeightOrBoxCount(ShippingInfoType.BoxCount)),
                                new XElement(ShipmentTotalWeight, GetShippingWeightOrBoxCount(ShippingInfoType.ShippingWeight))),
                                BuildCarrier(), BuildBOL(),
                                new XElement(MethodOfPayment, EDIHelperFunctions.DF),
                                new XElement(FOBLocationQualifier, "OR"),
                                new XElement(FOBDescription, new XCData(EDIHelperFunctions.FOBDescription2)),
                                BuildShipFrom(), BuildShipTo(),
                                new XElement(EDIHelperFunctions.VendorCode, GetVendorCode()),
                                BuildContactType());
        }

        private XElement  BuildTransportationMethod()
        {
            return new XElement(TransportationMethod, TransportationMethodType);
        }

        private XElement BuildCarrier()
        {
            return new XElement(Carrier,
                new XElement(CarrierCode,  new XCData( NFIL)),
             new XElement(CarrierType, CarrierTypeVaule),
             BuildTransportationMethod());

        }

        private XElement BuildBOL()
        {
            int  BOLNumber = GetNewBOLID();
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                Store cStore = UoW.AddEDI850.Find(t => t.PONumber == PO).FirstOrDefault();
                Bill cBill = new Bill();
                cBill.ID = Guid.NewGuid();
                cBill.Store = cStore;
                cBill.BOLNumber = BOLNumber;
                UoW.Bol.Add(cBill);
                int Result = UoW.Complate();

            }
            return new XElement(BillOfLadingNumber, new XCData(BOLNumber.ToString()));
        }

        private XElement BuildContactType()
        {
            ContactType cContactInfo = GetContactInfo();
            return new XElement(EDIHelperFunctions.ContactType,
                     new XElement(EDIHelperFunctions.FunctionCode,  new XCData(EDIHelperFunctions.CUST)),
                     new XElement(EDIHelperFunctions.ContactName,  new XCData( cContactInfo.FullName)),
                     new XElement(EDIHelperFunctions.ContactQualifier,  EDIHelperFunctions.Phone),
                     new XElement(EDIHelperFunctions.PhoneEmail,  new XCData( cContactInfo.PhoneNumber)),
                     new XElement(EDIHelperFunctions.ContactQualifier, EDIHelperFunctions.Email),
                     new XElement(EDIHelperFunctions.PhoneEmail,  new XCData(cContactInfo.EmailAddress)));


        }


        XElement BuildShipFrom()
        {
            ShipFromInformation ShipFrom = GetShipFromAddress();
            return new XElement(NAME, new XElement(BillAndShipToCode, EDIHelperFunctions.ShipFrom),
                new XElement(EDIHelperFunctions.DUNSOrLocationNumberString, ShipFrom.DUNSOrLocationNumber),
                new XElement(EDIHelperFunctions.NameComponentQualifier, EDIHelperFunctions.DescShipFrom),
                new XElement(EDIHelperFunctions.NameComponent,  new XCData( EDIHelperFunctions.ValidName)),
                new XElement(EDIHelperFunctions.CompanyName,  new XCData( EDIHelperFunctions.ValidName)),
                new XElement(EDIHelperFunctions.Address,  new XCData(ShipFrom.Address)),
                new XElement(EDIHelperFunctions.City,  new XCData( ShipFrom.City)),
                new XElement(EDIHelperFunctions.State,  new XCData(ShipFrom.State)),
                new XElement(EDIHelperFunctions.Zip, ShipFrom.PostalCode));

        }

        private ShipFromInformation GetShipFromAddress()
        {
            ShipFromInformation cAddreessDomain = null;

            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                cAddreessDomain = UoW.ShipFrom.Find(t => t.BillAndShipToCodes == EDIHelperFunctions.ShipFrom).FirstOrDefault();
                if (cAddreessDomain != null)
                {
                    return cAddreessDomain;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0}{1}{2}", EDIHelperFunctions.Help, EDIHelperFunctions.Space, ErrorCodes.HSAErro41));
                }
            }

        }

        XElement BuildShipTo()
        {
            DCInformation ShipTo = GetShipToAddress();
            return new XElement(NAME,
                    new XElement(EDIHelperFunctions.BillAndShipToCode, EDIHelperFunctions.ShipTo),
                    new XElement(EDIHelperFunctions.DUNSOrLocationNumberString, DCNumber),
                    new XElement(EDIHelperFunctions.NameComponentQualifier, EDIHelperFunctions.DescShipTo),
                    new XElement(EDIHelperFunctions.CompanyName,  new XCData(string.Format("{0}    {1}", EDIHelperFunctions.SHIPPREDISTROTODC, DCNumber))),
                    new XElement(EDIHelperFunctions.Address,  new XCData( ShipTo.Address)),
                    new XElement(EDIHelperFunctions.City,  new XCData(ShipTo.City)),
                    new XElement(EDIHelperFunctions.State, new XCData( ShipTo.State)),
                    new XElement(EDIHelperFunctions.Zip,  new XCData(ShipTo.PostalCode)),
                    new XElement(EDIHelperFunctions.Country, EDIHelperFunctions.US));

        }

        public void BuildASN()
        {

            #region Test Header
            var File = new XElement(FILE,
                new XElement(EDIHelperFunctions.DOCUMENT, BuildHeader(), BuildShipmentLevel(), BuildOrderLevel()));
            #endregion
            File.NextNode 
            //SaveASN(File);
            //List<Store> lisStore = UpdatePo();
            //    SaveForInventory(lisStore);
            File.Save(path);
        }

        private void SaveForInventory(List<Store> lisStore)
        {
            if (lisStore != null && lisStore.Count > 0)
            {
                using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
                {
                    StoreOrderDetail cStoreOrderDetail = new StoreOrderDetail();
                    foreach (Store store in lisStore)
                    {
                        cStoreOrderDetail.Id = Guid.NewGuid();
                        cStoreOrderDetail.Store = store;
                    }
                }
            }

        }

        private List<Store> UpdatePo()
        {

            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                List<Store> lisStore = UoW.AddEDI850.Find(x => x.PONumber == PO)
               .Where(t => t.OrderStoreNumber == CurrentStore)
               .Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN).ToList();
                lisStore.ForEach(t => t.ASNStatus = (int)ASNStatus.HasASN);
                int Result = UoW.AddEDI850.SaveChange();
                return lisStore;
            }
        }

        private XElement BuildOrderLevel()
        {
            return new XElement(ORDERLEVEL,
                   new XElement(IDS,
                   new XElement(EDIHelperFunctions.PurchaseOrderNumber,  new XCData( PO)),
                   new XElement(EDIHelperFunctions.PURCHASEORDERSOURCEID, GetDocID()),
                   new XElement(EDIHelperFunctions.PurchaseOrderDate, GetPODate())),
                   BuildOrderTotals(), BuildPickStruture());

        }

        XElement BuildPickStruture()
        {
            List<Store> cInbound850Current = GetOrders();
            List<StoreOrderDetail> lisStoreOrderDetail = GetSKUInfo(cInbound850Current);
            List<Carton> Cartons = BuildPickPackStructure(cInbound850Current);
            return BuildPickPackStructureXML(Cartons);
        }

        private XElement BuildPickPackStructureXML(List<Carton> cartons)
        {

            foreach (Carton item in cartons)
            {
                return new XElement(PICKPACKSTRUTURE,
                               new XElement(EDIHelperFunctions.Carton,
                               new XElement(MARKS,
                               new XElement(UCC128, new XCData( item.UCC128))),
                               new XElement(Quantities,
                               new XElement(QtyQualifier, QtyQualifierZZ),
                               new XElement(QtyUOM, Each),
                               new XElement(Qty, item.Qty))), BuildItemDetail(item.StoreOrderDetail));

            }

            return null;
        }


        private XElement BuildItemDetail(ICollection<StoreOrderDetail> lisStoreDetils)
        {
            return BuildSingleItem(lisStoreDetils);
        }

        private XElement BuildSingleItem(ICollection<StoreOrderDetail> items)
        {
            XElement ItemElement = new XElement(ITEM,
            from i in items
            select new XElement(ITEM,

            new XElement(CustomerLineNumber, i.CustomerLineNumber),
                            new XElement(ItemIDs,
                            new XElement(IdQualifier, UP),
                            new XElement(ID, new XCData( i.UPC))),
                            new XElement(ItemIDs,
                            new XElement(IdQualifier, VN),
                            new XElement(ID,  new XCData( "SVS Holida"))),
                            new XElement(ItemIDs,
                            new XElement(IdQualifier, CU),
                            new XElement(ID, new XCData( i.DPCI))),
                            new XElement(Quantities,
                            new XElement(QtyQualifier, "39"),
                            new XElement(QtyUOM, Each),
                            new XElement(Qty, QtyUOMSize)),
                            new XElement(PackSize, InnersPerPacksSize),
                            new XElement(Inners, EachesPerInnerInt),
                            new XElement(EachesPerInner, EachesPerInnerInt),
                            new XElement(InnersPerPacks, InnersPerPacksSize),
                            new XElement(ItemDescription, new XCData( i.ItemDescription))));

            return ItemElement;
        }


        private XElement BuildSingleItem(StoreOrderDetail item, int LineNumber)
        {
            return new XElement(ITEM,
                            new XElement(CustomerLineNumber, LineNumber),
                            new XElement(ItemIDs,
                            new XElement(IdQualifier, UP),
                            new XElement(ID, item.UPC)),
                            new XElement(ItemIDs,
                            new XElement(IdQualifier, VN),
                            new XElement(ID, "SVS Holida")),
                            new XElement(ItemIDs,
                            new XElement(IdQualifier, CU),
                            new XElement(ID, item.DPCI)),
                            new XElement(Quantities,
                            new XElement(QtyQualifier, QtyQualifierZZ),
                            new XElement(QtyUOM, QtyUOMSize),
                            new XElement(Qty, QtyUOMSize)),
                            new XElement(PackSize, InnersPerPacksSize),
                            new XElement(Inners, EachesPerInnerInt),
                            new XElement(EachesPerInner, EachesPerInnerInt),
                            new XElement(InnersPerPacks, InnersPerPacksSize),
                            new XElement(ItemDescription, item.ItemDescription));

        }
        /// <summary>
        /// I need to review this 
        /// </summary>
        /// <param name="lisStore"></param>
        /// <returns></returns>
        private List<Carton> BuildPickPackStructure(List<Store> lisStore)
        {

            List<Carton> lisCarton = new List<Domain.Carton>();
            int iQtyTotals = 0;
            int iCartonWeightWithIems = 0;
            int CustomerLineNumber = 1;
            Carton _Carton = new Domain.Carton();


            _Carton = GetSSCCForNewOrder();

            foreach (Store item in lisStore)
            {
                StoreOrderDetail cStoreOrderDetail = new StoreOrderDetail();
                iQtyTotals += item.QtyOrdered;
                iCartonWeightWithIems = (iQtyTotals / InnersPerPacksSizeInt) + BOXWEIGHT;
                if (iCartonWeightWithIems >= (MAXLBS - 1))
                {
                    lisCarton.Add(_Carton);
                    _Carton = GetSSCCForNewOrder();
                    iQtyTotals = 0; //Reset totals for a new cartons
                    iCartonWeightWithIems = 0;
                }
                else
                {


                    cStoreOrderDetail.Carton = _Carton;
                    cStoreOrderDetail.CartonFK = _Carton.Id;
                    cStoreOrderDetail.Id = Guid.NewGuid();
                    cStoreOrderDetail.QtyOrdered = item.QtyOrdered;
                    SkuItem ItemDescription = GetItemDescription(item.UPCode);
                    cStoreOrderDetail.CustomerLineNumber = CustomerLineNumber;
                    CustomerLineNumber++;
                    cStoreOrderDetail.ItemDescription = ItemDescription.SubProduct;
                    cStoreOrderDetail.DPCI = ItemDescription.DPCI;
                    cStoreOrderDetail.UPC = ItemDescription.ProductUPC;
                    cStoreOrderDetail.QtyPacked = 0;
                    cStoreOrderDetail.SKUFK = ItemDescription.Id;
                    _Carton.StoreOrderDetail.Add(cStoreOrderDetail);
                    if (!lisCarton.Exists(t => t.Id == _Carton.Id))
                    {
                        lisCarton.Add(_Carton);
                    }
                }
                _Carton.Weight = iCartonWeightWithIems;
                _Carton.Qty = iQtyTotals;

            }
            SaveCarton(lisCarton);



            return lisCarton;
        }

        private SkuItem GetItemDescription(string uPCode)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                return UoW.Sku.Find(t => t.ProductUPC == uPCode.Replace(@"'", "")).FirstOrDefault();
            }
        }

        private void SaveCarton(List<Carton> lisCarton)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                if (lisCarton.Count > 0)
                {
                    UoW._Cartons.AddRage(lisCarton);
                    int iResult = UoW.Complate();
                    //if iResult = 0 then throw a error 
                }
            }
        }

        XElement GetPick(StoreOrderDetail cStoreOrderDetail, int iCustomerLineNumber)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                SkuItem sku = UoW.Sku.Find(t => t.Id == cStoreOrderDetail.SKUFK).FirstOrDefault();

                return new XElement(ITEM,
                         new XElement(CustomerLineNumber, iCustomerLineNumber),
                         new XElement(EDIHelperFunctions.ItemIDs,
                         new XElement(IdQualifier, UP),
                         new XElement(Id, sku.ProductUPC)),
                         new XElement(EDIHelperFunctions.ItemIDs,
                         new XElement(IdQualifier, VN),
                         new XElement(Id, VIN)),
                         new XElement(ITEMIDS,
                         new XElement(IdQualifier, CU),
                         new XElement(Id, sku.DPCI)),
                         new XElement(Quantities,
                         new XElement(QtyQualifier, "39"),
                         new XElement(QtyUOM, Each),
                         new XElement(Qty, QtyUOMSize)),
                         new XElement(PackSize, QtyUOMSize),
                         new XElement(Inners, one),
                         new XElement(EachesPerInner, one),
                         new XElement(InnersPerPacks, InnersPerPacksSize),
                         new XElement(ItemDescription, sku.Product));
            }

        }



        XElement BuildOrderTotals()
        {
            int t = GetShippingWeightOrBoxCount(ShippingInfoType.ShippingWeight);
            return new XElement(ORDERTOTALS,
                    new XElement(OrderTotalCases, GetShippingWeightOrBoxCount(ShippingInfoType.BoxCount)),
                     new XElement(OrderTotalWeight, GetShippingWeightOrBoxCount(ShippingInfoType.ShippingWeight)));

        }


        #endregion
        #region Class functions 

        private ContactType GetContactInfo()
        {
            ContactType cContactInfo = null;
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                cContactInfo = UoW.ASNContact.GetAll().FirstOrDefault();
                if (cContactInfo != null)
                {
                    return cContactInfo;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0}{1}{2}", EDIHelperFunctions.Help, EDIHelperFunctions.Space, ErrorCodes.HSAErro19));
                }
            }

        }

        private string GetVendorCode()
        {
            string sVendorCode = string.Empty;
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                Store _Store = UoW.AddEDI850.Find(t => t.PONumber == PO).Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN)
                                                                           .Where(t => t.OrderStoreNumber == CurrentStore).FirstOrDefault();
                sVendorCode = _Store?.VendorNumber;
                if (!string.IsNullOrEmpty(_Store.VendorNumber))
                {
                    return _Store.VendorNumber;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0}{1}{2}", EDIHelperFunctions.Help, EDIHelperFunctions.Space, ErrorCodes.HSAErro17));
                }
            }
        }

        private string DCNumber
        {
            get
            {
                string sStoreNumber = string.Empty;
                using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
                {
                    Store lisInbound850 = UoW.AddEDI850.Find(t => t.PONumber == PO).FirstOrDefault();
                    if (!string.IsNullOrEmpty(lisInbound850.DCNumber))
                    {
                        sStoreNumber = lisInbound850.DCNumber.Replace(@"'", "");
                    }
                    else
                    {
                        throw new ExceptionsEDI(string.Format("{0}{1}{2}", EDIHelperFunctions.Help, EDIHelperFunctions.Space, ErrorCodes.HSAErro27));
                    }
                }

                return sStoreNumber;

            }

        }

        private DCInformation GetShipToAddress()
        {
            DCInformation cDCInformation = null;

            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                //DCNumber.Replace(@"'", "")
                string DCNumber = UoW.AddEDI850.Find(x => x.PONumber == PO).Where(x => x.OrderStoreNumber == CurrentStore)
                                                        .Where(x => x.ASNStatus == (int)ASNStatus.ReadyForASN).FirstOrDefault().DCNumber;


                cDCInformation = UoW.DCInfo.Find(t => t.StoreID == CurrentDCNumber).FirstOrDefault();

                if (cDCInformation != null)
                {
                    return cDCInformation;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0}{1}{2}", EDIHelperFunctions.Help, EDIHelperFunctions.Space, ErrorCodes.HSAErro41));
                }
            }
        }

        private string GetPODate()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                Store cInbound850 = UoW.AddEDI850.Find(t => t.PONumber == PO)
                                                 .Where(x => x.OrderStoreNumber == CurrentStore)
                                                 .Where(x => x.ASNStatus == (int)ASNStatus.ReadyForASN).FirstOrDefault();
                if (cInbound850 != null)
                {
                    return cInbound850.PODate.ToString(toFormat);
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
                }
            }

        }

        private string GetDocID()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                Store store = UoW.AddEDI850.Find(t => t.PONumber == PO)
                                           .Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN)
                                           .Where(t => t.OrderStoreNumber == CurrentStore).FirstOrDefault();
                if (!string.IsNullOrEmpty(store.DocumentId))
                {
                    return store.DocumentId;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
                }
            }

        }

        private Carton GetSSCCForNewOrder()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                return new Carton() { Id = Guid.NewGuid(), UCC128 = UoW.SSCCBarcode.GetNextSequenceNumber(SSCCStatus.NotUsed) };
            }
        }



        private List<Carton> GetSSCCForNewOrder(int boxTotal)
        {
            List<Carton> lisNewSSCC = new List<Carton>();
            if (boxTotal < 1)
            {
                return null;
            }
            for (int i = 0; i < boxTotal; i++)
            {
                using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
                {
                    string sBarCode = string.Empty;
                    sBarCode = UoW.SSCCBarcode.GetNextSequenceNumber(SSCCStatus.NotUsed);
                    Carton cNewSSCC = new Carton() { Id = Guid.NewGuid(), UCC128 = sBarCode };
                    lisNewSSCC.Add(cNewSSCC);
                }
            }
            return lisNewSSCC;
        }

        private List<Store> GetOrders()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                return UoW.AddEDI850.Find(t => t.PONumber == PO).Where(x => x.ASNStatus == (int)ASNStatus.ReadyForASN)
                                                                    .Where(X => X.OrderStoreNumber == CurrentStore).ToList();
            }

        }

        public int GetShippingWeightOrBoxCount(ShippingInfoType ShippingInfo)
        {

            int dTotalWeight = 0;
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                List<Store> Stores = UoW.AddEDI850.Find(t => t.PONumber == PO)
                             .Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN)
                             .Where(t => t.OrderStoreNumber == CurrentStore)
                             .Distinct()
                             .ToList();

                foreach (Store item in Stores)
                {
                    dTotalWeight += item.QtyOrdered;
                }

                switch (ShippingInfo)
                {
                    case ShippingInfoType.PickCount:
                        return (dTotalWeight / InnersPerPacksSize);


                    case ShippingInfoType.ShippingWeight:
                        int f = (dTotalWeight / InnersPerPacksSize);
                        return (dTotalWeight / InnersPerPacksSize) + BOXWEIGHT;


                    case ShippingInfoType.BoxCount:
                        return GetNumberofBoxes(dTotalWeight / InnersPerPacksSize);

                    default:
                        return 0;

                }

            }



        }

        private int GetNumberofBoxes(int packs)
        {

            int iCount = 0;
            if (packs == 0)
            {
                return 0;
            }
            if (packs <= EDIHelperFunctions.MINLBS || packs <= (EDIHelperFunctions.MAXLBS - 1))
            {
                iCount++;
                return iCount;

            }
            else
            {

                while (packs >= (MAXLBS - 1))
                {
                    packs = packs - MAXLBS;
                    iCount++;
                }
                if (packs >= 1)
                {
                    iCount++;
                }
                return iCount;

            }
        }



        /// <summary>
        /// This function will change at some point! For now it is just todays date. Talk o Kari about this one 
        /// </summary>
        public string ShipDate
        {
            get
            {
                return DateTime.Today.ToString(toFormat);
            }

        }

        public double GetWeight
        {
            get
            {
                double dTotal = 0;

                using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
                {
                    List<Store> Stores = UoW.AddEDI850.Find(t => t.PONumber == PO)
                                                        .Where(x => x.ASNStatus == (int)ASNStatus.ReadyForASN)
                                                       .Where(t => t.OrderStoreNumber == CurrentStore).ToList();
                    foreach (Store item in Stores)
                    {
                        dTotal += item.QtyOrdered;
                    }
                    double dPackTotal = dTotal / InnersPerPacksSize;
                    dPackTotal += BOXWEIGHT;
                    return dPackTotal;
                }

            }
        }


        private string GetNewShipmentID()
        {
            return EDIHelperFunctions.VPS + EDIHelperFunctions.RandomString(EDIHelperFunctions.shiplength);

        }

        private int GetNewBOLID()
        {
            Random cRandom = new Random();
            return cRandom.Next(100000, 100000000);
        }

        private List<StoreOrderDetail> GetSKUInfo(List<Store> lisStores)
        {
            SkuItem cSkuItem = null;
            StoreOrderDetail cStoreOrderDetail = new StoreOrderDetail();
            List<StoreOrderDetail> lisStoreOrderDetail = new List<StoreOrderDetail>();

            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                if (lisStores != null && lisStores.Count > 0)
                {
                    foreach (Store store in lisStores)
                    {
                        string StoreUpc = store.UPCode.Replace(@"'", "");
                        cSkuItem = UoW.Sku.Find(t => t.ProductUPC == StoreUpc).FirstOrDefault();
                        if (cSkuItem != null)
                        {

                            cStoreOrderDetail.Id = Guid.NewGuid();
                            cStoreOrderDetail.Store = store;
                            cStoreOrderDetail.SKUFK = cSkuItem.Id;
                            lisStoreOrderDetail.Add(cStoreOrderDetail);
                        }
                    }
                    return lisStoreOrderDetail;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAErro23));
                }

            }
        }



        private string GetCustomerNumber()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                List<Store> lisStore = UoW.AddEDI850.GetAll().ToList();
                Store cEDI850 = UoW.AddEDI850.Find(t => t.PONumber == PO)
                                             .Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN)
                                             .Where(t => t.OrderStoreNumber == CurrentStore)
                                             .FirstOrDefault();
                if (cEDI850 != null)
                {
                    return cEDI850.CustomerNumber;
                }

                throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAErro47));
            }

        }


        public string GetCompanyCode()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                List<Store> lisStore = UoW.AddEDI850.GetAll().ToList();
                Store cEDI850 = UoW.AddEDI850.Find(t => t.PONumber == PO)
                                             .Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN)
                                             .Where(t => t.OrderStoreNumber == CurrentStore)
                                             .FirstOrDefault();
                if (cEDI850 != null)
                {
                    CurrentID = cEDI850.Id;
                    return cEDI850.CompanyCode;
                }

                throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAErro13));
            }

        }
        #endregion



    }
}
