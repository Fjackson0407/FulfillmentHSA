using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Domain
{
    public class StoreInfoFromEDI850 : INotifyPropertyChanged
    {
        public StoreInfoFromEDI850()
        {
            Carton = new HashSet<Carton>();
            SerialRageNumber = new HashSet<SerialRageNumber>();
        }

        public Guid Id { get; set; }

        public DateTime DTS { get; set; }

        public string CompanyCode { get; set; }

        public string UPCode { get; set; }
        public string CustomerNumber { get; set; }

        public string PONumber { get; set; }

        public virtual string POSubstring
        {
            get
            {
                string[] parts = PONumber.Split('-');
                return string.Format("{0}-{1}", parts[0], parts[1]);
            }

        }

        public virtual  string DPCI { get; set;  }
        public virtual string Label { get; set;  }
        public string ShippingLocationNumber { get; set; }

        public string VendorNumber { get; set; }

        public DateTime PODate { get; set; }

        public string ShipDate { get; set; }

        public string CancelDate { get; set; }

        public string DCNumber { get; set; }

        public int PickStatus { get; set; }

        public string OrderStoreNumber { get; set; }

        public string BillToAddress { get; set; }

        public int QtyOrdered { get; set; }

        public string DocumentId { get; set; }

        public string OriginalLine { get; set; }

        public int ASNStatus { get; set; }

        public Guid? ASNFileOutBoundFK { get; set; }

        private int _QtyPacked;

        public int QtyPacked
        {
            get { return _QtyPacked; }
            set
            {
                _QtyPacked = value;
                OnPropertyChanged(nameof(QtyPacked));
            }
        }

        public virtual ASNFileOutBound ASNFileOutBound { get; set; }
        public int CustomerLineNumber { get; set; }
        public ICollection<BOLForASN> BOL { get; set; }
        public virtual SkuItem SkuItem { get; set; }
        public Guid? SkuItemFK { get; set; }

        public virtual ICollection<Carton> Carton { get; set; }
        public virtual ICollection<SerialRageNumber> SerialRageNumber { get; set; }
        public double PkgWeight { get; set; }
        public string User { get; set; }
        public bool InUse { get; set; }
        public virtual int BundleQty
        {
            get { return QtyOrdered / 25; }
        }

        
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}

