using Domain;
using Domain.ReportObjects;
using Repository.DataSource;
using Repository.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Helpers.EDIHelperFunctions;

namespace FulfillmentService
{
    public class OrderDetail
    {

        public OrderDetail(string _ConnectionString)
        {
            ConnectionString = _ConnectionString;
        }

        public string ConnectionString { get; set; }

 




    }
}
