using Domain;
using Domain.ReportObjects;
using EDIException;
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
using static Helpers.EDIHelperFunctions;

namespace ReportService
{
    public class Reports
    {
        public string ConnectionString { get; private set; }
        public string Path { get; set; }
        public string OutputFile { get; set; }

        public double BoxTotal { get; set; }
        public double WeightTotal { get; set; }
        public string _PathUPC { get; private set; }
        public string SummaryBOLPath { get; private set; }

        public Reports(string _ConnectionString,
                       string _Path,
                       string _OutputFile,
                       string PathUPC,
                       string _SummaryBOLPath)
        {
            ConnectionString = _ConnectionString;
            Path = _Path;
            _PathUPC = PathUPC;
            OutputFile = _OutputFile;
            SummaryBOLPath = _SummaryBOLPath;
        }
        public Reports(string _Path, string _OutputFile)
        {
            Path = _Path;
            OutputFile = _OutputFile;
        }


        private List<StoreInfoFromEDI850> GetDataFromFile()
        {
            if (!File.Exists(Path ) )
            {
                throw new ExceptionsEDI(string.Format("{0} {1}", Help, ErrorCodes.HSAError4));
            }
            EDIPOService cEDIPOService = new EDIPOService(Path, "");
            return cEDIPOService.BuildReport();
        }
        public void CardCount()
        {
            var Data = GetDataFromFile();
            List<CardType> AllCards = new List<CardType>();
            int Total = Data.Sum(t => t.QtyOrdered);
            //var SKu = Data.Select(t => t.SkuItem.DPCI).Distinct();
            int total = 0;
            int res = Data.Sum(t => t.QtyOrdered);
            List<SkuItem> lisSku = new List<SkuItem>();
            //foreach (var item in SKu)
            //{
            //    var card = Data.Where(t => t.SkuItem.DPCI == item).Select(t => new CardType { NumberOfCards = t.QtyOrdered, Description = t.SkuItem.Product, DPCI = t.SkuItem.DPCI });
            //    AllCards.AddRange(card);

            //}

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
            SummaryForInverntory cSummaryForInverntory;
            List<SummaryForInverntory> lisSummaryForInverntory = new List<SummaryForInverntory>();
            var lisStore = GetDataFromFile();
            foreach (var store in lisStore)
            {
                cSummaryForInverntory = new SummaryForInverntory();
                cSummaryForInverntory.Store = store.OrderStoreNumber;
                cSummaryForInverntory.PO = store.PONumber;
                cSummaryForInverntory.UPC = store.UPCode;
                cSummaryForInverntory.DPCIFull = store.DPCI;
                cSummaryForInverntory.QuantityOrdered = store.QtyOrdered;
                cSummaryForInverntory.ItemWeight = EDIHelperFunctions.MCVisaWeight;


                lisSummaryForInverntory.Add(cSummaryForInverntory);
            }
             BuildBOLSummary(lisStore );
            SaveListToCSV(lisSummaryForInverntory);
        }

        private void BuildBOLSummary(List<StoreInfoFromEDI850> lisSummaryForInverntory)
        {
            List<MasterBOLSummary> lisMasterBOLSummary = new List<MasterBOLSummary>();
            var lispo = lisSummaryForInverntory.Select(t => t.PONumber).Distinct();
            foreach (var item in lispo  )
            {
                var Stores = lisSummaryForInverntory.Where(t => t.PONumber == item);
                lisMasterBOLSummary.Add(GetPoSummary(Stores));

            }

            
            WriteCSVForBOL(lisMasterBOLSummary);
        }

        private MasterBOLSummary GetPoSummary(IEnumerable<StoreInfoFromEDI850> stores)
        {
            Double  PoundTotal = stores.Sum(t => t.PkgWeight);
            int qty1 = stores.Sum(t => t.QtyOrdered);
            int bundles = (stores.Sum(t => t.QtyOrdered) / 25);
            int boxes = 0;
            int max = 48;
            while (bundles > 0)
            {
                if (bundles < max )
                {
                    boxes++;
                }
                if (bundles > max )
                {
                    bundles = -max;
                    boxes++;
                }
                
            }



            return new MasterBOLSummary()
            {
                BOL = stores.Select(t => t.PONumber).FirstOrDefault(),
                Boxes =  boxes ,
                Pounds = PoundTotal
            };
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
            foreach (var item in lisMasterBOLSummary)
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
                cStringBuilder.Append(item.QuantityOrdered);
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



