using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ReportObjects
{
public   class WeeklySummaryView
    {
        public string  DCNumber { get; set; }

        public int NumberofStores { get; set; }

        public int NumberofPicks { get; set; }

        public string  CardType { get; set; }
    }
}
