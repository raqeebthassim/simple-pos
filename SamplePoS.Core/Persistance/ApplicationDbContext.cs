using Microsoft.EntityFrameworkCore;
using SamplePoS.Core.Models;

namespace SamplePoS.Core.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=SmartPos.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = 1,
                Name = "Juices"
            },
            new Category
            {
                CategoryId = 2,
                Name = "Meals"
            },
            new Category
            {
                CategoryId = 3,
                Name = "Starters"
            });
            modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1,
                CategoryId = 1,
                BuyingPrice = 10,
                SellingPrice = 20,
                Name = "Apple Juice",
                Description = "What do you expect!"
            }
            ,
            new Product
            {
                ProductId = 2,
                CategoryId = 1,
                BuyingPrice = 10,
                SellingPrice = 30,
                Name = "Strawberry Juice",
                Description = "What do you expect!"
            },
            new Product
            {
                ProductId = 3,
                CategoryId = 1,
                BuyingPrice = 20,
                SellingPrice = 50,
                Name = "Pineapple Juice",
                Description = "What do you expect!"
            },
            new Product
            {
                ProductId = 4,
                CategoryId = 2,
                BuyingPrice = 100,
                SellingPrice = 180,
                Name = "Vegetable Rice and Curry",
                Description = "What do you expect!"
            },
            new Product
            {
                ProductId = 5,
                CategoryId = 2,
                BuyingPrice = 125,
                SellingPrice = 250,
                Name = "Chicken Rice and Curry",
                Description = "What do you expect!"
            },
            new Product
            {
                ProductId = 6,
                CategoryId = 2,
                BuyingPrice = 140,
                SellingPrice = 250,
                Name = "Beef Rice and Curry",
                Description = "What do you expect!"
            },
            new Product
            {
                ProductId = 7,
                CategoryId = 2,
                BuyingPrice = 110,
                SellingPrice = 210,
                Name = "Fish Rice and Curry",
                Description = "What do you expect!"
            },
            new Product
            {
                ProductId = 8,
                CategoryId = 3,
                BuyingPrice = 250,
                SellingPrice = 660,
                Name = "Fried Sea Food Dish",
                Description = "What do you expect!"
            },
            new Product
            {
                ProductId = 9,
                CategoryId = 3,
                BuyingPrice = 280,
                SellingPrice = 320,
                Name = "Batter fried Cuttle Fish",
                Description = "What do you expect!"
            }
            );
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    Name = "Mr.Someone",
                    ContactNumber = "0111118880"
                },
                new Customer
                {
                    CustomerId = 2,
                    Name = "Someone Else",
                    ContactNumber = "0111118880"
                },
                new Customer
                {
                    CustomerId = 3,
                    Name = "Person Three",
                    ContactNumber = "0111118880"
                },
                new Customer
                {
                    CustomerId = 4,
                    Name = "The fourth",
                    ContactNumber = "0111118880"
                }
                );
            modelBuilder.Entity<Vendor>().HasData(
                new Vendor
                {
                    VendorId = 1,
                    Name = "Vendor One",
                    ContactNumber = "0111118880"
                },
                new Vendor
                {
                    VendorId = 2,
                    Name = "TWO Vendor",
                    ContactNumber = "0111118880"
                },
                new Vendor
                {
                    VendorId = 3,
                    Name = "Third Vendor",
                    ContactNumber = "0111118880"
                },
                new Vendor
                {
                    VendorId = 4,
                    Name = "Final Vendor",
                    ContactNumber = "0111118880"
                }
                );
        }
    }
}
