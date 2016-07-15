using ASNService;
using EDIService;
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
            string path = @"D:\850\TestingASN\Header.xml";
            string path2 = @"D:\850\DC.csv";
            string po = "0290-8987645-0555";
            string Store = "0310"; //Make function to get stores from po 
       //    EDIPOService cEDIPOService = new EDIPOService(path2, ConnectionString);
         //   cEDIPOService.AddDC();
         ASNBuild cASNBuild = new ASNBuild(path, ConnectionString, po, Store );
            cASNBuild.BuildASN();


            
        }
    }
}
