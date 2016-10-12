using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Notes
{
    public class StoreNotesRepo : RepositoryBase<StoreNotes>, IStoreNotesRepo
    {
        public StoreNotesRepo(EDIContext context)
            : base(context) 
        { }


    }
}
