using ASNService;
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

namespace IntakeFileTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string ConnectionString = @"Data Source=CCPC08\VALIDDB;Initial Catalog=EDIHSA;Integrated Security=True";
            string ConStringHome = @"Data Source=SUPERMANPC\SUPERMANDB;Initial Catalog=EDIHSA;Integrated Security=True";

            string path = @"D:\Testing\w20.csv";
            string path2 = @"D:\Testing\Full.xml";
            string AmexPath = @"D:\Testing\Amex\W1.csv";
            string po = "0290-9624723-0579";  // "0290 -8987645-0551";   // "0290 -8987645-0554";                                   // "0290 -8987645-0554";                  //               "0290 -8987645-0554"; 
            string po2 = "0290-5429221-0556";  // "0290 -8987645-0553"; //this is for home only will not work at work 
            string Store = "0656"; //Make function to get stores from po 


            //*********************************** Add Data *********************************************************************************************************

            try
            {

                EDIPOService cEDIPOService = new EDIPOService(AmexPath, ConnectionString);
                cEDIPOService.ParseEDI850();

            }
            catch (ExceptionsEDI ex)
            {
                string tgest = ex.Message.ToString();
            }

        



        //*********************************** Add Data *********************************************************************************************************

        //*********************************** Make Lables *********************************************************************************************************

        string FilePath = @"D:\Testing\Amex\Lables\AmexLabels.csv";
        LabelMaker cLabelMaker = new LabelMaker(ConnectionString , FilePath );
        cLabelMaker.GetAllOrders();

        //*********************************** Make Lables *********************************************************************************************************

        //*********************************** Make ASN *********************************************************************************************************

        //   ASNBuild cASNBuild = new ASNBuild(path2, ConnectionString, po2, Store, "0556");
        //    cASNBuild.BuildASN();

        //*********************************** Make ASN *********************************************************************************************************
    }
}
}
