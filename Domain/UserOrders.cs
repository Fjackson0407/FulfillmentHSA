using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
public   class UserOrders
    {

        public UserOrders()
        {
            StoreOrderDetail = new HashSet<StoreOrderDetail>();
            User = new HashSet<UserTable>();
        }

        public Guid  Id { get; set; }


        public ICollection<StoreOrderDetail> StoreOrderDetail { get; set; }

        public ICollection<UserTable> User { get; set; }
    }
}
