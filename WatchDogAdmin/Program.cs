using Domain;
using RegistryFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WatchDogHSA;

namespace WatchDogAdmin
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("This is EDI watch dog I will watch for in comming EDI 850 files from Ezcom");
            Console.WriteLine("Press q then hit the enter key  at anytime to stop watch dog");
            SendEmailForIncommingFile();
            while (Console.Read() != 'q') ;
        }
       
    private static void SendEmailForIncommingFile()
        {

            GetKeys cGetKeys = new GetKeys();
            EDIWatch cEDIWatch = new EDIWatch(cGetKeys.GetInboundLocation(), cGetKeys.ConnectionString );

        }
    }
}
