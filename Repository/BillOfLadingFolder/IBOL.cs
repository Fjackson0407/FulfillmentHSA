using Domain;
using Repository.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.BillOfLadingFolder
{
   public  interface IBOL : IRepositoryBase<Domain.BOLForASN>
    {
        int SaveChanges();
    }
}
