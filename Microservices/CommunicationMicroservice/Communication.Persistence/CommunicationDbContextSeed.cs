using Microsoft.EntityFrameworkCore;
using Communication.Application.Interfaces;
using Communication.Domain.CommonEntities;
using System;
using System.Threading.Tasks;
using Communication.Persistence.DBContext;
using System.Linq;
//using Entities = Communication.Domain.Entities;
using System.Collections.Generic;

namespace Communication.Persistence
{
    public static class CommunicationDbContextSeed  
    {
        public static async Task SeedConfiguration(CommunicationDbContext context)
        {
            
        }
    }
}
