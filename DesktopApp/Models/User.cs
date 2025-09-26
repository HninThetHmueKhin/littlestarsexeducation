using System.Text.Json.Serialization;

namespace ChildSafeSexEducation.Desktop.Models
{
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Parent/Guardian Information for Email Logs
        public string ParentName { get; set; } = string.Empty;
        public string ParentEmail { get; set; } = string.Empty;
        public bool EmailNotificationsEnabled { get; set; } = true;
        public string PreferredLanguage { get; set; } = "English";
        
        // JSON serialization properties
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
