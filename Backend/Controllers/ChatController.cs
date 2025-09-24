using Microsoft.AspNetCore.Mvc;
using ChildSafeSexEducation.Models;
using ChildSafeSexEducation.Services;

namespace ChildSafeSexEducation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ContentService _contentService;
        private readonly NLPService _nlpService;

        public ChatController(ContentService contentService, NLPService nlpService)
        {
            _contentService = contentService;
            _nlpService = nlpService;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            if (user.Age < 8 || user.Age > 15)
            {
                return BadRequest("Age must be between 8 and 15 years old.");
            }

            if (string.IsNullOrEmpty(user.Name))
            {
                return BadRequest("Name is required.");
            }

            return Ok(new { message = $"Welcome {user.Name}! You can now start learning about safe and healthy topics." });
        }

        [HttpGet("topics/{age}")]
        public IActionResult GetTopics(int age)
        {
            if (age < 8 || age > 15)
            {
                return BadRequest("Age must be between 8 and 15 years old.");
            }

            var topics = _contentService.GetTopicsForAge(age);
            return Ok(topics);
        }

        [HttpGet("questions/{topicId}/{age}")]
        public IActionResult GetQuestions(int topicId, int age)
        {
            if (age < 8 || age > 15)
            {
                return BadRequest("Age must be between 8 and 15 years old.");
            }

            var questions = _contentService.GetQuestionsForTopic(topicId, age);
            return Ok(questions);
        }

        [HttpGet("answer/{questionId}")]
        public IActionResult GetAnswer(int questionId)
        {
            var question = _contentService.GetQuestionById(questionId);
            if (question == null)
            {
                return NotFound("Question not found.");
            }

            return Ok(new { question = question.QuestionText, answer = question.Answer });
        }

        [HttpPost("ask")]
        public IActionResult AskQuestion([FromBody] AskQuestionRequest request)
        {
            if (request.Age < 8 || request.Age > 15)
            {
                return BadRequest("Age must be between 8 and 15 years old.");
            }

            var response = _nlpService.ProcessQuestion(request.Question, request.Age);
            return Ok(new { response });
        }
    }

    public class AskQuestionRequest
    {
        public string Question { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
