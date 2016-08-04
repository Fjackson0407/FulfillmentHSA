using Domain;
using Repository.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Users
{
    public  interface IUserLogin : IRepositoryBase<UserTable>
    {
        int SaveChange();
    }
}
