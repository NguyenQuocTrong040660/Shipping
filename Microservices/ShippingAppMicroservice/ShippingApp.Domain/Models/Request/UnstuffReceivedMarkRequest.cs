using AutoMapper;
using ShippingApp.Domain.Enumerations;
using System;

namespace ShippingApp.Domain.Models
{
    public class UnstuffReceivedMarkRequest
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

        public string Prefix
        {
            get
            {
                return PrefixTable.ReceivedMark;
            }
        }

        public int UnstuffQuantity { get; set; }
    }
}
