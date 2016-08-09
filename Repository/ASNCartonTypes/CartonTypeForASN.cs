using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ASNCartonTypes
{
    public class CartonTypeForASN: RepositoryBase<CartonType>, ICartonTypeForASN 
    {

        public CartonTypeForASN(EDIContext context)
            :base(context )
        { }

        public int SaveChange()
        {
            return this.Context.SaveChanges();
        }


    }
}
