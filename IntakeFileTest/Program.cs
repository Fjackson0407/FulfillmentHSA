using ASNService;
using Domain.ReportObjects;
using EDIService;
using FulfillmentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntakeFileTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string ConnectionString = @"Data Source=CCPC08\VALIDDB;Initial Catalog=EDIHSA;Integrated Security=True";
            string ConStringHome = @"Data Source=SUPERMANPC\SUPERMANDB;Initial Catalog=EDIHSA;Integrated Security=True";

            string path = @"D:\w8.csv";
            string path2 = @"D:\Testing\ASN123.xml";
            
            string po = "0290-8987645-0555"; 
            string po2 = "0290-8987645-0553"; //this is for home only will not work at work 
            string Store = "0310"; //Make function to get stores from po 

            OrderDetail cOrderDetail = new OrderDetail(ConnectionString);
            string dd = "7/11/2016";

            DateTime PODate = Convert.ToDateTime(dd);
             List< WeeklySummaryView> Summary =  cOrderDetail.WeeklyProductSummary(PODate);

            //  EDIPOService cEDIPOService = new EDIPOService(path, ConnectionString);
            //     cEDIPOService.AddEdi850();
            //   ASNBuild cASNBuild = new ASNBuild(path2, ConnectionString, po, Store  );
            //      cASNBuild.BuildASN();



        }
    }
}
