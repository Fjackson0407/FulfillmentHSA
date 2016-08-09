using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Pack
    {
        public Guid Id { get; set; }
        public int Size { get; set; }
        public bool InUse { get; set; }
        public DateTime  DTS { get; set; }


    }
}
