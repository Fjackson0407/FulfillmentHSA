using Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DataSource
{
    public class EDIContext: DbContext
    {

        public EDIContext()
        {

        }
        public EDIContext(string ConnectionString)
            :base(ConnectionString )
        {
         //  this.Configuration.LazyLoadingEnabled = false;

        }
        public virtual DbSet<DCInformation> DCInformation { get; set;  }
        public virtual DbSet<StoreInfoFromEDI850> EDI850 { get; set; }
        
         public virtual DbSet<OperatorObj> Operator { get; set; }
        
        public virtual DbSet<SkuItem> SkuItem { get; set; }
        public virtual DbSet<Carton> Carton { get; set; }

        public virtual DbSet<SerialRageNumber> SerialRageNumber { get; set;  }

        public virtual DbSet<ShipFromInformation> ShipFromInformation { get; set; }
        public virtual DbSet<ContactType> ContactType { get; set; }

        public virtual DbSet<SSCC> SSCC { get; set;  }

        public virtual DbSet<UserTable> User { get; set;  }

        public virtual  DbSet<BOLForASN> BillOfLading { get; set; }

        public virtual DbSet<UserOrders> UserOrders { get; set;  }

        
        public virtual DbSet<MaxWeightFullBox> MaxWeight { get; set; }
        public virtual DbSet<Pack> Pack { get; set; }
        public virtual DbSet<CartonType> CartonType { get; set;  }

        public virtual DbSet<ASNFileOutBound> ASNFile { get; set; }
        public virtual DbSet<ShipDate> ShipDate { get; set;  }
        public virtual DbSet<BundleWeight> BundleWeight { get; set;  }
        public virtual DbSet<EmptyBoxWeight> EmptyBoxWeight { get; set;  }
        public virtual DbSet<MinWeightForShipping> MinWeightForShipping { get; set; }
        public virtual DbSet<StoreNotes> StoreNotes { get; set;  }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //This is for packs in a carton 
            modelBuilder.Entity<StoreInfoFromEDI850>()
                .HasMany(s => s.Carton)
                .WithOptional(s => s.StoreNumber)  
                .HasForeignKey(s => s.StoreNumberFK)
                .WillCascadeOnDelete();


            modelBuilder.Entity<StoreInfoFromEDI850>()
                .HasMany(t => t.BOL)
                .WithOptional(t => t.Store)
                .HasForeignKey(t => t.StoreInfoFK)
                .WillCascadeOnDelete();



            modelBuilder.Entity<StoreInfoFromEDI850>()
                .HasMany(t => t.SerialRageNumber)
                .WithOptional(t => t.StoreInfoFromEDI850)
                .HasForeignKey(t => t.StoreInfoFromEDI850FK)
                .WillCascadeOnDelete();


            modelBuilder.Entity<StoreInfoFromEDI850>()
                .HasMany(t => t.StoreNotes )
                .WithOptional(t => t.StoreInfoFromEDI850)
                .HasForeignKey(t => t.StoreInfoFromEDI850FK)
                .WillCascadeOnDelete();


            modelBuilder.Entity<BundleWeight>()
                .HasOptional(t => t.store)
                .WithOptionalDependent();

            modelBuilder.Entity<ASNFileOutBound>()
                .HasMany(s => s.Store)
                .WithOptional(s => s.ASNFileOutBound)
                .HasForeignKey(s => s.ASNFileOutBoundFK)
                .WillCascadeOnDelete();

        

        }
        

    }
}
