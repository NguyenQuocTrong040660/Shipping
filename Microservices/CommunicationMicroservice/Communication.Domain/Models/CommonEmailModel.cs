namespace Communication.Domain.Models
{
    public class CommonEmailModel
    {
        public string SenderEmailAddress { get; set; }
        public string RecieverEmailAddress { get; set; }
        public string BCCEmailAddress { get; set; }
        public string CCEmailAddress { get; set; }
    }
}
