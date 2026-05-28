namespace Mithrandir_Sentinel.Models
{
    public class SecurityAlert
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Severity { get; set; } = string.Empty;

        public string ProcessName { get; set; } = string.Empty;

        public string RemoteAddress { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
