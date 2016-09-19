namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DEV : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ASNFileOutBounds",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        File = c.String(),
                        DTS = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreInfoFromEDI850",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DTS = c.DateTime(nullable: false),
                        CompanyCode = c.String(),
                        UPCode = c.String(),
                        CustomerNumber = c.String(),
                        PONumber = c.String(),
                        ShippingLocationNumber = c.String(),
                        VendorNumber = c.String(),
                        PODate = c.DateTime(nullable: false),
                        ShipDate = c.String(),
                        CancelDate = c.String(),
                        DCNumber = c.String(),
                        PickStatus = c.Int(nullable: false),
                        OrderStoreNumber = c.String(),
                        BillToAddress = c.String(),
                        QtyOrdered = c.Int(nullable: false),
                        DocumentId = c.String(),
                        OriginalLine = c.String(),
                        ASNStatus = c.Int(nullable: false),
                        ASNFileOutBoundFK = c.Guid(),
                        QtyPacked = c.Int(nullable: false),
                        CustomerLineNumber = c.Int(nullable: false),
                        SkuItemFK = c.Guid(),
                        PkgWeight = c.Double(nullable: false),
                        User = c.String(),
                        InUse = c.Boolean(nullable: false),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SkuItems", t => t.SkuItemFK, cascadeDelete: true)
                .ForeignKey("dbo.ASNFileOutBounds", t => t.ASNFileOutBoundFK, cascadeDelete: true)
                .Index(t => t.ASNFileOutBoundFK)
                .Index(t => t.SkuItemFK);
            
            CreateTable(
                "dbo.BOLForASNs",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        BOLNumber = c.Int(nullable: false),
                        StoreInfoFK = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.StoreInfoFromEDI850", t => t.StoreInfoFK, cascadeDelete: true)
                .Index(t => t.StoreInfoFK);
            
            CreateTable(
                "dbo.Cartons",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Qty = c.Int(nullable: false),
                        UCC128 = c.String(),
                        Weight = c.Int(nullable: false),
                        StoreNumberFK = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreInfoFromEDI850", t => t.StoreNumberFK, cascadeDelete: true)
                .Index(t => t.StoreNumberFK);
            
            CreateTable(
                "dbo.SerialRageNumbers",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        SerialNumber = c.String(),
                        Serialbundle = c.String(),
                        StoreInfoFromEDI850FK = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.StoreInfoFromEDI850", t => t.StoreInfoFromEDI850FK, cascadeDelete: true)
                .Index(t => t.StoreInfoFromEDI850FK);
            
            CreateTable(
                "dbo.SkuItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DPCI = c.String(),
                        Brand = c.String(),
                        Product = c.String(),
                        SubProduct = c.String(),
                        DENOM = c.String(),
                        BIN = c.String(),
                        GCCardType = c.String(),
                        GCProdId = c.String(),
                        DCMSID = c.String(),
                        EmbossedLine = c.String(),
                        DEPT = c.String(),
                        Class = c.String(),
                        Item = c.String(),
                        ProductUPC = c.String(),
                        PackageUPC = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BundleWeights",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CardType = c.String(),
                        Weigtht = c.Double(nullable: false),
                        DTS = c.DateTime(nullable: false),
                        InUse = c.Boolean(nullable: false),
                        store_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreInfoFromEDI850", t => t.store_Id)
                .Index(t => t.store_Id);
            
            CreateTable(
                "dbo.CartonTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(),
                        InUse = c.Boolean(nullable: false),
                        DTS = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LastName = c.String(),
                        FirstName = c.String(),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DCInformations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        PostalCode = c.String(),
                        BillAndShipToCodes = c.String(),
                        StoreID = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmptyBoxWeights",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Weight = c.Int(nullable: false),
                        DTS = c.DateTime(nullable: false),
                        InUse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaxWeightFullBoxes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Max = c.Int(nullable: false),
                        DTS = c.DateTime(nullable: false),
                        InUse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MinWeightForShippings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MinWeight = c.Int(nullable: false),
                        DTS = c.DateTime(nullable: false),
                        InUse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OperatorObjs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                        DTS = c.DateTime(nullable: false),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Packs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Size = c.Int(nullable: false),
                        InUse = c.Boolean(nullable: false),
                        DTS = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShipDates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ASNShip = c.String(),
                        InUse = c.Boolean(nullable: false),
                        DTS = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShipFromInformations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        PostalCode = c.String(),
                        DUNSOrLocationNumber = c.String(),
                        BillAndShipToCodes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SSCCs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SequenceNumber = c.Int(nullable: false),
                        Used = c.Int(nullable: false),
                        DTS = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserTables",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                        DTS = c.DateTime(nullable: false),
                        OrdersFK = c.Guid(),
                        Orders_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserOrders", t => t.Orders_Id)
                .Index(t => t.Orders_Id);
            
            CreateTable(
                "dbo.UserOrders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TBD = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTables", "Orders_Id", "dbo.UserOrders");
            DropForeignKey("dbo.BundleWeights", "store_Id", "dbo.StoreInfoFromEDI850");
            DropForeignKey("dbo.StoreInfoFromEDI850", "ASNFileOutBoundFK", "dbo.ASNFileOutBounds");
            DropForeignKey("dbo.StoreInfoFromEDI850", "SkuItemFK", "dbo.SkuItems");
            DropForeignKey("dbo.SerialRageNumbers", "StoreInfoFromEDI850FK", "dbo.StoreInfoFromEDI850");
            DropForeignKey("dbo.Cartons", "StoreNumberFK", "dbo.StoreInfoFromEDI850");
            DropForeignKey("dbo.BOLForASNs", "StoreInfoFK", "dbo.StoreInfoFromEDI850");
            DropIndex("dbo.UserTables", new[] { "Orders_Id" });
            DropIndex("dbo.BundleWeights", new[] { "store_Id" });
            DropIndex("dbo.SerialRageNumbers", new[] { "StoreInfoFromEDI850FK" });
            DropIndex("dbo.Cartons", new[] { "StoreNumberFK" });
            DropIndex("dbo.BOLForASNs", new[] { "StoreInfoFK" });
            DropIndex("dbo.StoreInfoFromEDI850", new[] { "SkuItemFK" });
            DropIndex("dbo.StoreInfoFromEDI850", new[] { "ASNFileOutBoundFK" });
            DropTable("dbo.UserOrders");
            DropTable("dbo.UserTables");
            DropTable("dbo.SSCCs");
            DropTable("dbo.ShipFromInformations");
            DropTable("dbo.ShipDates");
            DropTable("dbo.Packs");
            DropTable("dbo.OperatorObjs");
            DropTable("dbo.MinWeightForShippings");
            DropTable("dbo.MaxWeightFullBoxes");
            DropTable("dbo.EmptyBoxWeights");
            DropTable("dbo.DCInformations");
            DropTable("dbo.ContactTypes");
            DropTable("dbo.CartonTypes");
            DropTable("dbo.BundleWeights");
            DropTable("dbo.SkuItems");
            DropTable("dbo.SerialRageNumbers");
            DropTable("dbo.Cartons");
            DropTable("dbo.BOLForASNs");
            DropTable("dbo.StoreInfoFromEDI850");
            DropTable("dbo.ASNFileOutBounds");
        }
    }
}
