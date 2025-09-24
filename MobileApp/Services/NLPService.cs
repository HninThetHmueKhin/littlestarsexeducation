using ChildSafeSexEducation.Mobile.Models;

namespace ChildSafeSexEducation.Mobile.Services
{
    public class NLPService
    {
        private readonly ContentService _contentService;

        public NLPService(ContentService contentService)
        {
            _contentService = contentService;
        }

        public string ProcessQuestion(string userInput, int age)
        {
            // Simple keyword matching for child-safe responses
            var keywords = ExtractKeywords(userInput.ToLower());
            
            // Check for inappropriate content
            if (ContainsInappropriateContent(userInput))
            {
                return "I'm here to help you learn about safe and healthy topics. Please ask about body parts, personal safety, growing up, or healthy relationships.";
            }

            // Find best matching topic
            var topics = _contentService.GetTopicsForAge(age);
            var bestMatch = FindBestTopicMatch(keywords, topics);

            if (bestMatch != null)
            {
                var questions = _contentService.GetQuestionsForTopic(bestMatch.Id, age);
                if (questions.Any())
                {
                    return $"Great question! Here are some topics about {bestMatch.Title.ToLower()} that might help:\n\n" +
                           string.Join("\n", questions.Take(3).Select(q => $"• {q.QuestionText}"));
                }
            }

            return "I'd love to help you learn! You can ask about:\n• Body parts and keeping clean\n• Personal safety and boundaries\n• Growing up and changes\n• Healthy friendships and relationships\n\nWhat would you like to know more about?";
        }

        private List<string> ExtractKeywords(string input)
        {
            var stopWords = new HashSet<string> { "what", "how", "why", "when", "where", "is", "are", "do", "does", "can", "could", "should", "the", "a", "an", "and", "or", "but", "in", "on", "at", "to", "for", "of", "with", "by" };
            
            return input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                       .Where(word => word.Length > 2 && !stopWords.Contains(word))
                       .ToList();
        }

        private Topic? FindBestTopicMatch(List<string> keywords, List<Topic> topics)
        {
            var topicKeywords = new Dictionary<string, List<string>>
            {
                ["body"] = new List<string> { "body", "parts", "clean", "wash", "skin", "private" },
                ["safety"] = new List<string> { "safe", "safety", "touch", "uncomfortable", "trust", "boundaries" },
                ["growing"] = new List<string> { "grow", "growing", "change", "puberty", "adult", "teen" },
                ["relationships"] = new List<string> { "friend", "friendship", "relationship", "bully", "mean", "kind" }
            };

            var bestMatch = topics.OrderByDescending(topic =>
            {
                var topicKey = topic.Title.ToLower().Split(' ')[0];
                if (topicKeywords.ContainsKey(topicKey))
                {
                    return keywords.Count(k => topicKeywords[topicKey].Contains(k));
                }
                return 0;
            }).FirstOrDefault();

            return bestMatch;
        }

        private bool ContainsInappropriateContent(string input)
        {
            var inappropriateWords = new HashSet<string>
            {
                "sex", "sexual", "nude", "naked", "adult", "porn", "explicit", "intimate", "romance", "love"
            };

            return inappropriateWords.Any(word => input.ToLower().Contains(word));
        }
    }
}
