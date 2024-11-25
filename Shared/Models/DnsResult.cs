namespace Shared.Models
{
    public class DnsResult
    {
        public bool Passed { get; set; }
        public string IpAddress { get; set; }
        public string Error { get; set; }
    }
}