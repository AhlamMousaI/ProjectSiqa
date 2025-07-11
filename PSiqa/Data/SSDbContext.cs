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
    }
}
