using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // تكوين العلاقات الأساسية
            modelBuilder.Entity<TankArea>()
                .HasKey(ta => new { ta.TankId, ta.AreaId });

            modelBuilder.Entity<TankArea>()
                .HasOne(ta => ta.Tank)
                .WithMany(t => t.TankAreas)
                .HasForeignKey(ta => ta.TankId);

            modelBuilder.Entity<TankArea>()
                .HasOne(ta => ta.Area)
                .WithMany(a => a.TankAreas)
                .HasForeignKey(ta => ta.AreaId);

            // إضافة البيانات الأولية
            modelBuilder.Entity<Area>().HasData(
                new Area { Id = 1, Name = "المنطقة الشمالية" },
                new Area { Id = 2, Name = "المنطقة الشرقية" },
                new Area { Id = 3, Name = "المنطقة الغربية" }
            );

            modelBuilder.Entity<Tank>().HasData(
                new Tank
                {
                    Id = 1,
                    TankName = "خزان الشمال الرئيسي",
                    Capacity = 10000,
                    Location = "موقع شمالي",
                    WaterType = "شرب",
                    PricePerUnit = 2.5m
                },
                new Tank
                {
                    Id = 2,
                    TankName = "خزان الشرق الاحتياطي",
                    Capacity = 8000,
                    Location = "موقع شرقي",
                    WaterType = "زراعي",
                    PricePerUnit = 1.8m
                }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FullName = "محمد أحمد",
                    Phone = "0512345678",
                    Address = "شارع الملك - المنطقة الشمالية",
                    AreaId = 1
                },
                new Customer
                {
                    Id = 2,
                    FullName = "علي محمد",
                    Phone = "0598765432",
                    Address = "شارع الجامعة - المنطقة الشرقية",
                    AreaId = 2
                }
            );

            modelBuilder.Entity<Driver>().HasData(
                new Driver
                {
                    Id = 1,
                    FullName = "سائق الشمال",
                    Phone = "0501111222"
                },
                new Driver
                {
                    Id = 2,
                    FullName = "سائق الشرق",
                    Phone = "0503333444"
                }
            );

            modelBuilder.Entity<TankArea>().HasData(
                new TankArea { Id = 1, TankId = 1, AreaId = 1 },
                new TankArea { Id = 2, TankId = 2, AreaId = 2 }
            );

            // استخدام تواريخ ثابتة بدل الديناميكية
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    TankId = 1,
                    Quantity = 5,
                    Status = "تم",
                    OrderTime = new DateTime(2023, 1, 1, 10, 0, 0),
                    DriverId = 1
                },
                new Order
                {
                    Id = 2,
                    CustomerId = 2,
                    TankId = 2,
                    Quantity = 3,
                    Status = "جاري التسليم",
                    OrderTime = new DateTime(2023, 1, 2, 14, 30, 0),
                    DriverId = 2
                }
            );
        }
    }
}