using Domain;
using Repository.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ASNCartonTypes
{
  public   interface ICartonTypeForASN : IRepositoryBase<CartonType>
    {
        int SaveChange();
    }
}
