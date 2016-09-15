using Domain;
using EDIException;
using LumenWorks.Framework.IO.Csv;
using Repository.DataSource;
using Repository.UOW;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolService;
using static Helpers.EDIHelperFunctions;

namespace EDIService
{
    public class EDIPOService
    {
        public string FromFile { get; set; }
        public string ConnectionString { get; set; }

        public EDIPOService(string _FromPath, string _ConnectionString)
        {
            if (File.Exists(_FromPath))
            {
                FromFile = _FromPath;
                ConnectionString = _ConnectionString;
            }
            else
            {
                //Cisco, this should go to a log file or the console temporarily so it does not stop the process

                throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAError3));
            }
        }


        public List<StoreInfoFromEDI850> BuildReport()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                return  ParseFile();   
            }
        }
        
        public void ParseEDI850()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                List<StoreInfoFromEDI850> Invoice = ParseFile();
                UoW.AddEDI850.AddRage(Invoice);

                int Result = UoW.Complate();

            }
        }


        public void AddDC()
        {

            List<DCInformation> lisShippingInfo = new List<DCInformation>();

            using (CsvReader csv = new CsvReader(new StreamReader(FromFile), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))
            {

                csv.SupportsMultiline = true;
                IDataReader reader = csv;
                DCInformation cShippingInfo = null;
                string sPO = string.Empty;


                while (reader.Read())
                {
                    cShippingInfo = new DCInformation();
                    cShippingInfo.Id = Guid.NewGuid();
                    cShippingInfo.Address = reader.GetValue((int)DCAddressInfo.Address).ToString();
                    cShippingInfo.City = reader.GetValue((int)DCAddressInfo.City).ToString();
                    cShippingInfo.State = reader.GetValue((int)DCAddressInfo.State).ToString();
                    cShippingInfo.PostalCode = reader.GetValue((int)DCAddressInfo.PostalCode).ToString();
                    cShippingInfo.BillAndShipToCodes = reader.GetValue((int)DCAddressInfo.BillAndShipToCodes).ToString();
                    cShippingInfo.StoreID = reader.GetValue((int)DCAddressInfo.StoreID).ToString();
                    
                    
                    lisShippingInfo.Add(cShippingInfo);
                }
            }


            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                UoW.DCInfo.AddRage(lisShippingInfo);
                int Result = UoW.Complate();
            }


        }

        public void GetFromFile()
        {
            List<SkuItem> lisSku = new List<SkuItem>();

            //Cisco, interesting solution for the CSV reader...Im not sure I would have done it this way but it works.
            using (CsvReader csv = new CsvReader(new StreamReader(FromFile), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))

            {
                csv.SupportsMultiline = true;

                //Cisco, why the duplicate object, isn't csv already a datareader?  
                //Also, could this have been configured to use the headers and then map it to the object directly instead of the hard coded enum/index
                IDataReader reader = csv;
                SkuItem cSkuItem = null;
                string sPO = string.Empty;
                
                while (reader.Read())
                {
                    cSkuItem = new SkuItem();
                    cSkuItem.Id = Guid.NewGuid();
                    cSkuItem.DPCI = reader.GetValue((int)TargetProductMapping.DPCI).ToString();
                    cSkuItem.Brand = reader.GetValue((int)TargetProductMapping.Brand).ToString();
                    cSkuItem.Product = reader.GetValue((int)TargetProductMapping.Product).ToString();
                    cSkuItem.SubProduct = reader.GetValue((int)TargetProductMapping.SubProduct).ToString();
                    cSkuItem.DENOM = reader.GetValue((int)TargetProductMapping.DEMON).ToString();
                    cSkuItem.BIN = reader.GetValue((int)TargetProductMapping.Bin).ToString();
                    cSkuItem.GCCardType = reader.GetValue((int)TargetProductMapping.GcCardType).ToString();
                    cSkuItem.GCProdId = reader.GetValue((int)TargetProductMapping.GCProdId).ToString();
                    cSkuItem.DCMSID = reader.GetValue((int)TargetProductMapping.DCMSID).ToString();
                    cSkuItem.EmbossedLine = reader.GetValue((int)TargetProductMapping.EmbossedLine).ToString();
                    cSkuItem.DEPT = reader.GetValue((int)TargetProductMapping.DEPT).ToString();
                    cSkuItem.Class = reader.GetValue((int)TargetProductMapping.Class).ToString();
                    cSkuItem.Item = reader.GetValue((int)TargetProductMapping.Iten).ToString();
                    cSkuItem.ProductUPC = reader.GetValue((int)TargetProductMapping.ProductUPC).ToString();
                    cSkuItem.PackageUPC = reader.GetValue((int)TargetProductMapping.PackageUPC).ToString();

                    lisSku.Add(cSkuItem);

                }

            }
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                UoW.Sku.AddRage(lisSku);
                UoW.Complate();
            }

        }


        private List<StoreInfoFromEDI850> ParseFile()
        {

            List<StoreInfoFromEDI850> Invoice = new List<StoreInfoFromEDI850>();
             using (CsvReader csv = new CsvReader(new StreamReader(FromFile), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))
            {
                if (csv == null)
                {
                    //Cisco, csv will never be null but the contents may be.
                    throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAError5));
                }

                csv.SupportsMultiline = true;
                IDataReader reader = csv;
                StoreInfoFromEDI850 cEDI850Domain = null;

                string sPO = string.Empty;

                if (reader == null)
                {
                    //Cisco, reader will never be null but the contents may be.
                    throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAErro7));
                }

                while (reader.Read())
                {
                    cEDI850Domain = new StoreInfoFromEDI850();
                    sPO = reader.GetValue((int)Inbound850Mapping.PONumber).ToString();
                    cEDI850Domain.Id = Guid.NewGuid();
                    cEDI850Domain.PONumber = sPO.Replace(@"'", "");
                    cEDI850Domain.PODate = reader.GetDateTime((int)Inbound850Mapping.PODate);
                    cEDI850Domain.DTS = DateTime.Now;
                    cEDI850Domain.ASNStatus = (int) ASNStatus.ReadyForASN;
                    cEDI850Domain.PickStatus = 0;
                    cEDI850Domain.QtyOrdered = reader.GetInt32((int)Inbound850Mapping.QtyOrdered);
                    cEDI850Domain.UPCode = reader.GetValue((int)Inbound850Mapping.UPCCode).ToString().Replace(@"'", "");
                    CommonFunctions cCommonFunctions = new CommonFunctions(ConnectionString);
                    SkuItem cSkuItem = cCommonFunctions.GetSkuInfo(cEDI850Domain.UPCode);
                    if (cSkuItem != null)
                    {

                        cEDI850Domain.SkuItemFK = cSkuItem.Id;
                       // cEDI850Domain.SkuItem = cSkuItem;
                        cEDI850Domain.CustomerNumber = reader.GetValue((int)Inbound850Mapping.CustomerNumber).ToString();
                        cEDI850Domain.CompanyCode = reader.GetValue((int)Inbound850Mapping.CompanyCode).ToString();
                        if (cEDI850Domain.CompanyCode == "STO08")
                        {
                            cEDI850Domain.PkgWeight = 0.95;
                        }
                        else
                        {
                            cEDI850Domain.PkgWeight = .70;
                        }
                        cEDI850Domain.ShippingLocationNumber = reader.GetValue((int)Inbound850Mapping.LocationNumber).ToString();
                        cEDI850Domain.VendorNumber = reader.GetValue((int)Inbound850Mapping.VendorNumber).ToString();
                        cEDI850Domain.ShipDate = reader.GetDateTime((int)Inbound850Mapping.ShipDateOrder).ToString();
                        cEDI850Domain.CancelDate = reader.GetDateTime((int)Inbound850Mapping.CancelDate).ToString();
                        cEDI850Domain.DCNumber = reader.GetValue((int)Inbound850Mapping.ShipToAddress).ToString().Replace(@"'", "");
                        cEDI850Domain.CancelDate = reader.GetDateTime((int)Inbound850Mapping.CancelDate).ToString();
                        cEDI850Domain.OrderStoreNumber = reader.GetValue((int)Inbound850Mapping.OrderStoreNumber).ToString();
                        cEDI850Domain.BillToAddress = reader.GetValue((int)Inbound850Mapping.BillToAddress).ToString().Replace(@"'", "");
                        cEDI850Domain.DocumentId = reader.GetValue((int)Inbound850Mapping.DocumentId).ToString();
                        cEDI850Domain.OriginalLine = reader.GetValue((int)Inbound850Mapping.OriginalLine).ToString();
                        Invoice.Add(cEDI850Domain);
                    }
                }

            }
            return Invoice;
        }

        
    }
}
