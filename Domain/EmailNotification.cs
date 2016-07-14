using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidUSAEDI
{
public  class EmailNotification
    {

        public EmailNotification()
        {

            Recipients = new List<string>();
        }
        public string  UserName { get; set; }
        public string  Password  { get; set; }

        public string  EmailAddress  { get; set; }

        public string  SMTP { get; set; }

        public List<string> Recipients { get; set; }
    }

}
