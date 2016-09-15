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

            //Cisco, This loop will consume a considerable amount of processing capacity.
            //You may want to add a Thread.Sleep(500) to release some wasted resources.
            while (Console.Read() != 'q') ;
        }
       
    private static void SendEmailForIncommingFile()
        {

            try
            {
                GetKeys cGetKeys = new GetKeys();
                EDIWatch cEDIWatch = new EDIWatch(cGetKeys.GetInboundLocation(), cGetKeys.ConnectionString);
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
