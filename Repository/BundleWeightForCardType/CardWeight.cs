using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.BundleWeightForCardType
{
  public   class CardWeight : RepositoryBase<BundleWeight> ,ICardWeight
    {
        public CardWeight(EDIContext context)
            : base(context) { }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }
    }
}
