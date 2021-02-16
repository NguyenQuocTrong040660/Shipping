using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.CommonEntities;
using System;
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
                        Value = "2"
                    },
                    new Config
                    {
                        Key = "ShippingDeptEmail",
                        Value = "shipping@gmail.com"
                    },
                    new Config
                    {
                        Key = "LogisticDeptEmail",
                        Value = "logistic@gmail.com"
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

                var products = new List<Product>();

                for (int i = 0; i < 30; i++)
                {
                    products.Add(fake.Generate());
                }

                await context.Products.AddRangeAsync(products);
            }

            await context.SaveChangesAsync();
        }
    }
}
