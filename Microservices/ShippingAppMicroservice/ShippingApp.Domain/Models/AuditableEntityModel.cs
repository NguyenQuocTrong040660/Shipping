using AutoMapper;
using System;

namespace ShippingApp.Domain.Models
{
    public abstract class AuditableEntityModel
    {
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
