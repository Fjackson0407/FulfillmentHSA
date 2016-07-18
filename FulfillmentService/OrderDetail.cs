using Domain;
using Domain.ReportObjects;
using Repository.DataSource;
using Repository.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Helpers.EDIHelperFunctions;

namespace FulfillmentService
{
    public class OrderDetail
    {

        public OrderDetail(string _ConnectionString)
        {
            ConnectionString = _ConnectionString;
        }

        public string ConnectionString { get; set; }

        public List<WeeklySummaryView> WeeklyProductSummary(DateTime PODate)
        {
            List<WeeklySummaryView> lisWeeklySummaryView = new List<WeeklySummaryView>();
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                //The all orders for po
                
                List<string> DCs = UoW.AddEDI850.Find(t => t.PODate == PODate).Select(t => t.DCNumber).Distinct().ToList();
                foreach (string item in DCs)
                {

                    List<Store> StoresForDC = UoW.AddEDI850.Find(t => t.PODate == PODate)
                                                           .Where(x => x.DCNumber == item).ToList();
                                                           
                    int Cards = 0;
                    foreach (Store  _store in StoresForDC )
                    {
                        Cards  += _store.QtyOrdered;
                    }
                    int picks = Cards / InnersPerPacksSize;
                    WeeklySummaryView cWeeklySummaryView = new WeeklySummaryView();
                    cWeeklySummaryView.DCNumber = item;
                    cWeeklySummaryView.NumberofPicks = picks;
                    cWeeklySummaryView.NumberofStores = StoresForDC.Count;
                    lisWeeklySummaryView.Add(cWeeklySummaryView);
                    
                }




            }

            return lisWeeklySummaryView;

        }


    }
}
