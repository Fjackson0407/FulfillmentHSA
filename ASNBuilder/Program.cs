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
            while (Console.Read() != 'q') ;
        }
        private static void BuildASNs()
        {

            GetKeys cGetKeys = new GetKeys();
            ASNBuild cASNBuild = new ASNBuild(cGetKeys.GetASNLocation(),  cGetKeys.GetTempLocation(), cGetKeys.ConnectionString);

        }


    }
}
