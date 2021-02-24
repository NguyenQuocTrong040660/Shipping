using System.Threading.Tasks;
using ShippingApp.Persistence.DBContext;
using System.Linq;
using ShippingApp.Domain.Entities;
using System.Collections.Generic;
using Bogus;

namespace ShippingApp.Persistence
{
    public static class ShippingAppDbContextSeed  
    {
        public static async Task SeedConfiguration(ShippingAppDbContext context)
        {
            if (!context.Configs.Any())
            {
                var configs = new List<Config>
                {
                    new Config
                    {
                        Key = "MinShippingDay",
                        Value = "2",
                        Descriptions = "Shipping Request should be submitted before (n) days"
                    },
                    new Config
                    {
                        Key = "ShippingDeptEmail",
                        Value = "shipping@gmail.com",
                        Descriptions = "To Email of Shipping Department"
                    },
                    new Config
                    {
                        Key = "LogisticDeptEmail",
                        Value = "logistic@gmail.com",
                        Descriptions = "To Email of Logistic Department"
                    }
                };

                await context.Configs.AddRangeAsync(configs);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedData(ShippingAppDbContext context)
        {
            if (!context.Products.Any())
            {
                var fake = new Faker<Product>()
                    .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
                    .RuleFor(i => i.ProductNumber, f => f.Commerce.Ean13())
                    .RuleFor(i => i.QtyPerPackage, f => f.Random.Even(50,100))
                    .RuleFor(i => i.Notes, f => f.Commerce.ProductDescription());

                var products = new List<Product>
                {
                    new Product
                    {
                        ProductName = "Product Name 1",
                        ProductNumber = "Product Number 1",
                        QtyPerPackage = 20
                    },
                    new Product
                    {
                        ProductName = "Product Name 2",
                        ProductNumber = "Product Number 2",
                        QtyPerPackage = 50
                    },
                    new Product
                    {
                        ProductName = "Product Name 3",
                        ProductNumber = "Product Number 3",
                        QtyPerPackage = 100
                    }
                };

                for (int i = 0; i < 5; i++)
                {
                    products.Add(fake.Generate());
                }

                await context.Products.AddRangeAsync(products);
            }

            await context.SaveChangesAsync();
        }
    }
}
