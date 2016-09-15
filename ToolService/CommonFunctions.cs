using Domain;
using Repository.DataSource;
using Repository.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolService
{
public   class CommonFunctions
    {
        public CommonFunctions(string _ConnectionString)
        {
            ConnectionString = _ConnectionString;
        }

        public string ConnectionString { get; set; }

        public  SkuItem GetSkuInfo(string ProductUPC)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                SkuItem cSkuItem = UoW.Sku.Find(t => t.ProductUPC == ProductUPC).FirstOrDefault();
                return cSkuItem;
            }
        }
    }
}
