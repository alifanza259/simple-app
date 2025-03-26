using Microsoft.EntityFrameworkCore;

namespace SimpleApp.Entities
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<ComCustomer> ComCustomers { get; set; }
        public DbSet<SoItem> SoItems { get; set; }
        public DbSet<SoOrder> SoOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComCustomer>(entity =>
            {
                entity.ToTable("COM_CUSTOMER");
                entity.Property(e => e.ComCustomerId).HasColumnName("COM_CUSTOMER_ID");
                entity.Property(e => e.CustomerName).HasColumnName("CUSTOMER_NAME");
            });

            modelBuilder.Entity<SoOrder>(entity =>
            {
                entity.ToTable("SO_ORDER");
                entity.Property(e => e.SoOrderId).HasColumnName("SO_ORDER_ID");
                entity.Property(e => e.OrderNo).HasColumnName("ORDER_NO");
                entity.Property(e => e.OrderDate).HasColumnName("ORDER_DATE");
                entity.Property(e => e.ComCustomerId).HasColumnName("COM_CUSTOMER_ID");
                entity.Property(e => e.Address).HasColumnName("ADDRESS").IsRequired(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ComCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SoItem>(entity =>
            {
                entity.ToTable("SO_ITEM");
                entity.Property(e => e.SoItemId).HasColumnName("SO_ITEM_ID");
                entity.Property(e => e.SoOrderId).HasColumnName("SO_ORDER_ID");
                entity.Property(e => e.ItemName).HasColumnName("ITEM_NAME");
                entity.Property(e => e.Quantity).HasColumnName("QUANTITY");
                entity.Property(e => e.Price).HasColumnName("PRICE");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.SoOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ComCustomer>()
                .HasData(
                    new ComCustomer
                    {
                        ComCustomerId = 1,
                        CustomerName = "Alpha"
                    },
                    new ComCustomer
                    {
                        ComCustomerId = 2,
                        CustomerName = "Beta"
                    },
                    new ComCustomer
                    {
                        ComCustomerId = 3,
                        CustomerName = "Gamma"
                    }
                );

            modelBuilder.Entity<SoOrder>()
                .HasData(
                    new SoOrder
                    {
                        ComCustomerId = 1,
                        OrderDate = DateTime.Now,
                        OrderNo = "Order-1001",
                        Address = "Jl Nangka",
                        SoOrderId = 1
                    },
                    new SoOrder
                    {
                        ComCustomerId = 1,
                        OrderDate = DateTime.Now,
                        OrderNo = "Order-1002",
                        Address = "Jl Kampung Rambutan",
                        SoOrderId = 2
                    },
                    new SoOrder
                    {
                        ComCustomerId = 2,
                        OrderDate = DateTime.Now,
                        OrderNo = "Order-1003",
                        Address = "Jl Pabuaran",
                        SoOrderId = 3
                    },
                    new SoOrder
                    {
                        ComCustomerId = 2,
                        OrderDate = DateTime.Now,
                        OrderNo = "Order-1004",
                        Address = "Jl Kepandaian",
                        SoOrderId = 4
                    },
                    new SoOrder
                    {
                        ComCustomerId = 3,
                        OrderDate = DateTime.Now,
                        OrderNo = "Order-1005",
                        Address = "Jl Pasuruan",
                        SoOrderId = 5
                    },
                    new SoOrder
                    {
                        ComCustomerId = 3,
                        OrderDate = DateTime.Now,
                        OrderNo = "Order-1006",
                        Address = "Jl Gajah",
                        SoOrderId = 6
                    }
                );

            modelBuilder.Entity<SoItem>()
                .HasData(
                    new SoItem
                    {
                        SoItemId = 1,
                        SoOrderId = 1,
                        ItemName = "Kursi",
                        Price = 50000,
                        Quantity = 10
                    },
                    new SoItem
                    {
                        SoItemId = 2,
                        SoOrderId = 1,
                        ItemName = "Meja",
                        Price = 100000,
                        Quantity = 5
                    },
                    new SoItem
                    {
                        SoItemId = 3,
                        SoOrderId = 2,
                        ItemName = "Tabung Gas",
                        Price = 25000,
                        Quantity = 8
                    },
                    new SoItem
                    {
                        SoItemId = 4,
                        SoOrderId = 2,
                        ItemName = "Kompor Gas",
                        Price = 200000,
                        Quantity = 3
                    },
                    new SoItem
                    {
                        SoItemId = 5,
                        SoOrderId = 3,
                        ItemName = "Jendela",
                        Price = 500000,
                        Quantity = 3
                    },
                    new SoItem
                    {
                        SoItemId = 6,
                        SoOrderId = 4,
                        ItemName = "Pintu",
                        Price = 100000,
                        Quantity = 10
                    },
                    new SoItem
                    {
                        SoItemId = 7,
                        SoOrderId = 5,
                        ItemName = "Monitor",
                        Price = 1_000_000,
                        Quantity = 2
                    },
                    new SoItem
                    {
                        SoItemId = 8,
                        SoOrderId = 6,
                        ItemName = "AC",
                        Price = 900000,
                        Quantity = 5
                    }
                );
        }
    }
}
