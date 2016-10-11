
using ASNService;
using Domain;
using Domain.ReportObjects;
using EDIException;
using EDIService;
using FulfillmentService;
using LabelService;
using RegistryFunctions;
using ReportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempLables;


namespace IntakeFileTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string ConnectionStringWork = @"Data Source=CCPC08\VALIDDB;Initial Catalog=EDIDev;Integrated Security=True;MultipleActiveResultSets=True";
            string ConStringHome = @"Data Source=SUPERMANPC\SUPERMANDB;Initial Catalog=EDIHSADev;Integrated Security=True;MultipleActiveResultSets=True";
            string RemoteConnectionString = @"Data Source=CCPC08\VALIDDB;Database=EDIHSA;User Id=VaildUser;Password = ABC123";
            string HSAConnectionString = "Data Source=CCSQLSEC;Database = EDIHSA; uid = Cisco; pwd = Engineer12; MultipleActiveResultSets = True";
         //   string HSAProdString = "Data Source=CCSQLSEC;Database=ValidFulfillment;uid=Car;pwd=@Viper;MultipleActiveResultSets=True";
         //   string HSADevString = "Data Source=CCSQLSEC;Database=DevValidFulfillment;uid=Car;pwd=@Viper;MultipleActiveResultSets=True";

            string AmexPath = @"D:\Testing\Amex\W1.csv";
            string po = "0290-9624723-0579";  // "0290 -8987645-0551";   // "0290 -8987645-0554";  // "0290 -8987645-0554";                  //               "0290 -8987645-0554"; 
            string po2 = "0290-5429221-0556";  // "0290 -8987645-0553"; //this is for home only will not work at work 




            string HoildayPath = @"C:\Hoilday ASN\One off\MCVDoc_20161005125048644.csv";
            string TempPath = @"C:\Hoilday ASN\One off\tem.xml";
            string ASNPath = @"C:\Hoilday ASN\";
            string LabelFile = @"C:\Hoilday ASN\MC.csv";
        
            EDIPOService cEDIPOService = new EDIPOService(HoildayPath, 
                                                          ASNPath, 
                                                          TempPath,
                                                          LabelFile ,
                                                         "");
            cEDIPOService.BuildHoildayorders();


           

        }
    }
}
