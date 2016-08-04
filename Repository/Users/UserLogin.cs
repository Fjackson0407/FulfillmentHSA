using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Users
{
  public   class UserLogin : RepositoryBase<UserTable>, IUserLogin 
    {
        public UserLogin(EDIContext context )
            :base(context ) { }

        public int SaveChange()
        {
            return this.Context.SaveChanges();
        }
    }
}
