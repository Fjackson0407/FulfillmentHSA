using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Shipping
{
    public class ShipFrom: RepositoryBase<ShipFromInformation> , IShipFrom 
    {

        public ShipFrom(EDIContext context)
            : base(context)
        { }
    }
}
