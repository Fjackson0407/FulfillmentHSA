using ASNService;
using EDIException;
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
            string DemoXMLFile = @"C:\EDI850 prod\ASN AMEX\ASN114.xml";
            string root = @"C:\EDI850 prod\ASN2MC\";
            string HSAProdString = "Data Source=CCSQLSEC;Database=ValidFulfillment;uid=Car;pwd=@Viper;MultipleActiveResultSets=True";
            try
            {
                ASNBuild cASNBuild = new ASNBuild(root, DemoXMLFile, HSAProdString);
            }
            catch (ExceptionsEDI ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }




    }
}
