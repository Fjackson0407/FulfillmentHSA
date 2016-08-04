﻿using ASNService;
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
            string ConnectionString = @"Data Source=CCPC08\VALIDDB;Initial Catalog=EDIHSA;Integrated Security=True";
            string ConStringHome = @"Data Source=SUPERMANPC\SUPERMANDB;Initial Catalog=EDIHSA;Integrated Security=True";

            string path = @"D:\Testing\w20.csv";
            string AmexPath = @"D:\Testing\Amex\W1.csv";
            string po = "0290-9624723-0579";  // "0290 -8987645-0551";   // "0290 -8987645-0554";                                   // "0290 -8987645-0554";                  //               "0290 -8987645-0554"; 
            string po2 = "0290-5429221-0556";  // "0290 -8987645-0553"; //this is for home only will not work at work 
            string Store = "1175"; //Make function to get stores from po 

            string DemoVisaFile = @"D:\Testing\VisaDemo\Doc1.csv";
            string DemoAmxFile = @"D:\Testing\BigOrder\w2.csv";
            string DemoXMLFile = @"D:\Testing\VisaDemo\ASN\TestX0579.xml";
            string DemoPO = "0290-7029784-0579";        //"0290-9540383-0551";


            //********************************************** MAKE LABELS *********************************************************************

            //try
            //{
            //    string FilePathAmex = @"D:\Testing\Lables\Monday080116\Amex.csv";

            //    string FilePathVisa = @"D:\Testing\Lables\Monday080116\VMC.csv";

            //    string NewFile = @"D:\Testing\Lables\Monday080116\LabelsVisa.csv";

            //    MCLable cMCLable = new MCLable(FilePathVisa, ConnectionString, NewFile);
            //    cMCLable.MakeLabelsVisaMaster();


            //}
            //catch (ExceptionsEDI ex)
            //{
            //    string tgest = ex.Message.ToString();
            //}



            //******************************************************************************************************************
            //*********************************** Add Data *********************************************************************************************************

            //try
            //{

            //    EDIPOService cEDIPOService = new EDIPOService(DemoAmxFile, ConnectionString);
            //    cEDIPOService.ParseEDI850();

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

            //*********************************** Make ASN *********************************************************************************************************

            try
            {

                ASNBuild cASNBuild = new ASNBuild(DemoXMLFile, ConnectionString, DemoPO, Store, "0579");
                cASNBuild.BuildASN();


            }
            catch (ExceptionsEDI ex)
            {
                string tgest = ex.Message.ToString();
            }



            //*********************************** Make ASN *********************************************************************************************************
        }
    }
}
