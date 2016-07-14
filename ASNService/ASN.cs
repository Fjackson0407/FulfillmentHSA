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
                new XElement(CUSTOMERNUMBER, Store),
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
                                new XElement(EDIHelperFunctions.DateQualifier, 011,
                                new XAttribute(EDIHelperFunctions.Desc,
                                EDIHelperFunctions.ShipDateString)),
                                new XElement(EDIHelperFunctions.Date, ShipDate)),
                                new XElement(EDIHelperFunctions.ManifestCreateTime,
                                DateTime.Now.ToLongTimeString()),
                                new XElement(EDIHelperFunctions.ShipmentTotals,
                                new XElement(EDIHelperFunctions.ShipmentTotalCube, ShipmentTotalCubeValue),
                                new XElement(EDIHelperFunctions.ShipmentPackagingCode, EDIHelperFunctions.CartonType),
                                new XElement(ShipmentTotalCases, NumberOfBoxesForOrder()),
                                new XElement(ShipmentTotalWeight, GetWeight)),
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
                     new XElement(EDIHelperFunctions.EMAILADDRESS, cContactInfo.EmailAddress));


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
                cAddreessDomain = UoW.ShipFrom.Find(t => t.BillAndShipToCodes == "ShipFrom").FirstOrDefault();
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
                    new XElement(EDIHelperFunctions.DUNSOrLocationNumberString, Store ),
                    new XElement(EDIHelperFunctions.NameComponentQualifier, EDIHelperFunctions.DescShipTo),
                    new XElement(EDIHelperFunctions.CompanyName, string.Format("{0} {1}", EDIHelperFunctions.SHIPPREDISTROTODC, DCNumber)),
                    new XElement(EDIHelperFunctions.Address, ShipTo.Address),
                    new XElement(EDIHelperFunctions.City, ShipTo.City),
                    new XElement(EDIHelperFunctions.State, ShipTo.State),
                    new XElement(EDIHelperFunctions.Zip, ShipTo.PostalCode));

        }

        private DCInformation GetShipToAddress()
        {
            throw new NotImplementedException();
        }

        public void BuildASN()
        {
            var File = new XElement(FILE,
                new XElement(EDIHelperFunctions.DOCUMENT,
                BuildHeader(), BuildShipmentLevel(), BuildOrderLevel()));
            SaveASN(File);
            File.Save(path);
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
            int BoxTotal = NumberOfBoxesForOrder();
            int Packtotal = NumberOfPickesForOrder();
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
            throw new NotImplementedException();
        }

        private List<SkuItem> GetSKUInfo(List<Store> cInbound850Current)
        {
            throw new NotImplementedException();
        }

        private int NumberOfPickesForOrder()
        {
            throw new NotImplementedException();
        }

        private List<Store> GetOrders()
        {
            throw new NotImplementedException();
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
                    new XElement(OrderTotalCases, NumberOfBoxesForOrder()),
                     new XElement(OrderTotalWeight, GetWeight));

        }
        private string GetPODate()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                Store cInbound850 = UoW.AddEDI850 .Find(t => t.PONumber  == PO).Where(x => x.OrderStoreNumber ==  Store )
                                                                         .Where(x => x.ASNStatus  == (int)ASNStatus.ReadyForASN ).FirstOrDefault();
                if (cInbound850 != null)
                {
                    return cInbound850.PODate.ToString(toFormat);
                }
                else
                {
                    throw new Exception();
                }
            }

        }


        private string GetDocID()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                Store store = UoW.AddEDI850.Find(t => t.PONumber  == PO).Where(t => t.ASNStatus  == (int)ASNStatus.ReadyForASN )
                                                                            .Where(t => t.OrderStoreNumber == Store ).FirstOrDefault();
                if (!string.IsNullOrEmpty( store.DocumentId))
                {
                    return  store.DocumentId;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0}{1}{2}", EDIHelperFunctions.Help, EDIHelperFunctions.Space, ErrorCodes.HSAErro45));
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
        public int NumberOfBoxesForOrder()
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
            }
            return GetNumberofBoxes(dTotalWeight / InnersPerPacksSize);
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
        public string GetCompanyCode()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                Store cEDI850 = UoW.AddEDI850.Find(t => t.PONumber == PO)
                                             .Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN)
                                             .Where(t => t.OrderStoreNumber == Store).FirstOrDefault();
                if (cEDI850 != null)
                {
                    return cEDI850.CompanyCode;
                }

                throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAErro13));
            }

        }


        #endregion

    }
}
