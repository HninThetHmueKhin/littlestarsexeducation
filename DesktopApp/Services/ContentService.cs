using ChildSafeSexEducation.Desktop.Models;

namespace ChildSafeSexEducation.Desktop.Services
{
    public class ContentService
    {
        private readonly List<Topic> _topics;
        private readonly LanguageService _languageService;

        public ContentService()
        {
            _languageService = LanguageService.Instance;
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
                    Title = "topic_body_parts",
                    Description = "topic_body_parts_desc",
                    MinAge = 8,
                    MaxAge = 15,
                    Questions = new List<Question>
                    {
                        new Question { Id = 1, QuestionText = "question_what_private_parts", Answer = "answer_private_parts", TopicId = 1, MinAge = 8, MaxAge = 12 },
                        new Question { Id = 2, QuestionText = "question_why_keep_clean", Answer = "answer_keep_clean", TopicId = 1, MinAge = 8, MaxAge = 15 },
                        new Question { Id = 3, QuestionText = "question_what_private_parts_2", Answer = "answer_private_parts_2", TopicId = 1, MinAge = 10, MaxAge = 15 }
                    }
                },
                new Topic
                {
                    Id = 2,
                    Title = "topic_personal_safety",
                    Description = "topic_personal_safety_desc",
                    MinAge = 8,
                    MaxAge = 15,
                    Questions = new List<Question>
                    {
                        new Question { Id = 4, QuestionText = "question_how_stay_safe", Answer = "answer_stay_safe", TopicId = 2, MinAge = 8, MaxAge = 15 },
                        new Question { Id = 5, QuestionText = "question_good_bad_touch", Answer = "answer_good_bad_touch", TopicId = 2, MinAge = 8, MaxAge = 12 },
                        new Question { Id = 6, QuestionText = "question_who_trust", Answer = "answer_who_trust", TopicId = 2, MinAge = 10, MaxAge = 15 }
                    }
                },
                new Topic
                {
                    Id = 3,
                    Title = "topic_growing_up",
                    Description = "topic_growing_up_desc",
                    MinAge = 10,
                    MaxAge = 15,
                    Questions = new List<Question>
                    {
                        new Question { Id = 7, QuestionText = "question_what_puberty", Answer = "answer_puberty", TopicId = 3, MinAge = 10, MaxAge = 15 },
                        new Question { Id = 8, QuestionText = "question_confused_changes", Answer = "answer_confused_changes", TopicId = 3, MinAge = 10, MaxAge = 15 },
                        new Question { Id = 9, QuestionText = "question_when_talk_changes", Answer = "answer_when_talk_changes", TopicId = 3, MinAge = 10, MaxAge = 15 }
                    }
                },
                new Topic
                {
                    Id = 4,
                    Title = "topic_friendships",
                    Description = "topic_friendships_desc",
                    MinAge = 8,
                    MaxAge = 15,
                    Questions = new List<Question>
                    {
                        new Question { Id = 10, QuestionText = "question_how_make_friends", Answer = "answer_make_friends", TopicId = 4, MinAge = 8, MaxAge = 15 },
                        new Question { Id = 11, QuestionText = "question_someone_mean", Answer = "answer_someone_mean", TopicId = 4, MinAge = 8, MaxAge = 12 },
                        new Question { Id = 12, QuestionText = "question_bullying_help", Answer = "answer_bullying_help", TopicId = 4, MinAge = 8, MaxAge = 15 }
                    }
                }
            };
        }
    }
}
