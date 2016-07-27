using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempLables
{
    public class MCLable
    {
        public string  FilePath { get; set; }

        public MCLable(string _FilePath)
        {
            FilePath = _FilePath; 
        }

        public void MakeLabels()
        {

            using (CsvReader csv = new CsvReader(new StreamReader(FilePath ), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))
            {

                csv.SupportsMultiline = true;
                IDataReader reader = csv;
                
                string sPO = string.Empty;


            }
        }
}
