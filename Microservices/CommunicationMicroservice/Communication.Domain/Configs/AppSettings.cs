namespace Communication.Domain.Configs
{
    public class AppSettings
    {
        public EmailConfiguration EmailConfiguration { get; set; }
        public Smtp Smtp { get; set; }
    }

    public class Smtp
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class EmailConfiguration
    {
        public string RecieveCompany { get; set; }
        public string RecieveDepartment { get; set; }
        public string RecieverEmailAddress { get; set; }
    }
}