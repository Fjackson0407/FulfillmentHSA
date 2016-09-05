using ASNService;
using Domain;
using Domain.ReportObjects;
using EDIException;
using EDIService;
using FulfillmentService;
using LabelService;
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


            //********************************************** MAKE LABELS *********************************************************************

            //try
            //{
            //    string FilePathAmex = @"D:\Testing\Lables\AmexLabels\AmexFrom.csv";
            //    string NewFileAmex = @"D:\Testing\Lables\AmexLabels\AMEXNew.csv";

            //    string FilePathVisa = @"D:\Testing\Lables\VisaLables\8.30.16 VisaMC ASN.csv";
            //    string NewFileVisa = @"D:\Testing\Lables\VisaLables\VisaLabelsWeek4.csv";



            //    MCLable cMCLable = new MCLable(FilePathVisa , ConnectionStringWork,  NewFileVisa );
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
            //    string path2 = @"D:\Testing\BigOrder\Auto\AMEXDoc_20160829162632173.csv";
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
            string root = @"D:\Testing\ASN2\";
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
