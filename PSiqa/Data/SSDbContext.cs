using System;
using Microsoft.EntityFrameworkCore;
using PSiqa.Models;

namespace PSiqa.Data
{
    public class SSDbContext : DbContext
    {
        public SSDbContext(DbContextOptions<SSDbContext> options)
            : base(options) { }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<TankArea> TankAreas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TankArea>()
                .HasKey(ta => new { ta.TankId, ta.AreaId });

            modelBuilder.Entity<TankArea>()
                .HasOne(ta => ta.Tank)
                .WithMany(t => t.TankAreas)
                .HasForeignKey(ta => ta.TankId);

            modelBuilder.Entity<TankArea>()
                .HasOne(ta => ta.Area)
                .WithMany(a => a.TankArea)
                .HasForeignKey(ta => ta.AreaId);
        }
    }

}

    
