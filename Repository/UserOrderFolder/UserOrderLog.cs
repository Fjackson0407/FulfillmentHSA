using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UserOrderFolder
{
  public   class UserOrderLog : RepositoryBase<UserOrders>, IUserOrderLog 
    {
        public UserOrderLog(EDIContext context)
            : base(context) { }

        public int SaveChange()
        {
            return this.Context.SaveChanges();
        }

    }
}
