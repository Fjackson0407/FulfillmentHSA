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
      
            string inputfile = @"D:\EDI import files\Week 4\MCVDoc_20161005045043960.csv";
            string OutputFile = @"D:\EDI import files\Week 4\Week4 Results.csv";
            string OutputFile2 = @"C:\Users\Contractor\Dropbox\data\ResultsBOL.csv";
            string UPCData = @"C:\Users\Cisco\Dropbox\data\DPCIData.csv";
            Reports cReports = new Reports("", inputfile, OutputFile , UPCData, OutputFile2 );
            cReports.InventorySummaryFromFile();

        }
}
}
