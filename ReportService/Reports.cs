using Domain;
using Domain.ReportObjects;
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

        public double  BoxTotal { get; set; }
        public double  WeightTotal { get; set; }
        public string _PathUPC { get; private set; }
        public string SummaryBOLPath { get; private set; }

        public Reports(string _ConnectionString, 
                       string _Path, 
                       string _OutputFile, 
                       string PathUPC,
                       string _SummaryBOLPath)
        {
            ConnectionString = _ConnectionString;
            if (File.Exists(_Path) || File.Exists(PathUPC ))

                
            {
                Path = _Path;
                _PathUPC = PathUPC; 
                OutputFile = _OutputFile;
                SummaryBOLPath = _SummaryBOLPath;
            }

        }
        public Reports(string _Path, string _OutputFile)
        {
            if (File.Exists(_Path))
            {
                Path = _Path;
                OutputFile = _OutputFile;
            }
            //throw error 
            
        }


        private List<StoreInfoFromEDI850> GetDataFromFile()
        {
            EDIPOService cEDIPOService = new EDIPOService(Path, "" );
            return cEDIPOService.BuildReport();
        }
        public void CardCount()
        {
            var Data = GetDataFromFile();
            List<CardType> AllCards = new List<CardType>();
            int Total = Data.Sum(t => t.QtyOrdered);
            var SKu = Data.Select(t => t.SkuItem.DPCI).Distinct();
            int total = 0;
            int res = Data.Sum(t => t.QtyOrdered);
            List<SkuItem> lisSku = new List<SkuItem>();
            foreach (var item in SKu)
            {
                var card = Data.Where(t => t.SkuItem.DPCI == item).Select(t => new CardType { NumberOfCards = t.QtyOrdered, Description = t.SkuItem.Product, DPCI = t.SkuItem.DPCI });
                AllCards.AddRange(card);

            }

            MakeCSVForCards(AllCards);
        }

        private void MakeCSVForCards(List<CardType> allCards)
        {
            StringBuilder cStringBuilder = new StringBuilder();
            cStringBuilder.Append(EDIHelperFunctions.Item);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.Description);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.Cards);
            cStringBuilder.Append(EDIHelperFunctions.LineBreak);
            foreach (var item in allCards)
            {
                cStringBuilder.Append(item.DPCI);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.Description);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.NumberOfCards);
                cStringBuilder.Append(EDIHelperFunctions.LineBreak);
            }
            SavetoFile(cStringBuilder.ToString());
        }


        private void SavetoFile(string cSVFiile)
        {
            using (StreamWriter sw = new StreamWriter(Path))
            {
                sw.WriteLine(cSVFiile);
            }
        }


        /// <summary>
        /// Use the data from the raw file via Ezcom before it goes to the database
        /// </summary>
        public void InventorySummaryFromFile()
        {
            //EDIPOService cEDIPOService = new EDIPOService (_PathUPC, "");
            SummaryForInverntory cSummaryForInverntory;
            List<SummaryForInverntory> lisSummaryForInverntory = new List<SummaryForInverntory>(); 
            var lisStore = GetDataFromFile();
            foreach (var store in lisStore)
            {
                cSummaryForInverntory = new SummaryForInverntory();
                cSummaryForInverntory.Store = store.OrderStoreNumber;
                cSummaryForInverntory.QuantityOrdered = store.QtyOrdered;
                cSummaryForInverntory.PO = store.PONumber;
                cSummaryForInverntory.UPC = store.UPCode;
                cSummaryForInverntory.ItemWeight = store.PkgWeight * cSummaryForInverntory.Bundles;
                cSummaryForInverntory.DPCIFull = store.DPCI; 
                
                lisSummaryForInverntory.Add(cSummaryForInverntory);
            }
            BuildBOLSummary(lisSummaryForInverntory);
            SaveListToCSV(lisSummaryForInverntory);
        }

        private void BuildBOLSummary(List<SummaryForInverntory> lisSummaryForInverntory)
        {
            List<MasterBOLSummary> lisMasterBOLSummary = new List<MasterBOLSummary>();
            foreach (var item in lisSummaryForInverntory)
            {
                // var result = lisSummaryForInverntory.Where(t => t.PO == item.PO && t.Store == item.Store );
                List<SummaryForInverntory> lisInventory = lisSummaryForInverntory.Where(t => t.PO == item.PO && t.Store == item.Store).ToList();
                foreach ( SummaryForInverntory  Inventory in lisInventory)
                {
                    lisMasterBOLSummary.Add(new MasterBOLSummary()
                    {
                        BOL = Inventory.PO,
                        Boxes = lisInventory.Count(),
                        Pounds = lisInventory.Count()
                    });
                }
                
            }
            WriteCSVForBOL(lisMasterBOLSummary);
        }

        private void WriteCSVForBOL(List<MasterBOLSummary> lisMasterBOLSummary)
        {
            StringBuilder cStringBuilder = new StringBuilder();
            cStringBuilder.Append(EDIHelperFunctions.BOLSummary);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.Boxes);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.Lbs);
            cStringBuilder.Append(EDIHelperFunctions.LineBreak);
            foreach (var item in lisMasterBOLSummary )
            {
                cStringBuilder.Append(item.BOL);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.Boxes);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.Pounds);
                cStringBuilder.Append(EDIHelperFunctions.LineBreak);
            }
            using (StreamWriter sw = new StreamWriter(SummaryBOLPath))
            {
                sw.WriteLine(cStringBuilder.ToString());
            }
        }


        private void SaveListToCSV(List<SummaryForInverntory> lisSummaryInventory)
        {
            StringBuilder cStringBuilder = new StringBuilder();
            cStringBuilder.Append(EDIHelperFunctions.STORE);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.PoNumber);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.UPC);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.DPCI);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.DPCIFORMAT);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.QuantityOrdered);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.Bundles);
            cStringBuilder.Append(EDIHelperFunctions.Comma);
            cStringBuilder.Append(EDIHelperFunctions.ItemWeight);
            cStringBuilder.Append(EDIHelperFunctions.LineBreak);

            foreach (var item in lisSummaryInventory)
            {
                cStringBuilder.Append(item.Store);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.PO);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.UPC);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.DPCIFull);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.DPCI);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.QuantityOrdered );
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.Bundles);
                cStringBuilder.Append(EDIHelperFunctions.Comma);
                cStringBuilder.Append(item.ItemWeight);
                cStringBuilder.Append(EDIHelperFunctions.LineBreak);
            }

            using (StreamWriter sw = new StreamWriter(OutputFile))
            {
                sw.WriteLine(cStringBuilder.ToString());
            }
        }
    }

}



