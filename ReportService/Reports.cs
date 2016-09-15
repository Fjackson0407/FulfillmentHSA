using Domain;
using EDIService;
using Helpers;
using Repository.DataSource;
using Repository.UOW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService
{
    public class Reports
    {
        public string ConnectionString { get; private set; }
        public string Path { get; set; }
        public string OutputFile { get; set; }
        public Reports(string _ConnectionString, string _Path, string _OutputFile)
        {
            ConnectionString = _ConnectionString;
            Path = _Path;
            OutputFile = _OutputFile;
        }


        /// <summary>
        /// Use the data from the raw file via Ezcom before it goes to the database
        /// </summary>
        public void InventorySummaryFromFile()
        {
            SummaryForInverntory cSummaryForInverntory;
            List<SummaryForInverntory> lisSummaryForInverntory = new List<SummaryForInverntory>();

            EDIPOService cEDIPOService = new EDIPOService(Path, ConnectionString);
            List<StoreInfoFromEDI850> lisStore = cEDIPOService.BuildReport();

            foreach (var store in lisStore)
            {
                cSummaryForInverntory = new SummaryForInverntory();
                cSummaryForInverntory.Store = store.OrderStoreNumber;
                cSummaryForInverntory.PO = store.PONumber;
                cSummaryForInverntory.UPC = store.UPCode;
                cSummaryForInverntory.DPCIFull = store.SkuItem.DPCI;
                cSummaryForInverntory.DPCI = store.SkuItem.DPCI.Substring(store.SkuItem.DPCI.Length - 2, 2);
                cSummaryForInverntory.Pkgs = store.QtyOrdered / 25;
                cSummaryForInverntory.QTYCards = store.QtyOrdered; 
                lisSummaryForInverntory.Add(cSummaryForInverntory);
            }
            SaveListToCSV(lisSummaryForInverntory);
        }

        public void InventorySummaryFromDatabase(DateTime StartDate, DateTime EndDate)
        {
            SummaryForInverntory cSummaryForInverntory;
            List<SummaryForInverntory> lisSummaryForInverntory = new List<SummaryForInverntory>();

            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                var stores = UoW.AddEDI850.Find(x => x.PODate >= StartDate && x.PODate <= EndDate);
                foreach (var store in stores)
                {
                    cSummaryForInverntory = new SummaryForInverntory();
                    cSummaryForInverntory.Store = store.OrderStoreNumber;
                    cSummaryForInverntory.DPCI = store.SkuItem.DPCI.Substring(store.SkuItem.DPCI.Length - 2, 2);
                    cSummaryForInverntory.Pkgs = store.QtyOrdered / 25;
                    lisSummaryForInverntory.Add(cSummaryForInverntory);
                }

            }
            SaveListToCSV(lisSummaryForInverntory);
        }

        private void SaveListToCSV(List<SummaryForInverntory> lisSummaryInventory)
        {
            StringBuilder cStringBuilder = new StringBuilder();
            cStringBuilder.Append(EDIHelperFunctions.STORE);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.DPCIFORMAT);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.PKGS);
            cStringBuilder.Append(EDIHelperFunctions.LineBreak);
            foreach (var item in lisSummaryInventory)
            {
                cStringBuilder.Append(item.Store);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.DPCI);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.Pkgs);
                cStringBuilder.Append(EDIHelperFunctions.LineBreak);
            }

            using (StreamWriter sw = new StreamWriter(OutputFile))
            {
                sw.WriteLine(cStringBuilder.ToString());
            }
        }
    }

}



