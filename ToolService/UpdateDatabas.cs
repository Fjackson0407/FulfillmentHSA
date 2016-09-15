using Domain;
using LumenWorks.Framework.IO.Csv;
using Repository.DataSource;
using Repository.UOW;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ToolService
{
    public class UpdateDatabas
    {
        string Test = "00001908290000010752";
        string ConnectionString = string.Empty;
        string Path = string.Empty;

        public UpdateDatabas(string _ConnectionString, string _Path)
        {
            ConnectionString = _ConnectionString;
            Path = _Path;
        }

        public enum EOrderStatus
        {
            Open,
            Closed,
            All
        }

        private List<string> GetUpc()
        {
            List<string> Upcs = new List<string>();

            using (CsvReader csv = new CsvReader(new StreamReader(Path), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))
            {

                csv.SupportsMultiline = true;
                IDataReader reader = csv;
                string sPO = string.Empty;


                while (reader.Read())
                {
                    sPO = reader.GetValue(0).ToString();
                    Upcs.Add(sPO);
                }

            }
            return Upcs;
        }


        public void UpDateDB()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                List<string> upcs = GetUpc();
                
                foreach (string UCC128 in upcs)
                {
                    if (!string.IsNullOrEmpty(UCC128))
                    {
                        Carton carton = UoW._Cartons.Find(t => t.UCC128 ==  UCC128  ).FirstOrDefault();
                        
                        if (carton != null)
                        {
                            StoreInfoFromEDI850 store = UoW.AddEDI850.Find(t => t.Id == carton.StoreNumberFK).FirstOrDefault();
                            if (store != null)
                            {
                                store.QtyPacked = store.QtyOrdered;
                                store.PickStatus = (int)EOrderStatus.Closed;
                                int Result = UoW.AddEDI850.SaveChange();
                            }

                        }
                    }
                }


            }
        }
    }
}
