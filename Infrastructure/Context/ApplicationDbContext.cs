using Microsoft.EntityFrameworkCore;
using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Orders.Entities;

namespace TicketApi.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<UserPermission> UserPermissions { get; set; } = null!;

        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Concert> Concerts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(u => u.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId);

                entity.HasMany(u => u.UserPermissions)
                    .WithOne()
                    .HasForeignKey(up => up.UserId);

                entity.HasMany(u => u.Orders)
                    .WithOne()
                    .HasForeignKey(o => o.UserId);
            });

            modelBuilder.Entity<UserRole>()
                    .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasMany(r => r.RolePermissions)
                    .WithOne()
                    .HasForeignKey(rp => rp.RoleId);
            });

            modelBuilder.Entity<RolePermission>()
                    .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<UserPermission>()
                    .HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasMany(o => o.Tickets)
                    .WithOne()
                    .HasForeignKey(t => t.OrderId)
                    .IsRequired(false);

                entity.Property(t => t.TotalAmount)
                    .HasPrecision(18, 2);

                entity.Property(o => o.Status)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<Concert>(entity =>
            {
                entity.HasMany(c => c.Tickets)
                    .WithOne()
                    .HasForeignKey(t => t.ConcertId);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(t => t.Price)
                    .HasPrecision(18, 2);

                entity.Property(t => t.Status)
                    .HasConversion<string>();

                entity.Property(t => t.Zone)
                    .HasConversion<string>();
            });
        }

    }
}
