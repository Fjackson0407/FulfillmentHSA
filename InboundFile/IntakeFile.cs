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
using static Helpers.EDIHelperFunctions;

namespace EDIService
{
    public class EDIPOService
    {
        public string Path { get; set; }
        public string ConnectionString { get; set; }

        public EDIPOService(string _path, string _ConnectionString)
        {
            if (File.Exists(_path))
            {
                Path = _path;
                ConnectionString = _ConnectionString;
            }
            else
            {
                throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAError3));
            }
        }


        public void AddEdi850()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {

                List<Store> Invoice = ParseFile();
                UoW.AddEDI850.AddRage(Invoice);

                int Result = UoW.Complate();

            }
        }


        public void AddDC()
        {

            List<DCInformation> lisShippingInfo = new List<DCInformation>();

            using (CsvReader csv = new CsvReader(new StreamReader(Path), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))
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


            using (CsvReader csv = new CsvReader(new StreamReader(Path), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))

            {
                csv.SupportsMultiline = true;
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


        private List<Store> ParseFile()
        {

            List<Store> Invoice = new List<Store>();

            using (CsvReader csv = new CsvReader(new StreamReader(Path), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))
            {
                if (csv == null)
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAError5));
                }

                csv.SupportsMultiline = true;
                IDataReader reader = csv;
                Store cEDI850Domain = null;

                string sPO = string.Empty;

                if (reader == null)
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAErro7));
                }

                while (reader.Read())
                {
                    cEDI850Domain = new Store();
                    sPO = reader.GetValue((int)Inbound850Mapping.PONumber).ToString();
                    cEDI850Domain.Id = Guid.NewGuid();
                    cEDI850Domain.PONumber = sPO;
                    cEDI850Domain.PODate = reader.GetDateTime((int)Inbound850Mapping.PODate);
                    cEDI850Domain.DTS = DateTime.Now;
                    cEDI850Domain.PickStatus = (int) PickStatus.Open;
                    cEDI850Domain.QtyOrdered = reader.GetInt32((int)Inbound850Mapping.QtyOrdered);
                    cEDI850Domain.ASNStatus = (int) ASNStatus.ReadyForASN;
                    cEDI850Domain.UPCode = reader.GetValue((int)Inbound850Mapping.UPCCode).ToString();
                    cEDI850Domain.CustomerNumber = reader.GetValue((int)Inbound850Mapping.CustomerNumber).ToString();
                    cEDI850Domain.CompanyCode = reader.GetValue((int)Inbound850Mapping.CompanyCode).ToString();
                    cEDI850Domain.ShippingLocationNumber = reader.GetValue((int)Inbound850Mapping.LocationNumber).ToString();
                    cEDI850Domain.VendorNumber = reader.GetValue((int)Inbound850Mapping.VendorNumber).ToString();
                    cEDI850Domain.ShipDate = reader.GetDateTime((int)Inbound850Mapping.ShipDateOrder).ToString();
                    cEDI850Domain.CancelDate = reader.GetDateTime((int)Inbound850Mapping.CancelDate).ToString();
                    cEDI850Domain.DCNumber = reader.GetValue((int)Inbound850Mapping.ShipToAddress).ToString();
                    cEDI850Domain.CancelDate = reader.GetDateTime((int)Inbound850Mapping.CancelDate).ToString();
                    cEDI850Domain.OrderStoreNumber = reader.GetValue((int)Inbound850Mapping.OrderStoreNumber).ToString();
                    cEDI850Domain.BillToAddress = reader.GetValue((int)Inbound850Mapping.BillToAddress).ToString();
                    cEDI850Domain.DocumentId = reader.GetValue((int)Inbound850Mapping.DocumentId).ToString();
                    cEDI850Domain.OriginalLine = reader.GetValue((int)Inbound850Mapping.OriginalLine).ToString();
                    Invoice.Add(cEDI850Domain);
                }

            }
            return Invoice;
        }
        
    }
}
