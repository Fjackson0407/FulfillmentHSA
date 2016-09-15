using ASNService;
using RegistryFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchDogHSA;

namespace ASNBuilder
{
    class Program
    {
        static void Main(string[] args)
        {

                 BuildASNs();
            //Cisco, This loop will consume a considerable amount of processing capacity.
            //You may want to add a Thread.Sleep(500) to release some wasted resources.
            while (Console.Read() != 'q') ;
        }
        private static void BuildASNs()
        {

            GetKeys cGetKeys = new GetKeys();

            //Cisco, this is weird that you perform the operation from the constructor.  You would not be able to unit test this.
            ASNBuild cASNBuild = new ASNBuild(cGetKeys.GetASNLocation(),  cGetKeys.GetTempLocation(), cGetKeys.ConnectionString);

        }


    }
}
