namespace ShippingApp.Domain.Models
{
    public class EmailModel : ContentEmailModel
    {
        public string Sender { get; set; }
        public string EmailAddress { get; set; }
    }
}
