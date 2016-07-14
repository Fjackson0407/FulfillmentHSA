using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDIException
{
    public class ExceptionsEDI :Exception 
    {
        public ExceptionsEDI(string ExpString, Exception cEx)
            :base(ExpString, cEx  )
        {
           
        }
        public ExceptionsEDI(string ExpString)
            : base(ExpString)
        {
            
        }
    }
}
