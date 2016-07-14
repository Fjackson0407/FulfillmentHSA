using Domain;
using Repository.BaseClass;
using Repository.Inbound850;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Inbound850
{
    public interface IAddEDI850: IRepositoryBase<Store>
    {
        
        int SaveChange();
    }
}
