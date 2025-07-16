using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSiqa.Data;
using PSiqa.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PSiqa.Controllers
{
    public class OrdersController : Controller
    {
        private readonly SSDbContext _context;

        public OrdersController(SSDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Tank)
                .Include(o => o.Driver)
                .ToList();

            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string CustomerName, string TankName, string? DriverName, int Quantity, string Status)
        {
            if (string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(TankName) || Quantity <= 0 || string.IsNullOrWhiteSpace(Status))
            {
                ModelState.AddModelError("", "يرجى إدخال جميع البيانات المطلوبة");
                return View();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.FullName == CustomerName);
            if (customer == null)
            {
                var defaultArea = await _context.Areas.FirstOrDefaultAsync();
                if (defaultArea == null)
                {
                    ModelState.AddModelError("", "لا توجد مناطق مضافة في النظام. الرجاء إضافة منطقة أولاً.");
                    return View();
                }

                customer = new Customer
                {
                    FullName = CustomerName,
                    Phone = "غير محدد",
                    Address = "غير محدد",
                    AreaId = defaultArea.Id
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            var tank = await _context.Tanks.FirstOrDefaultAsync(t => t.TankName == TankName);
            if (tank == null)
            {
                tank = new Tank
                {
                    TankName = TankName,
                    Capacity = 100,
                    Location = "غير محدد",
                    WaterType = "غير محدد",
                    PricePerUnit = 0
                };
                _context.Tanks.Add(tank);
                await _context.SaveChangesAsync();
            }

            Driver driver = null;
            if (!string.IsNullOrWhiteSpace(DriverName))
            {
                driver = await _context.Drivers.FirstOrDefaultAsync(d => d.FullName == DriverName);
                if (driver == null)
                {
                    driver = new Driver
                    {
                        FullName = DriverName,
                        Phone = "غير محدد"
                    };
                    _context.Drivers.Add(driver);
                    await _context.SaveChangesAsync();
                }
            }

            var order = new Order
            {
                CustomerId = customer.Id,
                TankId = tank.Id,
                DriverId = driver?.Id,
                Quantity = Quantity,
                Status = Status,
                OrderTime = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Tank)
                .Include(o => o.Driver)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Tank)
                .Include(o => o.Driver)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, int Quantity, string Status, string CustomerName, string TankName, string DriverName)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Tank)
                .Include(o => o.Driver)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.FullName == CustomerName);
            if (customer == null)
            {
                var defaultArea = await _context.Areas.FirstOrDefaultAsync();
                if (defaultArea == null)
                {
                    ModelState.AddModelError("", "لا توجد مناطق مضافة في النظام.");
                    return View(order);
                }

                customer = new Customer
                {
                    FullName = CustomerName,
                    Phone = "غير محدد",
                    Address = "غير محدد",
                    AreaId = defaultArea.Id
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            var tank = await _context.Tanks.FirstOrDefaultAsync(t => t.TankName == TankName);
            if (tank == null)
            {
                tank = new Tank
                {
                    TankName = TankName,
                    Capacity = 100,
                    Location = "غير محدد",
                    WaterType = "غير محدد",
                    PricePerUnit = 0
                };
                _context.Tanks.Add(tank);
                await _context.SaveChangesAsync();
            }

            Driver driver = null;
            if (!string.IsNullOrWhiteSpace(DriverName))
            {
                driver = await _context.Drivers.FirstOrDefaultAsync(d => d.FullName == DriverName);
                if (driver == null)
                {
                    driver = new Driver
                    {
                        FullName = DriverName,
                        Phone = "غير محدد"
                    };
                    _context.Drivers.Add(driver);
                    await _context.SaveChangesAsync();
                }
            }

            order.CustomerId = customer.Id;
            order.TankId = tank.Id;
            order.DriverId = driver?.Id;
            order.Quantity = Quantity;
            order.Status = Status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Tank)
                .Include(o => o.Driver)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
