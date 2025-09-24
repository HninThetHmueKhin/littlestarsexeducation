namespace ChildSafeSexEducation.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }

    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public int TopicId { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
    }
}
