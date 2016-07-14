using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/******************************************************************************
	* Namespace: ValidUSAEDI
	******************************************************************************/
namespace Domain
{
    /******************************************************************************
		* Class: SSCC
		******************************************************************************/
    /// <include file='Comments.xml' path='AIMDocs/AIMMembers/AIMMember[@name="this"]/summary' />
    public class SSCC
    {
        public Guid  Id { get; set; }
        public int   SequenceNumber { get; set; }
        public int   Used { get; set; }
        public DateTime  DTS { get; set; }
        
    }
}
 