using Domain;
using Repository.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.PackSize
{
   public  interface IPackSizeForBundles: IRepositoryBase<Pack>
    {
        int SaveChange();
    }
}
