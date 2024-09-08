namespace UserServiceDAL.Helpers
{
    public class SmtpConfig
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string FromAddressEmail { get; set; }
        public string FromAddressName { get; set; }
    }
}
