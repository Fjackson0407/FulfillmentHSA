using EDIException;
using EDIService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportEDI850
{
    class Program
    {
      
        static void Main(string[] args)
        {
            try
            {
                string HSAProdString = "Data Source=CCSQLSEC;Database=ValidFulfillment;uid=Car;pwd=@Viper;MultipleActiveResultSets=True";
                string EDI850Import = @"C:\EDI850 prod\MCVDoc_20161003165049067.csv"; 
                 EDIPOService cEDIPOService = new EDIPOService(EDI850Import, HSAProdString );
                cEDIPOService.ParseEDI850();
            }
            catch (ExceptionsEDI ex)
            {
                string tgest = ex.Message.ToString();
            }

        }
    }
}
