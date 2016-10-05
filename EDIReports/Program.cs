using ReportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDIReports
{
    class Program
    {
        static void Main(string[] args)
        {
      
            string inputfile = @"C:\Users\Contractor\Dropbox\data\w2.csv";
            string OutputFile = @"C:\Users\Contractor\Dropbox\data\Results.csv";
            string OutputFile2 = @"C:\Users\Contractor\Dropbox\data\ResultsBOL.csv";
            string UPCData = @"C:\Users\Cisco\Dropbox\data\DPCIData.csv";
            Reports cReports = new Reports("", inputfile, OutputFile , UPCData, OutputFile2 );
            cReports.InventorySummaryFromFile();

        }
}
}
