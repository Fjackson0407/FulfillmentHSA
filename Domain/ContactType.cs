using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public     class ContactType
    {
        public Guid Id { get; set; }
        public string  LastName { get; set; }
        public string  FirstName { get; set; }
        public string  EmailAddress { get; set; }
        public string  PhoneNumber { get; set; }
        private const string SPACE = " ";
        public string FullName
        {
            get
            {
                return string.Format("{0}{1}{2}", FirstName, SPACE, LastName);
                
            }
        }
    }
}
