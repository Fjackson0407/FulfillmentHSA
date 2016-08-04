﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
public     class UserTable
    {
        public Guid Id { get; set; }
        public string  UserName { get; set; }
        public string  Password { get; set; }
        public DateTime  DTS { get; set; }
           public virtual UserOrders Orders { get; set; }
        public Guid? OrdersFK { get; set; }

    }
}
