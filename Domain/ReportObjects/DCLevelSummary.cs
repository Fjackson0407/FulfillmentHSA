using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ReportObjects
{
   public  class DCLevelSummary
    {

        public List<WeeklySummaryView> VisaMCTotals { get; set; }
        public List<WeeklySummaryView> AmexTotals { get; set; }
    }
}
