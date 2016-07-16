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
        public string Store { get; set; }
        public StoreOrderDetail CurentStoreOrderDetail { get; set; }
        public Guid  CurrentID { get; set; }
        public ASNBuild(string _path, string _ConnectionString, string _PO, string _Store)
        {
            PO = _PO;
            path = _path;
            ConnectionString = _ConnectionString;
            Store = _Store;
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
             new XElement(EDIHelperFunctions.ShippingLocationNumber, Store));

        }

        
        XElement BuildShipmentLevel()
        {
            return new XElement(EDIHelperFunctions.ShipmentLevel,
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
                                new XElement(ShipmentTotalCases, GetShippingWeightOrBoxCount( ShippingInfoType.BoxCount   )),
                                new XElement(ShipmentTotalWeight, GetShippingWeightOrBoxCount( ShippingInfoType.ShippingWeight  ))),
                                new XElement(MethodOfPayment, EDIHelperFunctions.DF),
                                new XElement(FOBLocationQualifier, EDIHelperFunctions.DE),
                                new XElement(FOBDescription, EDIHelperFunctions.RETAILLOCATON),
                                BuildShipFrom(), BuildShipTo(),
                                new XElement(EDIHelperFunctions.VendorCode, GetVendorCode()),
                                BuildContactType());
        }

        private XElement  BuildContactType()
        {
            ContactType cContactInfo = GetContactInfo();
            return new XElement(EDIHelperFunctions.ContactType,
                     new XElement(EDIHelperFunctions.FunctionCode, EDIHelperFunctions.CUST),
                     new XElement(EDIHelperFunctions.ContactName, cContactInfo.FullName),
                     new XElement(EDIHelperFunctions.ContactQualifier, EDIHelperFunctions.Phone),
                     new XElement(EDIHelperFunctions.PhoneEmail, cContactInfo.PhoneNumber),
                     new XElement(EDIHelperFunctions.ContactQualifier, EDIHelperFunctions.Email),
                     new XElement(EDIHelperFunctions.PhoneEmail, cContactInfo.EmailAddress));


        }


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

        private string  GetVendorCode()
        {
            string sVendorCode = string.Empty;
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                Store _Store = UoW.AddEDI850.Find(t => t.PONumber == PO).Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN)
                                                                           .Where(t => t.OrderStoreNumber == Store).FirstOrDefault();
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
                    Store lisInbound850 = UoW.AddEDI850.Find(t => t.PONumber  == PO).FirstOrDefault();
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

        XElement BuildShipFrom()
        {
            ShipFromInformation  ShipFrom = GetShipFromAddress();
            return new XElement(NAME, new XElement(BillAndShipToCode, EDIHelperFunctions.ShipFrom),
                new XElement(EDIHelperFunctions.DUNSOrLocationNumberString, ShipFrom.DUNSOrLocationNumber ),
                new XElement(EDIHelperFunctions.NameComponentQualifier, EDIHelperFunctions.DescShipFrom),
                new XElement(EDIHelperFunctions.NameComponent, EDIHelperFunctions.ValidName),
                new XElement(EDIHelperFunctions.CompanyName, EDIHelperFunctions.ValidName),
                new XElement(EDIHelperFunctions.Address, ShipFrom.Address),
                new XElement(EDIHelperFunctions.City, ShipFrom.City),
                new XElement(EDIHelperFunctions.State, ShipFrom.State),
                new XElement(EDIHelperFunctions.Zip, ShipFrom.PostalCode));

        }

        private ShipFromInformation GetShipFromAddress()
        {
            ShipFromInformation cAddreessDomain = null;

            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                cAddreessDomain = UoW.ShipFrom.Find(t => t.BillAndShipToCodes ==  EDIHelperFunctions.ShipFrom).FirstOrDefault();
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
                    new XElement(EDIHelperFunctions.DUNSOrLocationNumberString, Store),
                    new XElement(EDIHelperFunctions.NameComponentQualifier, EDIHelperFunctions.DescShipTo),
                    new XElement(EDIHelperFunctions.NameComponent, TargetStores),
                    new XElement(EDIHelperFunctions.CompanyName, TargetStores),
                    new XElement(EDIHelperFunctions.CompanyName, string.Format("{0} {1}", EDIHelperFunctions.SHIPPREDISTROTODC, DCNumber)),
                    new XElement(EDIHelperFunctions.Address, ShipTo.Address),
                    new XElement(EDIHelperFunctions.City, ShipTo.City),
                    new XElement(EDIHelperFunctions.State, ShipTo.State),
                    new XElement(EDIHelperFunctions.Zip, ShipTo.PostalCode));

        }

        private DCInformation GetShipToAddress()
        {
            DCInformation cDCInformation  = null;

            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                string DCNumber  = UoW.AddEDI850.Find(x => x.PONumber == PO).Where(x => x.OrderStoreNumber == Store)
                                                        .Where(x => x.ASNStatus == (int)ASNStatus.ReadyForASN).FirstOrDefault().DCNumber;

                cDCInformation = UoW.DCInfo.Find(t => t.StoreID   == DCNumber.Replace(@"'", "")).FirstOrDefault();
                    
                if (cDCInformation  != null)
                {
                    return cDCInformation ;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0}{1}{2}", EDIHelperFunctions.Help, EDIHelperFunctions.Space, ErrorCodes.HSAErro41));
                }
            }
        }

        public void BuildASN()
        {
           
            #region Test Header
            var File = new XElement(FILE,
                new XElement(EDIHelperFunctions.DOCUMENT, BuildHeader(), BuildShipmentLevel(), BuildOrderLevel()));
            #endregion
            //SaveASN(File);
            List<Store>  lisStore = UpdatePo();
            SaveForInventory(lisStore);
            File.Save(path);
        }

        private void SaveForInventory(List<Store> lisStore)
        {
            if (lisStore != null && lisStore.Count > 0)
            {
                using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
                {
                    StoreOrderDetail cStoreOrderDetail = new StoreOrderDetail();
                    foreach (Store  store in lisStore )
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
               .Where(t => t.OrderStoreNumber == Store)
               .Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN).ToList();
                lisStore.ForEach(t => t.ASNStatus = (int)ASNStatus.HasASN);
               int Result = UoW.AddEDI850.SaveChange();
                return lisStore;
            }
        }

        private XElement  BuildOrderLevel()
        {
            return new XElement(ORDERLEVEL,
                   new XElement(IDS,
                   new XElement(EDIHelperFunctions.PurchaseOrderNumber, PO),
                   new XElement(EDIHelperFunctions.PURCHASEORDERSOURCEID, GetDocID()),
                   new XElement(EDIHelperFunctions.PurchaseOrderDate, GetPODate()),
                   new XElement(EDIHelperFunctions.StoreNumber, Store )), BuildOrderTotals(), BuildPickStruture());

        }

        XElement BuildPickStruture()
        {
            int BoxTotal = GetShippingWeightOrBoxCount( ShippingInfoType.BoxCount );
            int Packtotal = GetShippingWeightOrBoxCount(ShippingInfoType.PickCount);
            List<Store > cInbound850Current = GetOrders();
            List<SkuItem> Skus = GetSKUInfo(cInbound850Current);
            XElement Picks;
            XElement TotalCartones;
            List<XElement> Cartons = GetBoxesforOrder(BoxTotal);
            Picks = new XElement(PICKPACKSTRUTURE);
            int iBoxCount = 1;
            int iItemCount = 0;
            XElement Carton = Cartons.Take(iBoxCount).FirstOrDefault();
            int iCustomerLineNumber = 1;
            foreach (SkuItem sku in Skus)
            {
                if (iItemCount == MAXLBS)
                {
                    TotalCartones = new XElement(Carton);
                    Carton = Cartons.Take(iBoxCount).FirstOrDefault();
                    iBoxCount--;
                }

                XElement Pick = GetPick(sku, iCustomerLineNumber);
                Carton.Add(Pick);
                iItemCount++;
                iCustomerLineNumber++;

            }

            return Carton;
        }

        private List<XElement> GetBoxesforOrder(int boxTotal)
        {

            List<XElement> Cartons = new List<XElement>();
            List<BarcodeForBox> lisNewSSCC = GetSSCCForNewOrder(boxTotal);
            //Make a boxes for a order that has a new SSCC 
            foreach (BarcodeForBox item in lisNewSSCC)
            {
                XElement CartonForASN = new XElement(PICKPACKSTRUTURE,
                    new XElement(EDIHelperFunctions.Carton,
                    new XElement(Marks,
                    new XElement(UCC128, item.UC128)),
                    new XElement(Quantities,
                    new XElement(QtyQualifier, "ZZ"),
                    new XElement(QtyUOM, Each),
                    new XElement(Qty, QtyUOMSize))));
                Cartons.Add(CartonForASN);
            }

            return Cartons;
        }

        private List<BarcodeForBox> GetSSCCForNewOrder(int boxTotal)
        {
            List<BarcodeForBox> lisNewSSCC = new List<BarcodeForBox>();
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
                    BarcodeForBox cNewSSCC = new BarcodeForBox() { UC128 = sBarCode };
                    lisNewSSCC.Add(cNewSSCC);
                }
            }

            return lisNewSSCC;
        }


        XElement GetPick(SkuItem sku, int iCustomerLineNumber)
        {
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



        XElement BuildOrderTotals()
        {
            return new XElement(ORDERTOTALS,
                    new XElement(OrderTotalCases, GetShippingWeightOrBoxCount( ShippingInfoType.BoxCount   )),
                     new XElement(OrderTotalWeight, GetShippingWeightOrBoxCount( ShippingInfoType.ShippingWeight )));

        }
        private string GetPODate()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                Store cInbound850 = UoW.AddEDI850 .Find(t => t.PONumber  == PO)
                                                 .Where(x => x.OrderStoreNumber ==  Store )
                                                 .Where(x => x.ASNStatus  == (int)ASNStatus.ReadyForASN ).FirstOrDefault();
                if (cInbound850 != null)
                {
                    return cInbound850.PODate.ToString(toFormat);
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help,  ErrorCodes.HSAErro45));
                }
            }

        }


        private string GetDocID()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                Store store = UoW.AddEDI850.Find(t => t.PONumber  == PO)
                                           .Where(t => t.ASNStatus  == (int)ASNStatus.ReadyForASN )
                                           .Where(t => t.OrderStoreNumber == Store ).FirstOrDefault();
                if (!string.IsNullOrEmpty( store.DocumentId))
                {
                    return  store.DocumentId;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help,  ErrorCodes.HSAErro45));
                }
            }
            
        }



        private void SaveASN(XElement file)
        {
            var ASNNodes = file.Nodes().Last();
            using (XmlReader reader = ASNNodes.CreateReader())
            {
                reader.MoveToContent();
                reader.ReadToDescendant(UCC128); //Get SSCC code 
                string SSCC = reader.ReadElementContentAsString();


            }
        }

        #endregion
        #region Class functions 

        private List<Store> GetOrders()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                return  UoW.AddEDI850.Find(t => t.PONumber  == PO).Where(x => x.ASNStatus  == (int)ASNStatus.ReadyForASN )
                                                                    .Where(X => X.OrderStoreNumber == Store).ToList();
            }
            
        }

        public int GetShippingWeightOrBoxCount(ShippingInfoType ShippingInfo )
        {

            int dTotalWeight = 0;
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                List<Store> Stores = UoW.AddEDI850.Find(t => t.PONumber == PO)
                             .Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN)
                             .Where(t => t.OrderStoreNumber == Store).ToList();
                foreach (Store  item in Stores )
                {
                    dTotalWeight += item.QtyOrdered;
                }

                switch (ShippingInfo )
                {
                    case ShippingInfoType.PickCount:
                        return (dTotalWeight / InnersPerPacksSize);


                    case ShippingInfoType.ShippingWeight:
                        return (dTotalWeight / InnersPerPacksSize) + BOXWEIGHT;
                        

                    case ShippingInfoType.BoxCount:
                        return GetNumberofBoxes(dTotalWeight / InnersPerPacksSize);
                        
                    default:
                        return 0;
                        
                }
                
            }
            
        }

        private int GetNumberofBoxes(int packs )
        {

            int iCount = 0;
            if (packs  == 0)
            {
                return 0;
            }
            if (packs  <= EDIHelperFunctions.MINLBS || packs  <= (EDIHelperFunctions.MAXLBS -1 ))
            {
                iCount++;
                return iCount;

            }
            else
            {

                while (packs  >= (MAXLBS - 1 ))
                {
                    packs  = packs  - MAXLBS;
                    iCount++;
                }
                if (packs  >= 1)
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
                    List<Store> Stores = UoW.AddEDI850.Find(t => t.PONumber  == PO)
                                                        .Where(x => x.ASNStatus  == (int)ASNStatus.ReadyForASN )
                                                       .Where(t => t.OrderStoreNumber == Store ).ToList();
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

        private List<SkuItem> GetSKUInfo(List<Store> lisStores)
        {
            List<SkuItem> lisSku = new List<SkuItem>();
            SkuItem cSkuItem = null;

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
                            lisSku.Add(cSkuItem);
                        }
                    }
                    return lisSku;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAErro23));
                }

            }
        }



        private string  GetCustomerNumber()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                List<Store> lisStore = UoW.AddEDI850.GetAll().ToList();
                Store cEDI850 = UoW.AddEDI850.Find(t => t.PONumber == PO)
                                             .Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN)
                                             .Where(t => t.OrderStoreNumber == Store)
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
                                             .Where(t => t.OrderStoreNumber == Store)
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
