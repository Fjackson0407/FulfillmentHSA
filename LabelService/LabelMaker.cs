using Domain;
using EDIException;
using Helpers;
using Repository.DataSource;
using Repository.UOW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Helpers.EDIHelperFunctions;

namespace LabelService
{

    public class LabelMaker
    {
        /// <summary>
        /// Connection string to database 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// CSV file for lables 
        /// </summary>
        public string  Path { get; set; }
        public int InnersPerPacksSizeInt { get; private set; }

        public LabelMaker(string _ConnectionString, string _Path)
        {
            ConnectionString = _ConnectionString;
            Path = _Path;
        }

        public void GetAllOrders()
        {

            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                List<StoreInfoFromEDI850> _lisStores = UoW.AddEDI850.Find(t => t.ASNStatus == (int)ASNStatus.ReadyForASN).ToList();
                List<Label> lisLabels = LoadPO(_lisStores);  //Works 
                List<Label> lisDC = LoadDC(lisLabels); //DC Number
                List<Label> lisLabelsWithST = LoadShipFrom(lisDC); //From 
                List<Label> lisLabelsWithTO = AddTargetName(lisLabelsWithST); //Adxd target name for the label
                List<Label> LisStores = LoadStores(lisLabelsWithTO); //stoes 
                List<Label> lisDCAddress = LoadDCAndAddress(LisStores); //DC Number 
                List<Label> lisSSCC = LoadSSCCforLabels(lisDCAddress); //Get SSCC  
                string CSVFiile = ConvertToString(lisSSCC);
                UpdateOrders(_lisStores);
                SavetoFile(CSVFiile);

            }
        }

        private void UpdateOrders(List<StoreInfoFromEDI850> lisStores)
        {
            throw new NotImplementedException();
        }

        private void SavetoFile(string cSVFiile)
        {
            using (StreamWriter sw = new StreamWriter(Path))
            {
                sw.WriteLine(cSVFiile);
            }
        }

        private string ConvertToString(List<Label> lisSSCC)
        {
            StringBuilder cStringBuilder = new StringBuilder();
            cStringBuilder.Append(From);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Faddress);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Fcity);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Fstate);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Fzip);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(To);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Taddress);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Tcity);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Tstate);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Tzip);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(po);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(StoreID);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(EDIHelperFunctions.SSCC);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(DCLocation);
            cStringBuilder.Append(LineBreak);

            foreach (Label  label in lisSSCC )
            {
                cStringBuilder.Append(label.From);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Faddress);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Fcity);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Fstate);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.FZip);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.To);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Taddress);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Tcity);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Tstate);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Tzip);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.PONumber);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.OrderStoreNumber);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.SSCC);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.DcNumber);
                cStringBuilder.Append(LineBreak);

            }
            return cStringBuilder.ToString();
        }

        private List<Label> LoadSSCCforLabels(List<Label> lisDCAddress)
        {

            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                List<Label> Masterlabels = new List<Label>();
                foreach (Label item in lisDCAddress )
                {
                   //List<Label> NewList =       MakeSSCCforAStore(item);
                    //Masterlabels.AddRange(NewList);
                }
                return Masterlabels;
            }
                    
        }

        //private List<Label>  MakeSSCCforAStore(Label ParentLabel)
        //{
        //    List<Carton> lisCarton = new List<Domain.Carton>();
        //    List<Label> Lables = new List<Label>();

        //    using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
        //    {
        //        int iQty = 0;
                
        //        List<Store> lisOrderStore = UoW.AddEDI850.Find(t => t.OrderStoreNumber == ParentLabel.OrderStoreNumber)
        //                                                    .Where(x => x.PONumber == ParentLabel.PONumber).ToList();

        //        Carton _Carton = new Domain.Carton();
        //        _Carton = GetSSCCForNewOrder();
        //        ParentLabel.SSCC = _Carton.UCC128;
        //        Label Child = ParentLabel; 
        //        Lables.Add(ParentLabel);
        //        int CustomerLineNumber = 1;
        //        //hydrate Qty for alll stores 
        //        foreach (Store  _Store in lisOrderStore )
        //        {
        //            StoreOrderDetail cStoreOrderDetail = new StoreOrderDetail();
        //            iQty += _Store.QtyOrdered;
        //            int iCartonWeightWithIems = ((int)(VisaMasterCardBundleWeight * iQty ) / InnersPerPacksSizeInt) + 1;
        //            if (iCartonWeightWithIems >= (20 - 1))
        //            {
        //                lisCarton.Add(_Carton);
        //                _Carton = GetSSCCForNewOrder();
        //                iQty  = 0; //Reset totals for a new cartons
        //                iCartonWeightWithIems = 0;
        //                Child.SSCC = _Carton.UCC128;
        //                Lables.Add(Child);

        //            }
                        
        //                cStoreOrderDetail.Carton = _Carton;
        //                cStoreOrderDetail.CartonFK = _Carton.Id;
        //                cStoreOrderDetail.Id = Guid.NewGuid();
        //                cStoreOrderDetail.QtyOrdered =  _Store.QtyOrdered;
        //                SkuItem ItemDescription = GetItemDescription(_Store.UPCode);
        //                cStoreOrderDetail.CustomerLineNumber = CustomerLineNumber;
        //                CustomerLineNumber++;
        //                cStoreOrderDetail.ItemDescription = ItemDescription.Product;
        //                cStoreOrderDetail.DPCI = ItemDescription.DPCI;
        //                cStoreOrderDetail.UPC = ItemDescription.ProductUPC;
        //                cStoreOrderDetail.QtyPacked = 0;
        //                cStoreOrderDetail.SKUFK = ItemDescription.Id;
        //                _Carton.StoreOrderDetail.Add(cStoreOrderDetail);
        //                if (!lisCarton.Exists(t => t.Id == _Carton.Id))
        //                {
        //                    lisCarton.Add(_Carton);
        //                }
                    
        //            _Carton.Weight = iCartonWeightWithIems;
        //        }

        //    }
        //    SaveCarton(lisCarton);
        //    return Lables;   
        //}

        private Carton GetSSCCForNewOrder()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                return new Carton() { Id = Guid.NewGuid(), UCC128 = UoW.SSCCBarcode.GetNextSequenceNumber(SSCCStatus.NotUsed) };
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

        private SkuItem GetItemDescription(string uPCode)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                return UoW.Sku.Find(t => t.ProductUPC == uPCode.Replace(@"'", "")).FirstOrDefault();
            }
        }


        private List<Label> LoadDCAndAddress(List<Label> lisDC)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                foreach (Label  item in lisDC )
                {
                    DCInformation cDCInformation  =  UoW.DCInfo.Find(t => t.StoreID == item.DcNumber).FirstOrDefault();
                    if (cDCInformation == null)
                    {                        throw new ExceptionsEDI(string.Format("{0}  {1}", Help, ErrorCodes.HSAErro27));
                    }
                        item.Taddress = cDCInformation.Address;
                        item.Tcity = cDCInformation.City;
                        item.Tstate = cDCInformation.State;
                        item.Tzip = cDCInformation.PostalCode;
                   
                }
                

                return lisDC;
            }
        
        }

        /// <summary>
        /// Get DC number from PO
        /// </summary>
        /// <param name="lisStores"></param>
        /// <returns></returns>
        private List<Label> LoadDC(List<Label> lisStores)
        {
            foreach (Label  item in lisStores)
            {
                string PoNumber = item.PONumber;
                string[] poSplit = PoNumber.Split('-');
                if (string.IsNullOrEmpty(poSplit[2]))
                {
                    throw new ExceptionsEDI(string.Format("{0}  {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro51));
                }
                item.DcNumber = poSplit[2];

            }
            return lisStores;
        }

        private List<Label> AddTargetName(List<Label> lisLabels)
        {
            foreach (Label  item in lisLabels )
            {
                item.To = TargetStores; 
            }

            return lisLabels;
        }

        /// <summary>
        /// Get the Ship From Information
        /// </summary>
        /// <param name="lisStores"></param>
        /// <returns></returns>
        private List<Label> LoadShipFrom(List<Label> lisStores)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                ShipFromInformation cShipFromInformation = UoW.ShipFrom.Find(t => t.BillAndShipToCodes == ShipFrom).FirstOrDefault();
                if (cShipFromInformation == null)
                {
                    throw new ExceptionsEDI(string.Format("{0}  {1}", Help, ErrorCodes.HSAErro41));
                }
                foreach (Label   item in lisStores )
                {
                    item.From = ValidUSA;
                    item.Faddress = cShipFromInformation.Address;
                    item.Fcity = cShipFromInformation.City;
                    item.Fstate = cShipFromInformation.State;
                    item.FZip = cShipFromInformation.PostalCode;
                    

                }
                return lisStores ;
            }
        }

        private List<Label> LoadStores(List<Label> lisLabels)
        {
            
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                foreach (Label  item in lisLabels )
                {
                    string OrderStoreNumber =  UoW.AddEDI850.Find(t => t.PONumber == item.PONumber).FirstOrDefault().OrderStoreNumber;
                    if (string.IsNullOrEmpty(OrderStoreNumber ))
                    {
                        throw new ExceptionsEDI(string.Format("{0}  {1}", Help, ErrorCodes.HSAErro53));
                    }
                    item.OrderStoreNumber = OrderStoreNumber;
                }

            }
            return lisLabels; 
        }

        private List<Label> LoadPO(List<StoreInfoFromEDI850> lisStores)
        {
            List<Label> lisLables = new List<Label>();
            int count = 1; 
            foreach (StoreInfoFromEDI850  item in lisStores )
            {
                Label cLabel = new Label();
                cLabel.Count = count;
                cLabel.PONumber = item.PONumber;
                count++;
                lisLables.Add(cLabel);
            }

            return lisLables; 
        }
    }
}
