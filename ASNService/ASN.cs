using Domain;
using EDIException;
using Helpers;
using Repository.DataSource;
using Repository.Shipping;
using Repository.UOW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static Helpers.EDIHelperFunctions;
using static ToolService.UpdateDatabas;

namespace ASNService
{
    public class ASNBuild
    {
        public string m_sPathForASNFile { get; set; }
        public string ConnectionString { get; set; }
        public string PO { get; set; }
        public string CurrentStore { get; set; }

        public Guid CurrentID { get; set; }
        public string CurrentDCNumber { get; private set; }
        public int NumberofCardsInBundle { get; private set; }

        public string m_CartonTypeForASN { get; private set; }

        public int m_MaxWeightForCarton { get; private set; }
        public int m_MinWeightForCarton { get; private set; }
        public string m_sCompanyCode { get; private set; }
        public int m_EmptyBoxWeight { get; set; }
        //string m_TempPath = @"D:\ASN Folder\ASN.xml";
        string m_TempPath = string.Empty;
        //string m_sRootPath = @"D:\ASN Results\";
        string m_sASNFolder = string.Empty;
        public ASNBuild(string _path, string _ConnectionString, string _PO, string _Store, string _DCNumber)
        {
            if (!NoASN(_ConnectionString, _PO, _DCNumber, _Store))
            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro55));
            }

            PO = _PO;
            m_sPathForASNFile = _path;
            ConnectionString = _ConnectionString;
            CurrentStore = _Store;
            CurrentDCNumber = _DCNumber;
            SetPackSize();
            SetCompanyCode();
            SetMaxCartonWeight();
            SetCartonType();
            SetEmptyBoxWeight();
            SetMinCartonWeight();
        }


        public ASNBuild(string ASNPath, string _TempPath, string _ConnectionString)
        {
            m_TempPath = _TempPath;
            m_sASNFolder = ASNPath;
            ConnectionString = _ConnectionString;
                
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                //Cisco, Im not a big fan of these wide open loops.  This could be refactored to get all the 'ReadyForASN' records and then process in foreach.
                for (;;)
                {
                    //DateTime mydate = DateTime.Parse("09/12/2016"); for testing only 
                    StoreInfoFromEDI850 cStore = UoW.AddEDI850.Find(t => t.ASNStatus == (int)ASNStatus.ReadyForASN ).FirstOrDefault();
                    if (cStore == null)
                    {
                        break;
                    }
                    PO = cStore.PONumber;
                    CurrentDCNumber = cStore.DCNumber;
                    CurrentStore = cStore.OrderStoreNumber;
                    SetmemberVaribles();
                    BuildASN();
                }
            }
        }

        private void SetmemberVaribles()
        {
            SetPackSize();
            SetMaxCartonWeight();
            SetCartonType();
            SetCompanyCode();
            SetEmptyBoxWeight();
            SetMinCartonWeight();
            m_sPathForASNFile = string.Format("{0}ASN Store {1} for PO {2} {3}.xml", m_sASNFolder, CurrentStore, PO, GetNewShipmentID());
        }

        //Cisco, this method relies on specific data organization in the DB
        private void SetMinCartonWeight()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                m_MinWeightForCarton = UoW.MinWeightShipBox.Find(t => t.InUse == true).FirstOrDefault().MinWeight;
            }
        }

        //Cisco, this method relies on specific data organization in the DB
        private void SetEmptyBoxWeight()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                m_EmptyBoxWeight = UoW.EmptyBox.Find(t => t.InUse == true).FirstOrDefault().Weight;
            }
        }

        //Cisco, this method relies on specific data organization in the DB
        private void SetCartonType()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                m_CartonTypeForASN = UoW.CartonType.Find(t => t.InUse == true).FirstOrDefault().Type;
            }
        }

        //Cisco, this method relies on specific data organization in the DB
        private void SetMaxCartonWeight()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                m_MaxWeightForCarton = UoW.MaxCartonWeight.Find(t => t.InUse == true).FirstOrDefault().Max;
            }
        }

        //Cisco, this method relies on specific data organization in the DB
        private void SetPackSize()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                NumberofCardsInBundle = UoW.PackSizeForBundles.Find(t => t.InUse == true).FirstOrDefault().Size;

            }

        }

        /// <summary>
        /// Check to see if the po has a ASN if not then process the order 
        /// </summary>
        /// <param name="_ConnectionString"></param>
        /// <param name="_PO"></param>
        /// <param name="_DCNumber"></param>
        /// <param name="_Store"></param>
        /// <returns></returns>
        private bool NoASN(string _ConnectionString, string _PO, string _DCNumber, string _Store)
        {
            using (var UoW = new UnitofWork(new EDIContext(_ConnectionString)))
            {
                StoreInfoFromEDI850 cStore = UoW.AddEDI850.Find(t => t.PONumber == _PO).Where(x => x.ASNStatus == (int)ASNStatus.ReadyForASN)
                                                                    .Where(X => X.OrderStoreNumber == _Store).FirstOrDefault();

                var g = cStore.Carton;
                if (cStore == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }


        #region Xml code 
        XElement BuildHeader()
        {
            return new XElement(HEADER,
                new XElement(CompanyCode, m_sCompanyCode),
                new XElement(CUSTOMERNUMBER, GetCustomerNumber()),
                new XElement(Direction, EDIHelperFunctions.Outbound),
                new XElement(DocumentType, EDIHelperFunctions.EDI856),
               new XElement(EDIHelperFunctions.Version, VersionNumber),
              new XElement(EDIHelperFunctions.Footprint, ASN),
              new XElement(ShipmentID, GetNewShipmentID()),
             new XElement(EDIHelperFunctions.ShippingLocationNumber, new XCData(CurrentStore)));

        }


        XElement BuildShipmentLevel()
        {

            return new XElement(EDIHelperFunctions.ShipmentLevel,
       new XElement(TransactionSetPurposeCode, "00"),
                    new XElement(EDIHelperFunctions.DateLoop,
                    new XElement(EDIHelperFunctions.DateQualifier, ShipDateDateQualifierNumber,
                    new XAttribute(EDIHelperFunctions.Desc,
                    EDIHelperFunctions.ShipDateString)),
                    new XElement(EDIHelperFunctions.Date,  DateTime.Now.ToString(toFormat)  )),
                    new XElement(EDIHelperFunctions.ManifestCreateTime,
                     GetMilitaryTime()),
                    new XElement(EDIHelperFunctions.ShipmentTotals,
                    new XElement(EDIHelperFunctions.ShipmentTotalCube, ShipmentTotalCubeValue),
                    new XElement(EDIHelperFunctions.ShipmentPackagingCode, m_CartonTypeForASN),
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

        private string GetMilitaryTime()
        {
            return DateTime.Now.ToString("HH:mm");
        }
        private XElement BuildCarrier()
        {
            return new XElement(Carrier,
                new XElement(CarrierCode, new XCData(NFIL)),
             new XElement(CarrierType, CarrierTypeVaule));


        }

        private XElement BuildBOL()
        {
            int BOLNumber = GetNewBOLID();
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                StoreInfoFromEDI850 cStore = UoW.AddEDI850.Find(t => t.PONumber == PO)
                                                           .Where(t => t.OrderStoreNumber == CurrentStore).FirstOrDefault();
                    
                BOLForASN cBill = new BOLForASN();
                cBill.ID = Guid.NewGuid();
                cBill.StoreInfoFK = cStore.Id;
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
                     new XElement(EDIHelperFunctions.FunctionCode, new XCData(EDIHelperFunctions.CUST)),
                     new XElement(EDIHelperFunctions.ContactName, new XCData(cContactInfo.FullName)),
                     new XElement(EDIHelperFunctions.ContactQualifier, EDIHelperFunctions.Phone),
                     new XElement(EDIHelperFunctions.PhoneEmail, new XCData(cContactInfo.PhoneNumber)),
                     new XElement(EDIHelperFunctions.ContactQualifier, EDIHelperFunctions.Email),
                     new XElement(EDIHelperFunctions.PhoneEmail, new XCData(cContactInfo.EmailAddress)));


        }


        XElement BuildShipFrom()
        {
            ShipFromInformation ShipFrom = GetShipFromAddress();
            return new XElement(NAME, new XElement(BillAndShipToCode, EDIHelperFunctions.ShipFrom),
                new XElement(EDIHelperFunctions.DUNSOrLocationNumberString, ShipFrom.DUNSOrLocationNumber),
                new XElement(EDIHelperFunctions.NameComponentQualifier, EDIHelperFunctions.DescShipFrom),
                new XElement(EDIHelperFunctions.NameComponent, new XCData(EDIHelperFunctions.ValidName)),
                new XElement(EDIHelperFunctions.CompanyName, new XCData(EDIHelperFunctions.ValidName)),
                new XElement(EDIHelperFunctions.Address, new XCData(ShipFrom.Address)),
                new XElement(EDIHelperFunctions.City, new XCData(ShipFrom.City)),
                new XElement(EDIHelperFunctions.State, ShipFrom.State),
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
                    throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAErro41));
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
                    new XElement(EDIHelperFunctions.NameComponent, new XCData(string.Format("{0}    {1}", EDIHelperFunctions.SHIPPREDISTROTODC, DCNumber))),
                    new XElement(EDIHelperFunctions.CompanyName, new XCData(string.Format("{0}    {1}", EDIHelperFunctions.SHIPPREDISTROTODC, DCNumber))),
                    new XElement(EDIHelperFunctions.Address, new XCData(ShipTo.Address)),
                    new XElement(EDIHelperFunctions.City, new XCData(ShipTo.City)),
                    new XElement(EDIHelperFunctions.State, ShipTo.State),
                    new XElement(EDIHelperFunctions.Zip, ShipTo.PostalCode),
                    new XElement(EDIHelperFunctions.Country, EDIHelperFunctions.US));

        }
        /// <summary>
        /// Build the 850 to send back to EZcom 
        /// </summary>
        public void BuildASN()
        {
            XElement  File = new XElement(FILE,
                      new XElement(EDIHelperFunctions.DOCUMENT, BuildHeader(), BuildShipmentLevel(), BuildOrderLevel()));

            SaveDataToFile(File);
            SaveASN(File);
       
        }


        private void SaveDataToFile(XElement file)
        {

            var Setting = new XmlWriterSettings();
            Setting.Indent = true;
            Setting.NewLineOnAttributes = true;
            Setting.Encoding = Encoding.UTF8;
            Setting.WriteEndDocumentOnClose = true;

            StreamWriter cStreamWriter = File.CreateText(m_sPathForASNFile);

            using (XmlWriter cXmlWriter = XmlWriter.Create(cStreamWriter, Setting))
            {
              
                file.WriteTo(cXmlWriter);
              
            };
             
        }
        private void SaveASN(XElement file)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                List<StoreInfoFromEDI850> lisStores = UoW.AddEDI850.Find(t => t.PONumber == PO).Where(x => x.ASNStatus == (int)ASNStatus.ReadyForASN)
                                                                    .Where(X => X.OrderStoreNumber == CurrentStore).ToList();

                ASNFileOutBound cASNFileOutBound = new ASNFileOutBound();
                cASNFileOutBound.Id = Guid.NewGuid();
                cASNFileOutBound.File = file.ToString();
                cASNFileOutBound.DTS = DateTime.Now;

                lisStores.ForEach(s => s.ASNStatus = (int)ASNStatus.HasASN);
                lisStores.ForEach(s => s.PickStatus = (int)EOrderStatus.Open);
                UoW.AddEDI850.SaveChange();
                foreach (StoreInfoFromEDI850 item in lisStores)
                {
                    cASNFileOutBound.Store.Add(item);
                }
                UoW.ASNFile.Add(cASNFileOutBound);
                int Result = UoW.Complate();
            }
        }


        /// <summary>
        /// ths is for order detail 
        /// </summary>
        /// <returns></returns>
        private XElement BuildOrderLevel()
        {
            return new XElement(ORDERLEVEL,
                             new XElement(IDS,
                             new XElement(EDIHelperFunctions.PurchaseOrderNumber, new XCData(PO)),
                             new XElement(EDIHelperFunctions.PURCHASEORDERSOURCEID, GetDocID()),
                             new XElement(EDIHelperFunctions.PurchaseOrderDate, GetPODate()),
                             new XElement(EDIHelperFunctions.StoreNumber, new XCData(CurrentStore)),
                             new XElement(EDIHelperFunctions.DepartmentNumberString, EDIHelperFunctions.DepartmentNumber),
                             new XElement(EDIHelperFunctions.DivisionNumberString, EDIHelperFunctions.DivisionNumber),
                             new XElement(EDIHelperFunctions.ReleaseNumberString, EDIHelperFunctions.ReleaseNumber)),
                             BuildOrderTotals(), BuildPickStruture());

        }

        XElement BuildPickStruture()
        {
            List<StoreInfoFromEDI850> Orders = GetOrders();
            if (Orders.Count == 0)
            {
                throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAErro59));
            }
            List<Carton> Cartons = BuildPickPackStructure(Orders);
            ConvertToxml(Cartons);
           return  PickAndPackElement();
         
        }


        XElement PickAndPackElement()
        {
            using (FileStream cFileStream = File.OpenRead(m_TempPath))
            {
                XDocument doc = XDocument.Load(cFileStream);
                return new XElement(doc.Root);
            }
        }


        private void ConvertToxml(List<Carton> lisCarton)
        {

             var Setting = new XmlWriterSettings();
            Setting.Indent = true;
            Setting.NewLineOnAttributes = true;
            Setting.Encoding = Encoding.UTF8;
            Setting.WriteEndDocumentOnClose = true;

            StreamWriter cStreamWriter = File.CreateText(m_TempPath);

            using (XmlWriter cXmlWriter = XmlWriter.Create(cStreamWriter, Setting))
            {
                cXmlWriter.WriteStartDocument();
                cXmlWriter.WriteStartElement(EDIHelperFunctions.PickPackStructure);
                foreach (Carton cCarton in lisCarton)
                {
                    cXmlWriter.WriteStartElement(EDIHelperFunctions.Carton);
                    cXmlWriter.WriteStartElement(MARKS);
                    cXmlWriter.WriteStartElement(UCC128);
                    cXmlWriter.WriteCData(cCarton.UCC128);
                    cXmlWriter.WriteEndElement(); //End of UCC128
                    cXmlWriter.WriteEndElement(); //End of Mark
                    cXmlWriter.WriteStartElement(Quantities);
                    cXmlWriter.WriteElementString(QtyQualifier, "ZZ");
                    cXmlWriter.WriteElementString(QtyUOM, "ZZ");
                    cXmlWriter.WriteElementString(Qty, cCarton.Qty.ToString());
                    cXmlWriter.WriteEndElement();
                    cXmlWriter.WriteStartElement(Measurement);
                    cXmlWriter.WriteElementString(MeasureQual, "WT");
                    cXmlWriter.WriteElementString(MeasureValue, "0.95");
                    cXmlWriter.WriteEndElement(); //Close  Measurement tag 
                    cXmlWriter.WriteEndElement(); //Close carton tag


                    
                    List<StoreInfoFromEDI850> Stores = GetStoreItmesForCartons();
                    foreach (StoreInfoFromEDI850 Store in Stores)
                    {
                        SkuItem cSkuItem = GetSkuInfo(Store);
                        cXmlWriter.WriteStartElement(ITEM);
                        cXmlWriter.WriteElementString(CustomerLineNumber, Store.CustomerLineNumber.ToString());
                        cXmlWriter.WriteStartElement(ItemIDs);
                        cXmlWriter.WriteElementString(IdQualifier, UP);
                        cXmlWriter.WriteStartElement(ID);
                        cXmlWriter.WriteCData(Store.UPCode);
                        cXmlWriter.WriteEndElement(); //close tag for upc
                        cXmlWriter.WriteEndElement(); //end of item for upc

                        cXmlWriter.WriteStartElement(ItemIDs);
                        cXmlWriter.WriteElementString(IdQualifier, VN);
                        cXmlWriter.WriteStartElement(Id);
                        cXmlWriter.WriteCData(GetCompanyCode());
                        cXmlWriter.WriteEndElement(); //close VN tag
                        cXmlWriter.WriteEndElement(); //end of item for vn 

                        cXmlWriter.WriteStartElement(ItemIDs);
                        cXmlWriter.WriteElementString(IdQualifier, CU);
                        cXmlWriter.WriteStartElement(Id);
                        cXmlWriter.WriteCData(cSkuItem.DPCI);
                        cXmlWriter.WriteEndElement(); //Close cu tag 
                        cXmlWriter.WriteEndElement(); //Close itemids 
                        cXmlWriter.WriteStartElement(Quantities);
                        cXmlWriter.WriteElementString(QtyQualifier, "39");
                        cXmlWriter.WriteElementString(QtyUOM, Each);
                        cXmlWriter.WriteElementString(Qty, Store.QtyOrdered.ToString());
                        cXmlWriter.WriteEndElement(); //Quantities

                        cXmlWriter.WriteElementString(PackSize, QtyUOMSize);
                        cXmlWriter.WriteElementString(Inners, one);
                        cXmlWriter.WriteElementString(EachesPerInner, one);
                        cXmlWriter.WriteElementString(InnersPerPacks, NumberofCardsInBundle.ToString());
                        cXmlWriter.WriteStartElement(Measurement);
                        cXmlWriter.WriteElementString(MeasureQual, "WT");
                        cXmlWriter.WriteElementString(MeasureValue, "0.038");
                        cXmlWriter.WriteEndElement(); //close tag for Measurement


                        
                        cXmlWriter.WriteStartElement(ItemDescription);
                        cXmlWriter.WriteCData(cSkuItem.Product.Trim());
                        cXmlWriter.WriteEndElement();
                        cXmlWriter.WriteEndElement();
                    }

                }
                cXmlWriter.WriteEndElement(); //End of PickPackStructure
                cXmlWriter.Close();
                cStreamWriter.Close();
            }

        }

        private string GetCompanyCode()
        {
            if (!string.IsNullOrEmpty(m_sCompanyCode))
            {
                if (m_sCompanyCode == "CER05")
                {
                    return ASNVNSettingAMEX;
                }
                else
                {
                    return ASNVNSettingMC;
                }
            }
            return string.Empty;
        }

        private SkuItem GetSkuInfo(StoreInfoFromEDI850 store)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                return UoW.Sku.Find(t => t.Id == store.SkuItemFK).FirstOrDefault();
            }
        }

        
        private List<StoreInfoFromEDI850> GetStoreItmesForCartons()
        {
            List<StoreInfoFromEDI850> Stores = new List<StoreInfoFromEDI850>();
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                return UoW.AddEDI850.Find(t => t.PONumber == PO).Where(x => x.ASNStatus == (int)ASNStatus.ReadyForASN)   
                                                                .Where(X => X.OrderStoreNumber == CurrentStore).ToList();
            }

        }


        /// <summary>
        /// Get all the stores for a given carton 
        /// </summary>
        /// <param name="Test"></param>
        /// <param name="StoreFK"></param>
        /// <returns></returns>
        private IEnumerable<StoreInfoFromEDI850> GetStoreItmesForCartons( bool Test, Guid StoreFK )
        {
            List<StoreInfoFromEDI850> Stores = new List<StoreInfoFromEDI850>();
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                var temp =  UoW._Cartons.Find(t => t.StoreNumberFK == StoreFK).ToList();
                foreach (var item in temp )
                {
                    Stores.Add(UoW.AddEDI850.Find(t => t.Id == item.Id).FirstOrDefault());
                }
            }

            return Stores;
        }




        /// <summary>
        /// Fill carton with sku items
        /// </summary>
        /// <param name="lisStore"></param>
        /// <returns></returns>
        private List<Carton> BuildPickPackStructure(List<StoreInfoFromEDI850> lisStore)
        {
            int MaxWeightForBox = GetMaxWeight();
            return FillBox(lisStore, MaxWeightForBox);
        }

        /// <summary>
        /// Fill cartons based on the number of order items 
        /// </summary>
        /// <param name="lisStore"></param>
        /// <param name="MaxWeght"></param>
        /// <returns></returns>
        private List<Carton> FillBox(List<StoreInfoFromEDI850> lisStore, int MaxWeght)
        {
            List<Carton> lisCarton = new List<Domain.Carton>();
            Carton cCarton = new Domain.Carton();
            bool bNewCarton = false;
            int CustomerLineNumber = 1;
            cCarton = GetSSCCForNewOrder();
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                foreach (StoreInfoFromEDI850 cStore in lisStore)
                {
                    var temp = UoW.AddEDI850.Find(t => t.Id == cStore.Id).FirstOrDefault();
                    if (temp != null)
                    {


                        int OrderWeight = (temp.QtyOrdered / NumberofCardsInBundle);
                        int CurrentWeight = 0;
                        SkuItem ItemDescription = GetItemDescription(temp.UPCode);
                        //Set the weight for the carton 
                        if (OrderWeight <= MaxWeght)
                        {
                            cCarton.Qty += temp.QtyOrdered;
                            CurrentWeight = OrderWeight;
                        }
                        else
                        {
                            cCarton.Qty = MaxWeght;
                            CurrentWeight = MaxWeght;
                        }

                        while (OrderWeight > 0) //Keep adding items to box untill QtyOrdered = 0
                        {
                            if (bNewCarton)
                            {
                                cCarton.StoreNumberFK = temp.Id;
                                cCarton.StoreNumber = temp;
                                temp.Carton.Add(cCarton);
                                lisCarton.Add(cCarton);
                                cCarton = GetSSCCForNewOrder();
                                cCarton.Qty += CurrentWeight;
                                bNewCarton = false;
                                CustomerLineNumber = 1;
                            }
                            cCarton.Weight += CurrentWeight;
                            temp.CustomerLineNumber = CustomerLineNumber;
                            temp.QtyPacked = 0;
                       
                            CustomerLineNumber++;
                            if (OrderWeight >= MaxWeght)
                            {
                                OrderWeight -= MaxWeght;
                                bNewCarton = true;
                                CurrentWeight = OrderWeight;
                            }
                            else
                            {
                                OrderWeight -= CurrentWeight;

                            }


                        }
                        cCarton.StoreNumberFK = temp.Id;
                        cCarton.StoreNumber = temp;

                        temp.Carton.Add(cCarton);
                        int Result = UoW.AddEDI850.SaveChange();
                    }


                }
            }

            lisCarton.Add(cCarton);
            return lisCarton;
        }
        /// <summary>
        /// This function will get the weight for a box
        /// </summary>
        /// <returns></returns>
        private int GetMaxWeight()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                return UoW.MaxCartonWeight.Find(t => t.InUse == true).FirstOrDefault().Max;
            }

        }

        private SkuItem GetItemDescription(string uPCode)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                return UoW.Sku.Find(t => t.ProductUPC == uPCode.Replace(@"'", "")).FirstOrDefault();
            }
        }

        XElement BuildOrderTotals()
        {
            var weight = GetShippingWeightOrBoxCount(ShippingInfoType.ShippingWeight);
            var boxcount = GetShippingWeightOrBoxCount(ShippingInfoType.BoxCount);
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
                StoreInfoFromEDI850 _Store = UoW.AddEDI850.Find(t => t.OrderStoreNumber == CurrentStore).FirstOrDefault();

                if (!string.IsNullOrEmpty(_Store.VendorNumber))
                {
                    return _Store.VendorNumber;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro17));
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
                    StoreInfoFromEDI850 lisInbound850 = UoW.AddEDI850.Find(t => t.PONumber == PO).FirstOrDefault();
                    if (!string.IsNullOrEmpty(lisInbound850.DCNumber))
                    {
                        sStoreNumber = lisInbound850.DCNumber.Replace(@"'", "");
                    }
                    else
                    {
                        throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro27));
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

                string sTempStore = CurrentDCNumber.Replace(@"'", "");

                cDCInformation = UoW.DCInfo.Find(t => t.StoreID == sTempStore).FirstOrDefault();

                if (cDCInformation != null)
                {
                    return cDCInformation;
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro41));
                }
            }
        }

        private string GetPODate()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                StoreInfoFromEDI850 cInbound850 = UoW.AddEDI850.Find(t => t.PONumber == PO)
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
                StoreInfoFromEDI850 store = UoW.AddEDI850.Find(t => t.PONumber == PO)
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

        private List<StoreInfoFromEDI850> GetOrders()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                return UoW.AddEDI850.Find(t => t.PONumber == PO).Where(x => x.ASNStatus == (int)ASNStatus.ReadyForASN)
                                                                    .Where(X => X.OrderStoreNumber == CurrentStore).ToList();
            }
        }

        public double GetShippingWeightOrBoxCount(ShippingInfoType ShippingInfo)
        {

            double dTotalWeight = 0;
            double BundleWeight = 0;
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                List<StoreInfoFromEDI850> Stores = UoW.AddEDI850.Find(t => t.PONumber == PO)
                              .Where(x => x.OrderStoreNumber == CurrentStore)
                              .Distinct()
                              .ToList();


                BundleWeight = Stores.FirstOrDefault().PkgWeight;
                foreach (StoreInfoFromEDI850 item in Stores)
                {
                    dTotalWeight += item.QtyOrdered;

                }
                double SubTotal = dTotalWeight / NumberofCardsInBundle;
                SubTotal *= BundleWeight;

                switch (ShippingInfo)
                {
                    case ShippingInfoType.PickCount:

                        return (SubTotal / (double)NumberofCardsInBundle);


                    case ShippingInfoType.ShippingWeight:

                        return (SubTotal / NumberofCardsInBundle) + m_EmptyBoxWeight;


                    case ShippingInfoType.BoxCount:

                        return GetNumberofBoxes(SubTotal / (double)NumberofCardsInBundle);

                    default:
                        return 0;

                }

            }



        }

        private int GetNumberofBoxes(double packs)
        {

            int iCount = 0;
            if (packs == 0)
            {
                return 0;
            }
            if (packs <= m_MinWeightForCarton || packs <= (m_MaxWeightForCarton - 1))
            {
                iCount++;
                return iCount;

            }
            else
            {

                while (packs >= (m_MaxWeightForCarton - 1))
                {
                    packs = packs - m_MaxWeightForCarton;
                    iCount++;
                }
                if (packs >= 1)
                {
                    iCount++;
                }
                return iCount;

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



        private string GetCustomerNumber()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                StoreInfoFromEDI850 cEDI850 = UoW.AddEDI850.Find(t => t.PONumber == PO)
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


        public void SetCompanyCode()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                StoreInfoFromEDI850 cEDI850 = UoW.AddEDI850.Find(t => t.PONumber == PO)
                                             .Where(t => t.ASNStatus == (int)ASNStatus.ReadyForASN)
                                             .Where(t => t.OrderStoreNumber == CurrentStore)
                                             .FirstOrDefault();
                if (cEDI850 != null)
                {
                    m_sCompanyCode = cEDI850.CompanyCode;
                }

                else
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAErro13));
                }
            }

        }
        #endregion



    }
}
