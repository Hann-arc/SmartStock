using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.models;

namespace SmartStockAI.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<Alert> Alerts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Identity tables to use GUIDS
            builder.Entity<AppUser>(entity => entity.ToTable("Users"));
            builder.Entity<IdentityRole<Guid>>(entity => entity.ToTable("Roles"));

            // Soft Delete Global Filters
            builder.Entity<Item>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<AppUser>().HasQueryFilter(x => !x.IsDeleted);

            // Convert enums to strings
            builder.Entity<StockTransaction>()
                .Property(x => x.Type)
                .HasConversion<string>();
            
            builder.Entity<AppUser>()
                .Property(x => x.Role)
                .HasConversion<string>();

            // Configure relationships 

            //  Category -> Items (1 to many)
            builder.Entity<Category>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Category)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //  Item -> Stock transcations (1 to many)
            builder.Entity<Item>()
                .HasMany(i => i.StockTransactions)
                .WithOne(st => st.Item)
                .HasForeignKey(st => st.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            //  Item -> Alert (1 to many)
            builder.Entity<Item>()
                .HasMany(i => i.Alerts)
                .WithOne(a => a.Item)
                .HasForeignKey(a => a.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            //  User -> Stock transactions (1 to many)
            builder.Entity<AppUser>()
                .HasMany(u => u.StockTransactions)
                .WithOne(st => st.User)
                .HasForeignKey(st => st.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //  User -> Alerts (1 to many)
            builder.Entity<AppUser>()
                .HasMany(u => u.CreatedAlerts)
                .WithOne(a => a.CreatedBy)
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            //  User -> Audit logs (1 to many)
            builder.Entity<AppUser>()
                .HasMany(u => u.AuditLogs)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}