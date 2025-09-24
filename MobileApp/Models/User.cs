namespace ChildSafeSexEducation.Mobile.Models
{
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
