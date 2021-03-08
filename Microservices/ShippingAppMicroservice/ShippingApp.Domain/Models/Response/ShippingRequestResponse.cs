namespace ShippingApp.Domain.Models
{
    public class ShippingRequestResponse
    {
        public string ShippingDeptEmail { get; set; }
        public string LogisticDeptEmail { get; set; }
        public ShippingRequestModel ShippingRequest { get; set; }
    }
}
