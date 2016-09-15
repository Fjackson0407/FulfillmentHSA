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
    public class ReportsFromInboundFile
    {
        public string Path { get; set; }
        public string ConnectionString { get; set; }

        public ReportsFromInboundFile(string _Path, string _ConnectionString)
        {
            Path = _Path;
            ConnectionString = _ConnectionString;
        }

        private void BuildReport(List<StoreInfoFromEDI850ForReports> StoresForTheWeek)
        {
            var po =  StoresForTheWeek.Select(t => t.SubStringPO).Distinct();
            var sku = StoresForTheWeek.Select(t => t.SkuItem.DPCI).Distinct();
            
        }

        public void WeeklyReport()
        {
            using (CsvReader csv = new CsvReader(new StreamReader(Path), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))
            {

                csv.SupportsMultiline = true;
                IDataReader reader = csv;
                StoreInfoFromEDI850ForReports cStoreInfoFromEDI850ForReports = null;
                string sPO = string.Empty;
                List<StoreInfoFromEDI850ForReports> Invoice = new List<StoreInfoFromEDI850ForReports>();

                while (reader.Read())
                {
                    cStoreInfoFromEDI850ForReports = new StoreInfoFromEDI850ForReports();
                    sPO = reader.GetValue((int)Inbound850Mapping.PONumber).ToString();
                    cStoreInfoFromEDI850ForReports.Id = Guid.NewGuid();
                    cStoreInfoFromEDI850ForReports.PONumber = sPO.Replace(@"'", "");
                    cStoreInfoFromEDI850ForReports.PODate = reader.GetDateTime((int)Inbound850Mapping.PODate);
                    cStoreInfoFromEDI850ForReports.DTS = DateTime.Now;
                    cStoreInfoFromEDI850ForReports.ASNStatus = (int)ASNStatus.ReadyForASN;
                    cStoreInfoFromEDI850ForReports.PickStatus = 0;
                    cStoreInfoFromEDI850ForReports.QtyOrdered = reader.GetInt32((int)Inbound850Mapping.QtyOrdered);
                    cStoreInfoFromEDI850ForReports.UPCode = reader.GetValue((int)Inbound850Mapping.UPCCode).ToString().Replace(@"'", "");
                    CommonFunctions cCommonFunctions = new CommonFunctions(ConnectionString);
                    SkuItem cSkuItem = cCommonFunctions.GetSkuInfo(cStoreInfoFromEDI850ForReports.UPCode);
                    if (cSkuItem != null)
                    {

                        cStoreInfoFromEDI850ForReports.SkuItemFK = cSkuItem.Id;
                        cStoreInfoFromEDI850ForReports.SkuItem = cSkuItem;
                        cStoreInfoFromEDI850ForReports.CustomerNumber = reader.GetValue((int)Inbound850Mapping.CustomerNumber).ToString();
                        cStoreInfoFromEDI850ForReports.CompanyCode = reader.GetValue((int)Inbound850Mapping.CompanyCode).ToString();
                        if (cStoreInfoFromEDI850ForReports.CompanyCode == MasterVisaCard)
                        {
                            cStoreInfoFromEDI850ForReports.PkgWeight = 0.95;
                        }
                        else
                        {
                            cStoreInfoFromEDI850ForReports.PkgWeight = .70;
                        }
                        cStoreInfoFromEDI850ForReports.ShippingLocationNumber = reader.GetValue((int)Inbound850Mapping.LocationNumber).ToString();
                        cStoreInfoFromEDI850ForReports.VendorNumber = reader.GetValue((int)Inbound850Mapping.VendorNumber).ToString();
                        cStoreInfoFromEDI850ForReports.ShipDate = reader.GetDateTime((int)Inbound850Mapping.ShipDateOrder).ToString();
                        cStoreInfoFromEDI850ForReports.CancelDate = reader.GetDateTime((int)Inbound850Mapping.CancelDate).ToString();
                        cStoreInfoFromEDI850ForReports.DCNumber = reader.GetValue((int)Inbound850Mapping.ShipToAddress).ToString().Replace(@"'", "");
                        cStoreInfoFromEDI850ForReports.CancelDate = reader.GetDateTime((int)Inbound850Mapping.CancelDate).ToString();
                        cStoreInfoFromEDI850ForReports.OrderStoreNumber = reader.GetValue((int)Inbound850Mapping.OrderStoreNumber).ToString();
                        cStoreInfoFromEDI850ForReports.BillToAddress = reader.GetValue((int)Inbound850Mapping.BillToAddress).ToString().Replace(@"'", "");
                        cStoreInfoFromEDI850ForReports.DocumentId = reader.GetValue((int)Inbound850Mapping.DocumentId).ToString();
                        cStoreInfoFromEDI850ForReports.OriginalLine = reader.GetValue((int)Inbound850Mapping.OriginalLine).ToString();
                        Invoice.Add(cStoreInfoFromEDI850ForReports);
                    }
                }
                BuildReport(Invoice);
            }
        }
    }
}
