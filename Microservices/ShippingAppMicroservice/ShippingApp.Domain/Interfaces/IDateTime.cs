using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}
