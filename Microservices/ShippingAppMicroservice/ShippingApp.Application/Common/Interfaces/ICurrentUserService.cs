using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
    }
}
