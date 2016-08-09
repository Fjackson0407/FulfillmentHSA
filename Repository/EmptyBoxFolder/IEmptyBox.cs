using Domain;
using Repository.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EmptyBoxFolder
{
  public   interface IEmptyBox : IRepositoryBase<EmptyBoxWeight>
    {
        int SaveChanges();
    }
}
