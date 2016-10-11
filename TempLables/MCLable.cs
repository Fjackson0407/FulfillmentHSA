using Domain;
using EDIException;
using Helpers;
using LumenWorks.Framework.IO.Csv;
using Repository.DataSource;
using Repository.UOW;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Helpers.EDIHelperFunctions;

namespace TempLables
{
    public class MCLable
    {
        public string FilePath { get; set; }

        public string ConnectionString { get; set; }

        public string NewLabelFile { get; set; }
        public MCLable(string _FilePath, string _ConnectionString, string _NewLabelFile)
        {
            if (!File.Exists(_FilePath))
            {
                throw new ExceptionsEDI(string.Format("{0}  {1}", Help, ErrorCodes.HSAError5));
            }
            FilePath = _FilePath;
            ConnectionString = _ConnectionString;
            NewLabelFile = _NewLabelFile;
        }

        public MCLable(string _ConnectionString, string _NewLabelFile)
        {
            
            ConnectionString = _ConnectionString;
            NewLabelFile = _NewLabelFile;
        }

        private ShipFromInformation LoadShipFrom()
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                ShipFromInformation cShipFromInformation = UoW.ShipFrom.Find(t => t.BillAndShipToCodes == EDIHelperFunctions.ShipFrom).FirstOrDefault();
                if (cShipFromInformation == null)
                {
                    throw new ExceptionsEDI(string.Format("{0}  {1}", Help, ErrorCodes.HSAErro41));
                }
                return cShipFromInformation;
            }
        }

        private DCInformation LoadDCAndAddress(string DcNumber)
        {
            using (var UoW = new UnitofWork(new EDIContext(ConnectionString)))
            {
                DCInformation cDCInformation = UoW.DCInfo.Find(t => t.StoreID == DcNumber).FirstOrDefault();
                if (cDCInformation == null)
                {
                    throw new ExceptionsEDI(string.Format("{0}  {1}", Help, ErrorCodes.HSAErro27));
                }
                return cDCInformation;
            }
        }



        private void SavetoFile(string cSVFiile)
        {
            using (StreamWriter sw = new StreamWriter(NewLabelFile))
            {
                sw.WriteLine(cSVFiile);
            }
        }

        private string ConvertToString(List<Label> lisSSCC)
        {
            StringBuilder cStringBuilder = new StringBuilder();
            cStringBuilder.Append(From);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Faddress);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Fcity);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Fstate);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Fzip);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(To);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Taddress);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Tcity);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Tstate);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(Tzip);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(po);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(DCLocation);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(EDIHelperFunctions.SSCC);
            cStringBuilder.Append(Comma);
            cStringBuilder.Append(StoreID);
            cStringBuilder.Append(LineBreak);

            foreach (Label label in lisSSCC)
            {
                cStringBuilder.Append(label.From);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Faddress);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Fcity);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Fstate);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.FZip);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.To);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Taddress);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Tcity);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Tstate);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.Tzip);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.PONumber);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.DcNumber);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.SSCC);
                cStringBuilder.Append(Comma);
                cStringBuilder.Append(label.OrderStoreNumber);
                cStringBuilder.Append(LineBreak);

            }
            return cStringBuilder.ToString();
        }



        public void Amex(IEnumerable<HoildayOrder> Orders)
        {

            List<Label> lisLables = new List<Label>();

            foreach (var item in Orders)
            {
                ShipFromInformation cShipFromInformation = LoadShipFrom();
                   Label cLables = new Label();
                    cLables.From = ValidUSA;
                    cLables.Faddress = cShipFromInformation.Address;
                    cLables.Fcity = cShipFromInformation.City;
                    cLables.Fstate = cShipFromInformation.State;
                    cLables.FZip = cShipFromInformation.PostalCode;
                cLables.SSCC = item.SSCC;
                cLables.PONumber = item.PO;
                cLables.OrderStoreNumber = item.Store;
                    cLables.To = TargetStores;
                string DCNumber = item.DC;
                    DCInformation cDCInformation = LoadDCAndAddress(DCNumber);
                    cLables.DcNumber = DCNumber;
                    cLables.Taddress = cDCInformation.Address;
                    cLables.Tcity = cDCInformation.City;
                    cLables.Tstate = cDCInformation.State;
                    cLables.Tzip = cDCInformation.PostalCode;
                    Label Temp = lisLables.Find(t => t.SSCC == cLables.SSCC);
                    if (Temp == null)
                    {
                        lisLables.Add(cLables);
                    }
                
            

                string CSVString = ConvertToString(lisLables);
                SavetoFile(CSVString);
            }
        }


        public void Amex()
        {

            using (CsvReader csv = new CsvReader(new StreamReader(FilePath), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))
            {
                csv.SupportsMultiline = true;
                IDataReader reader = csv;
                string sPO = string.Empty;
                List<Label> lisLables = new List<Label>();
                ShipFromInformation cShipFromInformation = LoadShipFrom();
                while (reader.Read())
                {
                    Label cLables = new Label();
                    cLables.From = ValidUSA;
                    cLables.Faddress = cShipFromInformation.Address;
                    cLables.Fcity = cShipFromInformation.City;
                    cLables.Fstate = cShipFromInformation.State;
                    cLables.FZip = cShipFromInformation.PostalCode;
                    string SSCC = reader.GetValue((int)LabelMapAmex.CartonID).ToString();
                    string First = SSCC.Remove(0, 1);
                    string last = First.Remove(First.Length - 1, 1);
                    cLables.SSCC = last;
                    cLables.PONumber = reader.GetValue((int)LabelMapAmex.PoNumber).ToString();
                    cLables.OrderStoreNumber = reader.GetValue((int)LabelMapAmex.BillToStore).ToString();
                    cLables.To = TargetStores;
                    string DCNumber = reader.GetValue((int)LabelMapAmex.PoShipToStore).ToString();
                    DCInformation cDCInformation = LoadDCAndAddress(DCNumber);
                    cLables.DcNumber = DCNumber;
                    cLables.Taddress = cDCInformation.Address;
                    cLables.Tcity = cDCInformation.City;
                    cLables.Tstate = cDCInformation.State;
                    cLables.Tzip = cDCInformation.PostalCode;
                    Label Temp = lisLables.Find(t => t.SSCC == cLables.SSCC);
                    if (Temp == null )
                    {
                        lisLables.Add(cLables);
                    }
                }

                string CSVString = ConvertToString(lisLables);
                SavetoFile(CSVString);
            }
        }



        public void MakeLabelsVisaMaster()
        {

            using (CsvReader csv = new CsvReader(new StreamReader(FilePath), true, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultDelimiter, ValueTrimmingOptions.None))
            {
                csv.SupportsMultiline = true;
                IDataReader reader = csv;
                string sPO = string.Empty;
                List<Label> lisLables = new List<Label>();
                ShipFromInformation cShipFromInformation = LoadShipFrom();
                while (reader.Read())
                {
                    Label cLables = new Label();
                    cLables.From = ValidUSA;
                    cLables.Faddress = cShipFromInformation.Address;
                    cLables.Fcity = cShipFromInformation.City;
                    cLables.Fstate = cShipFromInformation.State;
                    cLables.FZip = cShipFromInformation.PostalCode;
                    string SSCC = reader.GetValue((int)LabelMapVisa.CartonID).ToString();
                    string First = SSCC.Remove(0, 1);
                    string last = First.Remove(First.Length - 1, 1);
                    cLables.SSCC = last;
                    cLables.PONumber = reader.GetValue((int)LabelMapVisa.PoNumber).ToString();
                    cLables.OrderStoreNumber = reader.GetValue((int)LabelMapVisa.BillToStore).ToString();
                    cLables.To = TargetStores;
                    string DCNumber = reader.GetValue((int)LabelMapVisa.PoShipToStore).ToString();
                    DCInformation cDCInformation = LoadDCAndAddress(DCNumber);
                    cLables.DcNumber = DCNumber;
                    cLables.Taddress = cDCInformation.Address;
                    cLables.Tcity = cDCInformation.City;
                    cLables.Tstate = cDCInformation.State;
                    cLables.Tzip = cDCInformation.PostalCode;
                    Label Temp = lisLables.Find(t => t.SSCC == cLables.SSCC);
                    if (Temp == null)
                    {
                        lisLables.Add(cLables);
                    }
                }

                string CSVString = ConvertToString(lisLables);
                SavetoFile(CSVString);
            }
        }

        private bool CheckLength(Label cLables)
        {
            bool Result = true;

            if (cLables.From.Length > (int)LabelMappingLength.From)
                      
            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }

            if (cLables.DcNumber.Length > (int)LabelMappingLength.DC)

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro47));
            }

            if (cLables.Faddress.Length > (int)LabelMappingLength.Faddress )

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro49));
            }
            if (cLables.Fcity.Length > (int)LabelMappingLength.Fcity)

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }
            if (cLables.Fstate.Length > (int)LabelMappingLength.Fstate )

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }

            if (cLables.FZip.Length > (int)LabelMappingLength.Fzip )

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }
            if (cLables.Taddress.Length > (int)LabelMappingLength.Taddress )

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }


            if (cLables.Tcity.Length > (int)LabelMappingLength.Tcity )

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }

            if (cLables.Tstate.Length > (int)LabelMappingLength.Tstate )

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }

            if (cLables.Tzip.Length > (int)LabelMappingLength.Tzip)

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }
            if (cLables.SSCC.Length > (int)LabelMappingLength.SSCC)

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }

            if (cLables.OrderStoreNumber.Length > (int)LabelMappingLength.StoreID)

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }

            if (cLables.PONumber.Length > (int)LabelMappingLength.PO)

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }

            if (cLables.DcNumber.Length > (int)LabelMappingLength.DC )

            {
                throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAErro45));
            }

















            return Result; 
        }
    }
}
