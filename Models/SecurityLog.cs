namespace IT15_Trojan_B.Models
{
    public class SecurityLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string Action { get; set; }
        public string IPAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
