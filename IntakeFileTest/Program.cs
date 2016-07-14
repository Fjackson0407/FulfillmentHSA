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
            string path = @"D:\850\w1.csv";
            string po = "0290-8987645-0555";
            string Store = "123";
            EDIPOService cEDIPOService = new EDIPOService(path, ConnectionString);
            cEDIPOService.AddEdi850();
            ASNBuild cASNBuild = new ASNBuild(path, ConnectionString, po, Store );
            cASNBuild.NumberOfBoxesForOrder();
            cEDIPOService.AddDC( );


            
        }
    }
}
