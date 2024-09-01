using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop
{
    public class OrderService
    {
        private readonly ApplicationContext _context;

        public OrderService(ApplicationContext context)
        {
            _context = context;
        }

        public void AddOrder(DateTime orderDate, List<int> productIds)
        {
            var products = _context.Products.Where(p => productIds.Contains(p.Id)).ToList();
            var order = new Order
            {
                OrderDate = orderDate,
                Products = products
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void RemoveOrder(int orderId)
        {
            var order = _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                _context.Products.RemoveRange(order.Products);
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine($"Order with Id {orderId} not found.");
            }
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.Include(o => o.Products).ToList();
        }

        public Order? GetOrderById(int orderId)
        {
            return _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == orderId);
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }

        public int? OrderId { get; set; }
        public Order? Order { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Product> Products { get; set; } = new();
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Shop;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 1200m },
                new Product { Id = 2, Name = "Mouse", Price = 25m }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, OrderDate = DateTime.Now }
            );
        }
    }

    class Program
    {
        static void Main()
        {
            using (var context = new ApplicationContext())
            {
                context.Database.EnsureCreated();

                var orderService = new OrderService(context);

                var productIds = context.Products.Select(p => p.Id).ToList();
                orderService.AddOrder(DateTime.Now, productIds);

                var orders = orderService.GetAllOrders();
                foreach (var order in orders)
                {
                    Console.WriteLine($"Order {order.Id} - Date: {order.OrderDate}");
                    foreach (var product in order.Products)
                    {
                        Console.WriteLine($"  Product: {product.Name}, Price: {product.Price}");
                    }
                }

                orderService.RemoveOrder(1);
            }
        }
    }
}
