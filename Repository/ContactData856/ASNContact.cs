using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ContactData856
{
    public class ASNContact: RepositoryBase<ContactType> , IASNContact 
    {
        public ASNContact(EDIContext context)
              :base(context )
        {

        }
    }
}
