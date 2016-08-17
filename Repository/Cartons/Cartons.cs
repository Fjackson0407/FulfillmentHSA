﻿using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cartons
{
   public  class Cartons856 : RepositoryBase<Carton> , ICartons856 
    {

        public Cartons856(EDIContext context ) 
            :base(context ) { }

        public  int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        public void AddNewCarton(List<Carton> NewCarton)
        {
            foreach (Carton CartonItem in NewCarton)
            {
                EDIContext.Carton.Attach(CartonItem );
            }
        }

        public EDIContext EDIContext
        {
            get { return Context as EDIContext; }
        }
    }
}
