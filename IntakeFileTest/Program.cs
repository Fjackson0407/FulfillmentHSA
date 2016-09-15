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
            string ConnectionStringWork = @"Data Source=CCPC08\VALIDDB;Initial Catalog=EDIHSATest;Integrated Security=True;MultipleActiveResultSets=True";
            string ConnectionStringWork2 = @"Data Source=CCPC08\VALIDDB;Initial Catalog=EDIAdminTest;Integrated Security=TrueMultipleActiveResultSets=True";
            string ConStringHome = @"Data Source=SUPERMANPC\SUPERMANDB;Initial Catalog=EDIHSA;Integrated Security=True";
            string RemoteConnectionString = @"Data Source=CCPC08\VALIDDB;Database=EDIHSA;User Id=VaildUser;Password = ABC123";
            string HSAConnectionString = "Data Source=CCSQLSEC;Database = EDIHSA; uid = Cisco; pwd = Engineer12; MultipleActiveResultSets = True";
            string path = @"D:\Testing\w20.csv";
            string AmexPath = @"D:\Testing\Amex\W1.csv";
            string po = "0290-9624723-0579";  // "0290 -8987645-0551";   // "0290 -8987645-0554";  // "0290 -8987645-0554";                  //               "0290 -8987645-0554"; 
            string po2 = "0290-5429221-0556";  // "0290 -8987645-0553"; //this is for home only will not work at work 



            //********************************************** Report Inventory *********************************************************************

            //string PathReport = @"D:\Inventory Reports\SotresReport.csv";
            //string EDI850 = @"D:\Inventory Reports\MCVDoc_20160829161839505.csv";
            //Reports cReports = new Reports(ConnectionStringWork, EDI850 ,PathReport );
            //cReports.InventorySummaryFromFile();

            //********************************************** Report Inventory *********************************************************************


            //********************************************** Report Maker *********************************************************************
            //string Path = @"D:\Report Files\AMX1.csv";
            //ReportsFromInboundFile cReportsFromInboundFile = new ReportsFromInboundFile(Path, ConnectionStringWork);
            //cReportsFromInboundFile.WeeklyReport();


            //********************************************** Report Maker *********************************************************************

            //********************************************** MAKE LABELS *********************************************************************

            //try
            //{
            //    string FilePathAmex = @"D:\Testing\Lables\AmexLabels\AmexFrom.csv";
            //    string NewFileAmex = @"D:\Testing\Lables\AmexLabels\AMEXNew.csv";

            //    string FilePathVisa = @"C:\Users\Contractor\Desktop\New folder\9.6.16 ASN VMC.csv";
            //    string NewFileVisa = @"C:\Users\Contractor\Desktop\New folder\VisaLabelsWeek4.csv";



            //    MCLable cMCLable = new MCLable(FilePathVisa, ConnectionStringWork, NewFileVisa);
            //    cMCLable.Amex();


            //}
            //catch (ExceptionsEDI ex)
            //{
            //    string tgest = ex.Message.ToString();
            //}



            //******************************************************************************************************************
            //*********************************** Add Data *********************************************************************************************************

            //try
            //{
            //    string MCFile = @"D:\Testing\BigOrder\Yesterday.csv";
            //    string Amex = @"D:\Testing\BigOrder\w2.csv";
            //    string CurrentAmexOrder = @"D:\Testing\BigOrder\wc-orders.csv";
            //    string path2 = @"C:\Users\Contractor\Downloads\W1.csv";
            //    EDIPOService cEDIPOService = new EDIPOService(path2, ConnectionStringWork);
            //    cEDIPOService.ParseEDI850();
            //    ///cEDIPOService.Path = Amex;
            //    //cEDIPOService.ParseEDI850();

            //}
            //catch (ExceptionsEDI ex)
            //{
            //    string tgest = ex.Message.ToString();
            //}





            //*********************************** Add Data *********************************************************************************************************

            //*********************************** Make Lables *********************************************************************************************************

            //try
            //{

            //    string FilePath = @"D:\Testing\Amex\Lables\AmexLabelsSmall.csv";
            //    string MSPath = @"D:\Testing\Lables\mc.csv";
            //    LabelMaker cLabelMaker = new LabelMaker(ConnectionString, MSPath);
            //    cLabelMaker.GetAllOrders();


            //}
            //catch (ExceptionsEDI ex)
            //{
            //    string tgest = ex.Message.ToString();
            //}



            //*********************************** Make Lables *********************************************************************************************************

            //   GetKeys cGetKeys = new GetKeys();
            // cGetKeys.SendEmail("This is only a test", "Testing 123 Testing 123", true );
            //

            //*********************************** Update Store *********************************************************************************************************

            //string path2 = @"D:\Testing\UPC2.csv";
            //UpdateDatabas cUpdateDatabas = new UpdateDatabas(ConnectionStringWork, path2 );
            //string Test = "00001908290000010752";
            //cUpdateDatabas.UpDateDB( );






            //*********************************** Update Store *********************************************************************************************************


            //*********************************** Make ASN *********************************************************************************************************
            string Store = "1114"; //Make function to get stores from po 
            const char dash = '-';
            string DemoXMLFile = @"D:\Testing\ASN2\ASN114.xml";
            string root = @"D:\ASN Test\";
            string DemoPO = "0290-7947639-0578";
            string[] POSplit = DemoPO.Split(dash);
            string DCNumber = POSplit[2];

            try
            {

                ASNBuild cASNBuild = new ASNBuild(root, DemoXMLFile, ConnectionStringWork);
                //ASNBuild cASNBuild = new ASNBuild(DemoXMLFile, ConnectionStringWork, DemoPO, Store, DCNumber);
                //cASNBuild.BuildASN();

            }
            catch (ExceptionsEDI ex)
            {
                string tgest = ex.Message.ToString();
            }



            //*********************************** Make ASN *********************************************************************************************************


            //*********************************** Make Report *********************************************************************************************************

            //Reports cReports = new Reports(ConStringHome);
            //DateTime StartDate = DateTime.Parse("2016-09-02");
            //List<StoreReport> Report = cReports.GetWeeklyStoreReport(StartDate);



            //*********************************** Make Report *********************************************************************************************************




        }
    }
}
