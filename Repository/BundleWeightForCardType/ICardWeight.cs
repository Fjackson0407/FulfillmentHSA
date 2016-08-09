using Domain;
using Repository.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.BundleWeightForCardType
{
    public interface  ICardWeight : IRepositoryBase<BundleWeight>
    {
        int SaveChanges();
    }
}
