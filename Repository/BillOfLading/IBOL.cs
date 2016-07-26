using Domain;
using Repository.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.BillOfLading
{
   public  interface IBOL : IRepositoryBase<Bill>
    {
        int SaveChanges();
    }
}
