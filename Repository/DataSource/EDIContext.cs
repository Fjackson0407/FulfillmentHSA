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

        }
        public virtual DbSet<DCInformation> DCInformation { get; set;  }
        public virtual DbSet<Store> EDI850 { get; set; }
        
         public virtual DbSet<OperatorObj> Operator { get; set; }
        public virtual DbSet<StoreOrderDetail> StoreOrderDetail { get; set; }
        public virtual DbSet<SkuItem> SkuItem { get; set; }
        public virtual DbSet<Carton> Carton { get; set; }

        public virtual DbSet<SerialRageNumber> SerialRageNumber { get; set;  }

        public virtual DbSet<ShipFromInformation> ShipFromInformation { get; set; }
        public virtual DbSet<ContactType> ContactType { get; set; }

        public virtual DbSet<SSCC> SSCC { get; set;  }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //This is for packs in a carton 
            modelBuilder.Entity<Carton>()
                .HasMany(s => s.StoreOrderDetail)
                .WithOptional(s => s.Carton)
                .HasForeignKey(s => s.CartonFK)
                .WillCascadeOnDelete();


            
            modelBuilder.Entity<StoreOrderDetail>()
                .HasMany(t => t.SerialRageNumber)
                .WithOptional(j => j.StoreOrderDetail)
                .HasForeignKey(t => t.StoreOrderDetailFK)
                .WillCascadeOnDelete();


            modelBuilder.Entity<Store>()
                .HasMany(t => t.StoreOrderDetail)
                .WithOptional(j => j.Store)
                .HasForeignKey(t => t.StorFK)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Store>()
                .HasMany(t => t.BOL)
                .WithOptional(t => t.Store)
                .HasForeignKey(t => t.StorFK)
                .WillCascadeOnDelete();


            modelBuilder.Entity<SkuItem>()
                    .HasMany(t => t.Store)
                    .WithOptional(t => t.SkuItem)
                    .HasForeignKey(t => t.SkuItemFK)
                    .WillCascadeOnDelete();



        }

    }
}
