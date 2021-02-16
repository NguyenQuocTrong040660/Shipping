using AutoMapper;
using System;

namespace ShippingApp.Domain.Models
{
    public abstract class AuditableEntityModel
    {
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
