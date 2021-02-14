using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.CommonEntities;
using System;
using System.Threading.Tasks;
using ShippingApp.Persistence.DBContext;
using System.Linq;
using ShippingApp.Domain.Entities;
using System.Collections.Generic;

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
    }
}
