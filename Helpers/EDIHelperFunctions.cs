﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class EDIHelperFunctions
    {

        #region Strings
        public const string MasterVisaCard = "STO08";
        public const string Help = "Please contact system administrator or call 651 348-5113  for  assistance ";
        public const string SOFTWARE = "Software";
        public const string EDI = "EDI";
        public const string CONNECTIONSTRING = "ConnectionString";
        public const string HSA = "HSA";
        public const string ASNVNSettingAMEX = "SVS AMEX O";
        public const string ASNVNSettingMC = "SVS MC";
        public const string EDIFLOER = "EDIInboundFile";
        public const string HSAFOLDEROLCATION = "EDIInboundFileHSASide";
        public const string ASNFOLDERLOCATION = "ASNFolder";
        public const string ASNTEMPFOLDERLOCATION = "ASNTempFolder";
        const string SLASH = "\\";
        public const string SoftwareNode = SOFTWARE + SLASH + EDI;
        public const string HSA_NOT_FOUND = "HSA not found";
        public const string RETAILLOCATON = "RETAIL LOCATON";
        public const string CONNECTION_STRING_NOPT_FOUND = "SQL Connection not found";
        public const string PASSWORD_NOT_FOUND = "Passwowrd not found";
        public const string USERNAME_NOT_FOUND = "Username not found";
        public const string INBOUND_FOLDER_NOT_FOUND = "Folder location not feonud";
        public const string ASN_FOLDER_NOT_FOUND = "Folder location for ASN not feonud";
        public const string ASN_TEMP_FOLDER_NOT_FOUND = "Folder location for temp not feonud";
        public const string EMAIL_ADDRESS_NOT_FOUND = "Email Address not found";
        public const string RECIPIENTS_NOT_FOUNND = "Recipients not found";
        public const string SMTP_NOT_FOUND = "SMTP not found";
        public const string USERNAME = "UserName";
        public const string PASSWORD = "Password";
        public const string EMAILADDRESS = "EmailAddress";
        public const string SMTP = "SMTP";
        public const string RECIPIENTS = "Recipients";
        public const string DepartmentNumberString = "DepartmentNumber";
        public const string DepartmentNumber = "290";
        public const string DivisionNumberString = "DivisionNumber";
        public const string DivisionNumber = "290";
        public const string Weight = "WT";
        public const string ReleaseNumberString = "ReleaseNumber";
        public const string ReleaseNumber = "D";
        public const string dotNetFourPath = "Microsoft";
        public const string KEY2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\VALID USA";
        public const string PickPackStructure = "PickPackStructure";
        public const string SVSOLVIS = "SVS OL VIS";

        #endregion

        #region "856 Upload xml"
        public const string Measurement = "Measurement";
        public const string MeasureQual = "MeasureQual";
        public const string MeasureValue = "MeasureValue";
        public const string FILE = "File";
        public const string DOCUMENT = "Document";
        public const string HEADER = "Header";
        public const string SHIPMENTLEVEL = "ShipmentLevel";
        public const string ORDERLEVEL = "OrderLevel";
        public const string PICKPACKSTRUTURE = "PickPackStructure";
        public const string CARTON = "Carton";
        public const string ITEM = "Item";
        public const string ITEMS = "Items";
        public const string CUSTOMERNUMBER = "CustomerNumber";
        public const string DocumentType = "DocumentType";
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
        public const string VisaMastercard = "VisaMastercard";
        public const string Amex = "Amex";
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
        public const string FOBLocationQualifier = "FOBLocationQualifier";
        public const string TransactionSetPurposeCode = "TransactionSetPurposeCode";
        public const string Inners = "Inners";
        public const string CompanyCode = "CompanyCode";
        public const string Direction = "Direction";
        public const string Outbound = "Outbound";
        public const string Footprint = "Footprint";
        public const int shiplength = 8;
        public const int BOLlength = 10;
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
       // public const string CartonType = "CTN25";
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
       // public const int InnersPerPacksSize = 25;
        //public const int InnersPerPacksSizeInt = 25;
        public const double VisaMasterCardBundleWeight = .95;
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
        public const string ShipmentTotalCubeValue = "0.0000";
        public const string TransportationMethod = "TransportationMethod";
        public const string TransportationMethodType = "U";
        public const string FOBDescription2 = "Saint Paul  MN";
        public const string FOBDescription = "FOBDescription";
        public const int PACKWEIGHT = 1;
        //public const int MAXLBS = 60;
        //public const double MINLBS = 2;
        //public const int BOXWEIGHT = 1;

        public const string ErrorCode = "Error Code";
        public const string Space = " ";
        public const string Case1 = "1";
        public const string toFormat = "yyyy-MM-dd";
        public const string OrderTotalWeight = "OrderTotalWeight";

        public const string ValidUSA = "Valid USA";
        public const string ShipFrom = "SF";
        public const string Carrier = "Carrier";
        public const string CarrierCode = "CarrierCode";
        public const string CarrierType = "CarrierType";
        public const string BillOfLadingNumber = "BillOfLadingNumber";
        public const string NFIL = "NFIL";
        public const string CarrierTypeVaule = "M";



        #region For CSV file 

        public const string From = "From";
        public const string Item = "Item";
        public const string Description = "Description";
        public const string Cards = "Cards";

        public const string Faddress = "Faddress";
        public const string Fcity = "Fcity";
        public const string Fstate = "Fstate";
        public const string Fzip = "Fzip";
        public const string To = "To";
        public const string Taddress = "Taddress";
        public const string Tcity = "Tcity";
        public const string Tstate = "Tstate";
        public const string Tzip = "Tzip";
        public const string po = "po";
        public const string StoreID = "StoreID";
        public const string SSCC = "SSCC";
        public const string DCLocation = "DCLocation";
        public const string Comma = ",";
        public const string LineBreak = "\n";
        public const string STORE = "Store";
        public const string DPCIFORMAT = "DPCI 4 digit";
        public const string DPCI = "DPCI";
        public const string PoNumber = "PoNumber";
        public const string Bundles = "Bundles";
        public const string ItemWeight = "ItemWeight";
        public const string UPC = "UPC";
        public const string BOLSummary = "BOL Summary";
        public const string Boxes = "Boxes";
        public const string Lbs = "Lbs.";
        public const string QuantityOrdered = "QuantityOrdered";



        public const string PKGS = "Pkgs";
        #endregion


        public const  double AmexWeight = .70;
        public const  double MCVisaWeight = .95;

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
            HSAError2 = 2, //Wrong registry hive 
            HSAError3 = 3, //Path to the folder could not be found 
            HSAError4 = 4, //File not found 
            HSAError5 = 5, //csv error
            HSAErro7 = 7, //csv error with data  reader 
            HSAErro9 = 9, //duplicate PO 
            HSAErro11 = 11, //No connection to Database
            HSAErro13 = 13, //Missing company code 
            HSAErro15 = 15, //Missing company qty
            HSAErro17 = 17, //Missing Vendor code 
            HSAErro19 = 19, //Missing Contact info 
            HSAErro21 = 21, //Missing rchange 
            HSAErro23 = 23, //Missing  SKU
            HSAErro25 = 25, //Missing  SSCCC sequence 
            HSAErro27 = 27, //Missing  DC number/address
            HSAErro29 = 29, //Missing    OrderCases
            HSAErro31 = 31, //Missing    shipping location 
            HSAErro33 = 33, //Missing    Sku File 
            HSAErro37 = 37, //Missing Store number 
            HSAErro39 = 39, //Missing Shipping Location number 
            HSAErro41 = 41, //Missing ship from address 
            HSAErro43 = 43, //Missing ship to address 
            HSAErro45 = 45, //Missing  doc Id
            HSAErro47 = 47, //Missing CustomerNumber
            HSAErro49 = 49, //Missing PO Date
            HSAErro51 = 51, //Bad PO Format
            HSAErro53 = 53, //Missing OrderStoreNumber
            HSAErro55 = 55, //Already haas ASN
            HSAErro57 = 57, //From lerngth out of rage 
            HSAErro59 = 59, //No orders to process 

            

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

        public enum LabelMappingLength : int
        {

            From = 15,
            Faddress = 20,
            Fcity = 10,
            Fstate = 2,
            Fzip = 10,
            PO = 20,
            SSCC = 20,
            StoreID = 5,
            To = 15,
            Taddress = 30,
            Tcity = 15,
            Tstate = 2,
            Tzip = 10,
            DC = 5,
        }


        public enum SSCCStatus : int
        {
            NotUsed = 0,
            Used = 1,

        }
        public enum ShippingInfoType : int
        {
            BoxCount = 1,
            ShippingWeight = 2,
            PickCount = 3,

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

        public enum TargetProductMappingStockPile : int

        {
            DPCI = 2,
            Product = 0,
            Iten = 12,
            ProductUPC = 3,
            PackageUPC = 4,

        }



        public enum UPCMapping : int
        {
            UPC = 1,
            DPCI = 0,
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
            DPCI = 23,
            QtyOrdered = 24,
            UnitofMeasure = 32,
            UnitPrice = 34,
            BillToAddress = 50,
            ShipToAddress = 51,
            DocumentId = 52,
            OriginalLine = 63,
        }


        public enum LabelMapAmex : int
        {

            PurchaseOrderID = 15,
            PoNumber = 16,
            BillToStore = 18,
            PoShipToStore = 19,
            CartonID = 27,



        }



        public enum LabelMapVisa : int
        {

            PurchaseOrderID = 69,
            PoNumber = 70,
            BillToStore = 92,
            PoShipToStore = 44,
            CartonID = 128,



        }

        public class SSCCPostions
        {
            public const string COMPANYCODE = "0190829";
            public const string APPLICATINIDENTIFER = "00";
            public const string EXTENSIONDIGIT = "0";
            public const int SSCCLENGTH = 17;
            public const int SCCNUMBERNOTUSED = 0;


        }

        public class Brands
        {
            public const string MasterCard = "MasterCard";
            public const string Visa = "Visa";
            public const string AmericanExpress = "American Express";

        }

    }
}
