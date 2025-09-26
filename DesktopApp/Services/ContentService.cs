using ChildSafeSexEducation.Desktop.Models;
using System;
using System.IO;
using System.Text.Json;

namespace ChildSafeSexEducation.Desktop.Services
{
    public class ContentService
    {
        private readonly List<Topic> _topics;
        private List<Question> _questions;
        private readonly List<Blog> _blogs;
        private readonly LanguageService _languageService;
        private readonly string _questionsFilePath;

        public ContentService()
        {
            _languageService = LanguageService.Instance;
            _topics = InitializeTopics();
            _questionsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "questions.json");
            _questions = LoadQuestionsFromFile();
            _blogs = InitializeBlogs();
        }

        public List<Topic> GetTopicsForAge(int age)
        {
            var topics = _topics.Where(t => age >= t.MinAge && age <= t.MaxAge).ToList();
            
            // Translate topic titles and descriptions
            foreach (var topic in topics)
            {
                topic.Title = _languageService.GetText(topic.Title);
                topic.Description = _languageService.GetText(topic.Description);
            }
            
            return topics;
        }

        public List<Question> GetQuestionsForTopic(int topicId, int age)
        {
            var questions = _questions.Where(q => q.TopicId == topicId && age >= q.MinAge && age <= q.MaxAge).ToList();
            
            // Translate question text and answers
            foreach (var question in questions)
            {
                question.QuestionText = _languageService.GetText(question.QuestionText);
                question.Answer = _languageService.GetText(question.Answer);
            }
            
            return questions;
        }

        public Question? GetQuestionById(int questionId)
        {
            var question = _questions.FirstOrDefault(q => q.Id == questionId);
            if (question != null)
            {
                // Translate question text and answer
                question.QuestionText = _languageService.GetText(question.QuestionText);
                question.Answer = _languageService.GetText(question.Answer);
            }
            return question;
        }

        public List<Blog> GetBlogs()
        {
            // Translate blog content
            var translatedBlogs = new List<Blog>();
            foreach (var blog in _blogs)
            {
                var translatedBlog = new Blog
                {
                    Id = blog.Id,
                    Title = _languageService.GetText(blog.Title),
                    Description = _languageService.GetText(blog.Description),
                    Category = _languageService.GetText(blog.Category),
                    Content = _languageService.GetText(blog.Content)
                };
                translatedBlogs.Add(translatedBlog);
            }
            return translatedBlogs;
        }

        public Blog? GetBlogById(int blogId)
        {
            var blog = _blogs.FirstOrDefault(b => b.Id == blogId);
            if (blog != null)
            {
                // Translate blog content
                return new Blog
                {
                    Id = blog.Id,
                    Title = _languageService.GetText(blog.Title),
                    Description = _languageService.GetText(blog.Description),
                    Category = _languageService.GetText(blog.Category),
                    Content = _languageService.GetText(blog.Content)
                };
            }
            return null;
        }

        public void RefreshQuestions()
        {
            _questions = LoadQuestionsFromFile();
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
                    Icon = "🧸"
                },
                new Topic
                {
                    Id = 2,
                    Title = "topic_personal_safety",
                    Description = "topic_personal_safety_desc",
                    MinAge = 8,
                    MaxAge = 15,
                    Icon = "🛡️"
                },
                new Topic
                {
                    Id = 3,
                    Title = "topic_growing_up",
                    Description = "topic_growing_up_desc",
                    MinAge = 10,
                    MaxAge = 15,
                    Icon = "🌱"
                },
                new Topic
                {
                    Id = 4,
                    Title = "topic_friendships",
                    Description = "topic_friendships_desc",
                    MinAge = 8,
                    MaxAge = 15,
                    Icon = "👫"
                }
            };
        }

        private List<Question> LoadQuestionsFromFile()
        {
            try
            {
                if (File.Exists(_questionsFilePath))
                {
                    var json = File.ReadAllText(_questionsFilePath);
                    var questions = JsonSerializer.Deserialize<List<Question>>(json);
                    if (questions != null)
                    {
                        Console.WriteLine($"🔍 Loaded {questions.Count} questions from file");
                        return questions;
                    }
                }
                else
                {
                    Console.WriteLine($"🔍 Questions file not found, using default questions");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading questions from file: {ex.Message}");
            }
            
            // Fallback to default questions if file doesn't exist or error occurs
            return InitializeDefaultQuestions();
        }

        private List<Question> InitializeDefaultQuestions()
        {
            return new List<Question>
            {
                new Question { Id = 1, QuestionText = "question_what_private_parts", Answer = "answer_private_parts", TopicId = 1, MinAge = 8, MaxAge = 12 },
                new Question { Id = 2, QuestionText = "question_why_keep_clean", Answer = "answer_keep_clean", TopicId = 1, MinAge = 8, MaxAge = 15 },
                new Question { Id = 3, QuestionText = "question_what_private_parts_2", Answer = "answer_private_parts_2", TopicId = 1, MinAge = 10, MaxAge = 15 },
                new Question { Id = 4, QuestionText = "question_how_stay_safe", Answer = "answer_stay_safe", TopicId = 2, MinAge = 8, MaxAge = 15 },
                new Question { Id = 5, QuestionText = "question_good_bad_touch", Answer = "answer_good_bad_touch", TopicId = 2, MinAge = 8, MaxAge = 12 },
                new Question { Id = 6, QuestionText = "question_who_trust", Answer = "answer_who_trust", TopicId = 2, MinAge = 10, MaxAge = 15 },
                new Question { Id = 7, QuestionText = "question_what_puberty", Answer = "answer_puberty", TopicId = 3, MinAge = 10, MaxAge = 15 },
                new Question { Id = 8, QuestionText = "question_confused_changes", Answer = "answer_confused_changes", TopicId = 3, MinAge = 10, MaxAge = 15 },
                new Question { Id = 9, QuestionText = "question_when_talk_changes", Answer = "answer_when_talk_changes", TopicId = 3, MinAge = 10, MaxAge = 15 },
                new Question { Id = 10, QuestionText = "question_how_make_friends", Answer = "answer_make_friends", TopicId = 4, MinAge = 8, MaxAge = 15 },
                new Question { Id = 11, QuestionText = "question_someone_mean", Answer = "answer_someone_mean", TopicId = 4, MinAge = 8, MaxAge = 12 },
                new Question { Id = 12, QuestionText = "question_bullying_help", Answer = "answer_bullying_help", TopicId = 4, MinAge = 8, MaxAge = 15 }
            };
        }

        private List<Blog> InitializeBlogs()
        {
            return new List<Blog>
            {
                new Blog { Id = 1, Title = "blog_body_awareness", Description = "blog_body_awareness_desc", Category = "category_body_parts", Icon = "🧸", Content = "blog_body_awareness_content" },
                new Blog { Id = 2, Title = "blog_safety_rules", Description = "blog_safety_rules_desc", Category = "category_personal_safety", Icon = "🛡️", Content = "blog_safety_rules_content" },
                new Blog { Id = 3, Title = "blog_growing_changes", Description = "blog_growing_changes_desc", Category = "category_growing_up", Icon = "🌱", Content = "blog_growing_changes_content" },
                new Blog { Id = 4, Title = "blog_healthy_friendships", Description = "blog_healthy_friendships_desc", Category = "category_healthy_relationships", Icon = "👫", Content = "blog_healthy_friendships_content" },
                new Blog { Id = 5, Title = "blog_talking_adults", Description = "blog_talking_adults_desc", Category = "category_personal_safety", Icon = "👨‍👩‍👧‍👦", Content = "blog_talking_adults_content" },
                new Blog { Id = 6, Title = "blog_body_boundaries", Description = "blog_body_boundaries_desc", Category = "category_personal_safety", Icon = "🚫", Content = "blog_body_boundaries_content" }
            };
        }
    }
}