//using Entities = Communication.Domain.Entities;
using Communication.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Communication.Migration
{
    public interface ISeedCommunication
    {
        void SeedData();
    }
    public class SeedCommunication : ISeedCommunication
    {
        private readonly ICommunicationDbContext _communicationDbContext;
        public SeedCommunication(ICommunicationDbContext context)
        {
            _communicationDbContext = context;
        }

        public void SeedData()
        {
            
        }
    }
}
