using ChildSafeSexEducation.Models;

namespace ChildSafeSexEducation.Services
{
    public class ContentService
    {
        private readonly List<Topic> _topics;

        public ContentService()
        {
            _topics = InitializeTopics();
        }

        public List<Topic> GetTopicsForAge(int age)
        {
            return _topics.Where(t => age >= t.MinAge && age <= t.MaxAge).ToList();
        }

        public List<Question> GetQuestionsForTopic(int topicId, int age)
        {
            var topic = _topics.FirstOrDefault(t => t.Id == topicId);
            if (topic == null) return new List<Question>();

            return topic.Questions.Where(q => age >= q.MinAge && age <= q.MaxAge).ToList();
        }

        public Question? GetQuestionById(int questionId)
        {
            return _topics.SelectMany(t => t.Questions).FirstOrDefault(q => q.Id == questionId);
        }

        private List<Topic> InitializeTopics()
        {
            return new List<Topic>
            {
                new Topic
                {
                    Id = 1,
                    Title = "Body Parts",
                    Description = "Learn about different parts of your body",
                    MinAge = 8,
                    MaxAge = 15,
                    Questions = new List<Question>
                    {
                        new Question { Id = 1, QuestionText = "What are the main parts of the human body?", Answer = "The human body has many parts including the head, torso, arms, and legs. Each part has an important job to keep you healthy and strong.", TopicId = 1, MinAge = 8, MaxAge = 12 },
                        new Question { Id = 2, QuestionText = "Why is it important to keep our bodies clean?", Answer = "Keeping our bodies clean helps prevent germs and keeps us healthy. It's important to wash regularly and take care of our skin.", TopicId = 1, MinAge = 8, MaxAge = 15 },
                        new Question { Id = 3, QuestionText = "What are private parts?", Answer = "Private parts are the parts of our body that are covered by underwear or swimsuits. These are special parts that should be kept private and only touched by yourself or a doctor when needed.", TopicId = 1, MinAge = 10, MaxAge = 15 }
                    }
                },
                new Topic
                {
                    Id = 2,
                    Title = "Personal Safety",
                    Description = "Understanding personal boundaries and safety",
                    MinAge = 8,
                    MaxAge = 15,
                    Questions = new List<Question>
                    {
                        new Question { Id = 4, QuestionText = "What should you do if someone makes you feel uncomfortable?", Answer = "If someone makes you feel uncomfortable, you should tell a trusted adult like a parent, teacher, or family member immediately. It's never your fault.", TopicId = 2, MinAge = 8, MaxAge = 15 },
                        new Question { Id = 5, QuestionText = "What are good touch and bad touch?", Answer = "Good touch makes you feel safe and comfortable, like hugs from family or a doctor's gentle examination. Bad touch makes you feel scared, uncomfortable, or confused.", TopicId = 2, MinAge = 8, MaxAge = 12 },
                        new Question { Id = 6, QuestionText = "Who can you trust to talk about personal problems?", Answer = "You can trust parents, teachers, school counselors, doctors, and other trusted adults who care about your safety and well-being.", TopicId = 2, MinAge = 10, MaxAge = 15 }
                    }
                },
                new Topic
                {
                    Id = 3,
                    Title = "Growing Up",
                    Description = "Understanding changes as you grow",
                    MinAge = 10,
                    MaxAge = 15,
                    Questions = new List<Question>
                    {
                        new Question { Id = 7, QuestionText = "What changes happen during puberty?", Answer = "Puberty is when your body starts changing from a child to an adult. You might grow taller, develop new body features, and experience emotional changes.", TopicId = 3, MinAge = 10, MaxAge = 15 },
                        new Question { Id = 8, QuestionText = "Is it normal to feel confused about changes in my body?", Answer = "Yes, it's completely normal to feel confused or worried about changes in your body. Everyone goes through these changes at their own pace.", TopicId = 3, MinAge = 10, MaxAge = 15 },
                        new Question { Id = 9, QuestionText = "When should I talk to someone about body changes?", Answer = "You should talk to a trusted adult whenever you have questions or concerns about changes in your body. It's always okay to ask questions.", TopicId = 3, MinAge = 10, MaxAge = 15 }
                    }
                },
                new Topic
                {
                    Id = 4,
                    Title = "Healthy Relationships",
                    Description = "Learning about friendships and relationships",
                    MinAge = 8,
                    MaxAge = 15,
                    Questions = new List<Question>
                    {
                        new Question { Id = 10, QuestionText = "What makes a good friend?", Answer = "A good friend is someone who is kind, respectful, honest, and makes you feel happy and safe. They listen to you and support you.", TopicId = 4, MinAge = 8, MaxAge = 15 },
                        new Question { Id = 11, QuestionText = "How do you know if someone is being mean to you?", Answer = "If someone is calling you names, excluding you, hurting you physically or emotionally, or making you feel bad about yourself, they are being mean.", TopicId = 4, MinAge = 8, MaxAge = 12 },
                        new Question { Id = 12, QuestionText = "What should you do if someone is bullying you?", Answer = "Tell a trusted adult immediately. Bullying is never okay, and adults can help stop it. You are not alone, and it's not your fault.", TopicId = 4, MinAge = 8, MaxAge = 15 }
                    }
                }
            };
        }
    }
}
