using Domain;
using Repository.DataSource;
using Repository.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService
{
    public class Reports
    {
        public string ConnectionString { get; private  set; }


        public Reports(string _ConnectionString )
        {
            ConnectionString = _ConnectionString;
        }



        private List<BundlesPerStore> BOLSummary(List<StoreReport>  Stores)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                int result = 0;
                int i = 1; 
                List<BundlesPerStore> lisBundlesPerStore = new List<BundlesPerStore>();

                            

                foreach (StoreReport  item in Stores )
                {
                    BundlesPerStore cBundlesPerStore = new BundlesPerStore();
                    List<StoreReport> BUNDLES = Stores.Where(t => t.PONumber == item.PONumber  )
                                        .Where(t => t.StoreNumber ==  item.StoreNumber ).ToList();

                    foreach (var item2 in BUNDLES)
                    {
                        cBundlesPerStore.Bundles = item2.Budles;
                        cBundlesPerStore.PONumber = item2.PONumber;
                        cBundlesPerStore.QuantityOrdered = item2.QtyOrdered;
                        cBundlesPerStore.Store = item2.StoreNumber;
                        lisBundlesPerStore.Add(cBundlesPerStore);
                    }

                }
                var rew = lisBundlesPerStore.OrderBy(t => t.PONumber);
                return lisBundlesPerStore;

            }
        }
        public  List<StoreReport> GetWeeklyStoreReport(DateTime StartDate)
        {
            List<StoreReport> Reports = new List<StoreReport>();
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                List<StoreInfoFromEDI850> stores = UoW.AddEDI850.Find(t => t.PODate == StartDate).ToList();
                int count = 1; 

                foreach (StoreInfoFromEDI850  item in stores )
                {
                    StoreReport cStoreReport = new StoreReport();
                    cStoreReport.Number = count;
                    cStoreReport.StoreNumber = item.OrderStoreNumber;
                    cStoreReport.PONumber = item.PONumber;
                    cStoreReport.UPC = item.UPCode;
                    cStoreReport.DPCI = item.SkuItem.DPCI;
                    cStoreReport.QtyOrdered = item.QtyOrdered;
                    cStoreReport.ItemWeight = item.PkgWeight;
                    cStoreReport.Boxes = 1; //This is only a place holder for now 
                    Reports.Add(cStoreReport);
                    count++;
                }
            }
            List<BundlesPerStore> result =  BOLSummary(Reports);
            return Reports;
        }
    }
}
