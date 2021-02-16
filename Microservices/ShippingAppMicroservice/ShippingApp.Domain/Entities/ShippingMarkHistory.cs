using ShippingApp.Domain.CommonEntities;
using System;

namespace ShippingApp.Domain.Entities
{
    public class ShippingMarkHistory
    {
        public int Id { get; set; }
        public string RefId { get; set; }
        public string ActionType { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UserName { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
