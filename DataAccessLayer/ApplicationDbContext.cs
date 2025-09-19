using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<ProductStatistic> ProductStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Unique constraint on Category.Name
            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Unique index on Product.SKU
            builder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            // Index frequently queried fields
            builder.Entity<Product>()
                .HasIndex(p => p.CategoryId);

            builder.Entity<Product>()
                .HasIndex(p => p.QuantityOnHand);

            // Configure decimal precision
            builder.Entity<Product>().Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Entity<Purchase>().Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
            builder.Entity<PurchaseItem>().Property(pi => pi.LineTotal).HasColumnType("decimal(18,2)");
            builder.Entity<Sale>().Property(s => s.TotalAmount).HasColumnType("decimal(18,2)");
            builder.Entity<SaleItem>().Property(si => si.LineTotal).HasColumnType("decimal(18,2)");

            // Configure relationships
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Purchase>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Purchases)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.PurchaseItems)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SaleItem>()
                .HasOne(si => si.Product)
                .WithMany(p => p.SaleItems)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InventoryTransaction>()
                .HasOne(it => it.Product)
                .WithMany(p => p.InventoryTransactions)
                .HasForeignKey(it => it.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductStatistic>()
                .HasKey(ps => ps.Id);

            // Configure concurrency token for Product
            builder.Entity<Product>()
                .Property(p => p.RowVersion)
                .IsRowVersion();

            // Seed roles
            var adminRoleId = new Guid("158e7be1-bbc2-4e23-a892-4201f2b51048");
            var staffRoleId = new Guid("6faa6333-5352-4889-9fd9-5c4bb19f3a28");

            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<Guid> { Id = staffRoleId, Name = "Staff", NormalizedName = "STAFF" }
            );
        }
    }
}
