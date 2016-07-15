using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class EDIHelperFunctions
    {

        #region Strings
        public const string Help = "Please contact system administrator or call 651 348-5113  for  assistance ";
        #endregion

        #region "856 Upload xml"
        public const string EMAILADDRESS = "EmailAddress";
        public const string FILE = "File";
        public const string DOCUMENT = "Document";
        public const string HEADER = "Header";
        public const string SHIPMENTLEVEL = "ShipmentLevel";
        public const string ORDERLEVEL = "OrderLevel";
        public const string PICKPACKSTRUTURE = "PickPackStructure";
        public const string CARTON = "Carton";
        public const string ITEM = "Item";
        public const string CUSTOMERNUMBER = "CustomerNumber";
        public const string DocumentType = "Direction";
        public const string PURCHASEORDERSOURCEID = "PurchaseOrderSourceID";
        public const string IDS = "IDs";
        public const string ORDERTOTALS = "OrderTotals";
        public const string ORDERTOTALSCASES = "OrderTotalsCases";
        public const string MARKS = "Marks";
        public const string UCC128 = "UCC128";
        public const string ITEMIDS = "ItemIDs";
        public const string IDQUALIFIER = "IdQualifier";
        public const string ID = "Id";
        public const string TARGET = "Target";
        public const string TargetStores = "TARGET STORES";
        public const string EDI856 = "856";
        public const string ASN = "ASN";
        
        public const string PALLET = "Pallet";
        public const string TARE = "Tare";
        public const string NAME = "Name";
        public const string BillAndShipToCode = "BillAndShipToCode";
        public const string DUNSQualifier = "DUNSQualifier";
        public const string ShipTo = "ST";
        
        public const string DescShipTo = "Ship To";
        public const string DescShipFrom = "Ship From";
        /// <summary>
        /// This is for the stor number 
        /// </summary>
        public const string DUNSOrLocationNumberString = "DUNSOrLocationNumber";
        public const string DUNSOrLocationNumber = "171884984";
        public const string NameComponentQualifier = "NameComponentQualifier";
        public const string Desc = "Desc";
        public const string NameComponent = "NameComponent";
        public const string CompanyName = "CompanyName";
        public const string Address = "Address";
        public const string City = "City";
        public const string State = "State";
        public const string Zip = "Zip";
        public const string ShipmentID = "ShipmentID";
        public const string ShipTime = "ShipTime";
        public const string ContactType = "ContactType";
        public const string FunctionCode = "FunctionCode";
        public const string ContactName = "ContactName";
        public const string ContactQualifier = "ContactQualifier";
        public const string PhoneEmail = "PhoneEmail";
        public const string TotalWeight = "TotalWeight";
        public const string BillOfLadingNumber = "BillOfLadingNumber";
        public const string FOBLocationQualifier = "FOBLocationQualifier";
        public const string TransactionSetPurposeCode = "TransactionSetPurposeCode";
        public const string Inners = "Inners";
        public const string CompanyCode = "CompanyCode";
        public const string Direction = "Direction";
        public const string Outbound = "Outbound";
        public const string Footprint = "Footprint";
        public const int shiplength = 8;
        public const string Version = "Version";
        public const string VersionNumber = "3.5";
        public const string ShippingLocationNumber = "ShippingLocationNumber";
        public const string InternalDocumentNumber = "InternalDocumentNumber";
        public const string ShipmentLevel = "ShipmentLevel";
        public const string ZeroForNow = "00";
        public const string DateLoop = "DateLoop";
        public const string DateQualifier = "DateQualifier";
        public const string ShipDateString = "ShipDate";
        public const string ShipDateDateQualifierNumber = "011";
        public const string DateQualifierValue = "00";
        public const string DateQualifierValueForOrderLoop = "004";
        public const string Date = "Date";
        public const string ShipmentTotals = "ShipmentTotals";
        public const string ShipmentPackagingCode = "ShipmentPackagingCode";
        public const string CartonType = "CTN25";
        public const string ShipmentTotalCases = "ShipmentTotalCases";
        public const string ShipmentTotalWeight = "ShipmentTotalWeight";
        public const string MethodOfPayment = "MethodOfPayment";
        public const string DF = "DF";
        public const string DE = "DE";
        public const string VendorCode = "VendorCode";
        public const string Email = "EM";
        public const string Phone = "PH";
        public const string PurchaseOrderDate = "PurchaseOrderDate";
        public const string StoreNumber = "StoreNumber";
        public const string OrderTotalCases = "OrderTotalCases";
        public const string Reference = "Reference";
        public const string RefQualifier = "RefQualifier";
        public const string POTYPE = "POTYPE";
        public const string RefID = "RefID";
        public const string RefType = "SA";
        public const string ManifestCreateTime = "ManifestCreateTime";
        public const string ValidNumber = "171884984";
        public const string ValidName = "Valid USA";
        public const string PurchaseOrderNumber = "PurchaseOrderNumber";
        public const string Carton = "Carton";
        public const string Marks = "Marks";
        public const string Quantities = "Quantities";
        public const string QtyQualifier = "QtyQualifier";
        public const string QtyUOM = "QtyUOM";
        public const string Each = "EA";
        public const string Qty = "Qty";
        public const string CustomerLineNumber = "CustomerLineNumber";
        public const string ItemIDs = "ItemIDs";
        public const string IdQualifier = "IdQualifier";
        public const string UP = "UP";
        public const string VN = "VN";
        public const string Id = "Id";
        public const string CU = "CU";
        public const string QtyUOMSize = "25";
        public const string PackSize = "PackSize";
        public const string one = "1";
        public const string EachesPerInner = "EachesPerInner";
        public const int EachesPerInnerInt = 1;
        public const string InnersPerPacks = "InnersPerPacks";
        public const int InnersPerPacksSize = 25;
        public const int InnersPerPacksSizeInt = 25;
        public const string ItemDescription = "ItemDescription";
        public const string SHIPPREDISTROTODC = "SHIP PREDISTRO TO DC";
        public const string VIN = "SVS Holid";
        public const string CUST = "CUST";
        public const string Country = "Country";
        public const string US = "US";
        public const string VPS = "VPS";
        public const string QtyQualifierZZ = "ZZ";
        public const int PackWeight = 1;
        public const string ShipmentTotalCube = "ShipmentTotalCube";
        public const string ShipmentTotalCubeValue = "0.000";
        public const string RETAILLOCATON = "RETAIL LOCATON";
        public const string FOBDescription = "FOBDescription";
        public const int PACKWEIGHT = 1;
        public const int MAXLBS = 60;
        public const double MINLBS = 2;
        public const int BOXWEIGHT = 1;
        
        public const string ErrorCode = "Error Code";
        public const string Space = " ";
        public const string Case1 = "1";
        public const string toFormat = "yyyy-MM-dd";
        public const string OrderTotalWeight = "OrderTotalWeight";


        public const string ShipFrom = "SF";



        /// <summary>
        /// Get ID for new ship for 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion

        #region Error codes
        public enum ErrorCodes
        {
            HSAError3 = 3, //Path to the folder could not be found 
            HSAError5 = 5, //csv error
            HSAErro7 = 7, //csv error with data  reader 
            HSAErro9 = 9, //duplicate PO 
            HSAErro11 = 11, //No connection to Database
            HSAErro13 = 13, //Missing company code 
            HSAErro15 = 15, //Missing company qty
            HSAErro17 = 17, //Missing Vendor code 
            HSAErro19 = 19, //Missing Contact info 
            HSAErro21 = 21, //Missing  Interchange 
            HSAErro23 = 23, //Missing  SKU
            HSAErro25 = 25, //Missing  SSCCC sequence 
            HSAErro27 = 27, //Missing  DC number
            HSAErro29 = 29, //Missing    OrderCases
            HSAErro31 = 31, //Missing    shipping location 
            HSAErro33 = 33, //Missing    Sku File 
            HSAErro35 = 35, //Duplicate PO's 
            HSAErro37 = 37, //Missing Store number 
            HSAErro39 = 39, //Missing Shipping Location number 
            HSAErro41 = 41, //Missing ship from address 
            HSAErro43 = 43, //Missing ship to address 
            HSAErro45 = 45, //Missing  doc Id
            HSAErro47 = 47, //Missing CustomerNumber
            HSAErro49 = 49, //Missing PO Date

        }
        #endregion

        public enum ASNStatus : int
        {
            ReadyForASN = 1, 
            HasASN = 2, 
            ErrorASN = 3
        }

        public enum PickStatus : int
        {
            Closed = 0,
            Open = 1
        }
        public enum DCAddressInfo : int
        {
            Address = 0,
            City = 1,
            State = 2,
            PostalCode = 3,
            BillAndShipToCodes = 4,
            StoreID = 5,

        }

        public enum ShippingInfoType : int
        {
            BoxCount = 1,
            ShippingWeight = 2,

        }

        public enum TargetProductMapping : int

        {
            DPCI = 0,
            Brand = 1,
            Product = 2,
            SubProduct = 3,
            DEMON = 4,
            Bin = 5,
            GcCardType = 6,
            GCProdId = 7,
            DCMSID = 8,
            EmbossedLine = 9,
            DEPT = 10,
            Class = 11,
            Iten = 12,
            ProductUPC = 13,
            PackageUPC = 14,
        }


        public enum Inbound850Mapping : int
        {
            CompanyCode = 0,
            CustomerNumber = 1,
            PONumber = 2,
            LocationNumber = 3,
            VendorNumber = 6,
            PODate = 7,
            ShipDateOrder = 8,
            CancelDate = 9,
            OrderStoreNumber = 12,
            OrderAmount = 16,
            OrderCases = 17,
            CustomerLine = 19,
            UPCCode = 22,
            QtyOrdered = 24,
            UnitofMeasure = 32,
            UnitPrice = 34,
            BillToAddress = 50,
            ShipToAddress = 51,
            DocumentId = 52,
            OriginalLine = 63,
        }


    }
}
