using Microsoft.EntityFrameworkCore;
using System;

namespace Core.Models
{
    public class SurveillanceContext : DbContext
    {
        public SurveillanceContext(DbContextOptions<SurveillanceContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Created)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<GroupUser>()
                .HasOne(u => u.User)
                .WithMany(gu => gu.Groups)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group>()
                .HasMany(x => x.GroupSecurityDevices)
                .WithOne(x => x.Group)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                .Property(x => x.SystemKey)
                .HasDefaultValueSql("NEWID()");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }
        public DbSet<GroupUserRole> GroupUserRoles { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<GroupSecurityDevice> GroupSecurityDevices { get; set; }
    }
}
