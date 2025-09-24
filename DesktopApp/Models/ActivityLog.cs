namespace ChildSafeSexEducation.Desktop.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public string ChildName { get; set; } = string.Empty;
        public int ChildAge { get; set; }
        public string ActivityType { get; set; } = string.Empty; // "Topic", "Question", "Blog"
        public string ActivityId { get; set; } = string.Empty; // ID of the topic/question/blog
        public string ActivityTitle { get; set; } = string.Empty; // Title of the topic/question/blog
        public string ActivityDescription { get; set; } = string.Empty; // Description of the activity
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int TimeSpentSeconds { get; set; } = 0; // Time spent on this activity
        public string Language { get; set; } = "English"; // Language used
        public string SessionId { get; set; } = string.Empty; // To group activities in same session
    }

    public class DailyLogSummary
    {
        public string ChildName { get; set; } = string.Empty;
        public int ChildAge { get; set; }
        public string ParentName { get; set; } = string.Empty;
        public string ParentEmail { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public List<ActivityLog> Activities { get; set; } = new List<ActivityLog>();
        public int TotalTimeSpentMinutes { get; set; }
        public int TopicsViewed { get; set; }
        public int QuestionsAsked { get; set; }
        public int BlogsRead { get; set; }
        public string Language { get; set; } = "English";
    }
}
