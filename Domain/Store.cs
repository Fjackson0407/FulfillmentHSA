//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain
//{
//    public class StoreObj
//    {

//        public StoreObj()
//        {
//            StoreOrderDetail = new HashSet<StoreOrderDetail>();
//        }
//      public Guid Id { get; set; }

//        public int ASNStatus { get; set; }

//        public int PickStatus { get; set; }

//        public DateTime CancelDate { get; set; }

//        public string OrderStoreNumber { get; set; }

        
//        public string BillToAddress { get; set; }

//        public int QtyOrdered { get; set; }

//        public string  DocumentId { get; set; }

//        public string OriginalLine { get; set; } 

       
//        public Guid? EDIFK { get; set; }

//        public virtual EDI850 EDI850 { get; set; }

//        public virtual ICollection<StoreOrderDetail> StoreOrderDetail { get; set; }

//    }
//}
