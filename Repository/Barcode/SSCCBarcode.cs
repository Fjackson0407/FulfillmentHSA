using Domain;
using Repository.BaseClass;
using Repository.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Helpers.EDIHelperFunctions;

namespace Repository.Barcode
{
    public class SSCCBarcode : RepositoryBase<SSCC>, ISSCCBarcode
    {

        public SSCCBarcode(EDIContext context)
            : base(context)
        { }

        public string GetNextSequenceNumber(SSCCStatus bUsed)
        {
            List<SSCC> First = this.GetAll().ToList();
            SSCC cSSCC = null;
            if (First.Count == 0)
            {
                cSSCC = new SSCC();
                {
                    cSSCC.Id = Guid.NewGuid();
                    cSSCC.DTS = DateTime.Now;
                    cSSCC.SequenceNumber = 1;
                    cSSCC.Used = (int)SSCCStatus.NotUsed;
                }

            }
            else
            {
                cSSCC = this.Find(T => T.Used == (int)bUsed).FirstOrDefault();

            }

            SSCC cSSCCNew = new SSCC();
            string sBarcode;
            sBarcode = string.Empty;
            
            
                sBarcode = SetBarcode(SSCCPostions.EXTENSIONDIGIT, SSCCPostions.COMPANYCODE, cSSCC.SequenceNumber.ToString());
                int iEven = 0;
                int iOdd = 0;
                int iPosition = 1;
                int iTotal = 0;
                int iDelta = 0;

                if (sBarcode.Length == SSCCPostions.SSCCLENGTH)
                {

                    foreach (char item in sBarcode)
                    {
                        if ((iPosition % 2) == 0)
                        {
                            iEven += int.Parse(item.ToString());
                        }
                        else
                        {
                            iOdd += int.Parse(item.ToString());
                        }
                        iPosition++;
                    }
                    iOdd *= 3;
                    iTotal = iEven + iOdd;

                    while ((iTotal % 10) != 0)
                    {
                        iTotal++;
                        iDelta++;

                    }

                }
            cSSCC.Used = (int)SSCCStatus.Used;
            if (First.Count == 0)
                {
                    
                    this.Add(cSSCC);
                }
            else
            {
                this.SaveChange();
            }

            cSSCCNew = NewSequenceNumber(cSSCC.SequenceNumber);
            this.Add(cSSCCNew);
            this.Context.SaveChanges();

                sBarcode += iDelta;
                int LengthBefore = sBarcode.Length;
                sBarcode = SSCCPostions.APPLICATINIDENTIFER + sBarcode;
                int lengthAfter = sBarcode.Length;

            

            return sBarcode;
        }

        private SSCC NewSequenceNumber(int sequenceNumber)
        {
            SSCC cSSCC = new SSCC();
            cSSCC.Id = Guid.NewGuid();
            cSSCC.Used = SSCCPostions.SCCNUMBERNOTUSED;
            cSSCC.DTS = DateTime.Now;
            cSSCC.SequenceNumber = sequenceNumber + 1;
            return cSSCC;
        }

        public int SaveChange()
        {
            return this.Context.SaveChanges();
        }

        private string SetBarcode(string ExtenstionDegit, string CompanyCode, string SequenceNumber)
        {
            string sResult = string.Empty;
            const string ZERO = "0";

            if (string.IsNullOrEmpty(ExtenstionDegit) || string.IsNullOrEmpty(CompanyCode) || string.IsNullOrEmpty(SequenceNumber))
            {
                sResult = string.Empty; //Throw error 
            }
            else
            {
                while (ExtenstionDegit.Length + CompanyCode.Length + SequenceNumber.Length != SSCCPostions.SSCCLENGTH)
                {
                    SequenceNumber =  ZERO +  SequenceNumber;
                }
                sResult = ExtenstionDegit + CompanyCode + SequenceNumber;
            }
            return sResult;
        }
    }
}
