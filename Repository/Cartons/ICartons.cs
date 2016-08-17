﻿using Domain;
using Repository.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cartons
{
public     interface ICartons856 : IRepositoryBase<Carton>
    {
        int SaveChanges();
        void AddNewCarton(List<Carton> NewCarton);
    }
}
