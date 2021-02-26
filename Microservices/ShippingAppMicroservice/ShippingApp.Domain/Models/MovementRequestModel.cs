using AutoMapper;
using System.Collections.Generic;

namespace ShippingApp.Domain.Models
{
    public class MovementRequestModel : AuditableEntityModel
    {
        [IgnoreMap]
        public string Identifier 
        {
            get 
            {
                return string.Concat(Prefix, Id);
            }
        }

        public int Id { get; set; }
        public string Notes { get; set; }
        public string Prefix { get; set; }
        public virtual ICollection<MovementRequestDetailModel> MovementRequestDetails { get; set; }
        public virtual ICollection<ReceivedMarkModel> ReceivedMarks { get; set; }
    }
}
