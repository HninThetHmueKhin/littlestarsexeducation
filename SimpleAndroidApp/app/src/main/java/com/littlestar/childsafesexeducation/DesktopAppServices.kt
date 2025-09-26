package com.littlestar.childsafesexeducation

// Reusing DesktopApp services - converted to Kotlin
class ContentService {
    private val topics = initializeTopics()
    private val questions = initializeQuestions()
    private val blogs = initializeBlogs()
    
    fun getTopicsForAge(age: Int): List<Topic> {
        return topics.filter { age >= it.minAge && age <= it.maxAge }
    }
    
    fun getQuestionsForTopic(topicId: Int, age: Int): List<Question> {
        return questions.filter { it.topicId == topicId && age >= it.minAge && age <= it.maxAge }
    }
    
    fun getQuestionById(questionId: Int): Question? {
        return questions.find { it.id == questionId }
    }
    
    fun getBlogs(): List<Blog> {
        return blogs
    }
    
    private fun initializeTopics(): List<Topic> {
        return listOf(
            Topic(1, "Body Parts", "Learn about your body and keeping clean", 8, 15, "🧸"),
            Topic(2, "Personal Safety", "Important safety tips for children", 8, 15, "🛡️"),
            Topic(3, "Growing Up", "What to expect as you grow older", 10, 15, "🌱"),
            Topic(4, "Healthy Relationships", "How to build healthy friendships", 8, 15, "👫")
        )
    }
    
    private fun initializeQuestions(): List<Question> {
        return listOf(
            Question(1, "What are private parts?", "Private parts are the parts of your body that are covered by your underwear or swimsuit. These are special parts that only you should touch, and only for cleaning or health reasons.", 1, 8, 12),
            Question(2, "Why is it important to keep clean?", "Keeping clean helps prevent germs and keeps you healthy. You should wash your hands, brush your teeth, and take regular baths or showers.", 1, 8, 15),
            Question(3, "How can I stay safe?", "Always tell a trusted adult if someone makes you feel uncomfortable. Never go anywhere with strangers, and remember that your body belongs to you.", 2, 8, 15),
            Question(4, "What is a good touch vs bad touch?", "Good touch makes you feel safe and comfortable, like hugs from family. Bad touch makes you feel scared, confused, or uncomfortable. Always tell a trusted adult about bad touch.", 2, 8, 12),
            Question(5, "What is puberty?", "Puberty is when your body starts changing as you grow up. It's completely normal and happens to everyone at different times.", 3, 10, 15),
            Question(6, "How do I make good friends?", "Good friends are kind, respectful, and make you feel happy. They listen to you, support you, and treat you well.", 4, 8, 15)
        )
    }
    
    private fun initializeBlogs(): List<Blog> {
        return listOf(
            Blog(1, "Understanding Your Body", "Learn about body parts and keeping clean", "Body Parts", "🧸", "Your body is amazing and special. It's important to understand how to take care of it properly."),
            Blog(2, "Staying Safe", "Important safety tips for children", "Personal Safety", "🛡️", "Safety is very important. Always trust your feelings and tell a trusted adult if something doesn't feel right."),
            Blog(3, "Growing Up Changes", "What to expect as you grow older", "Growing Up", "🌱", "Growing up is a natural part of life. Everyone goes through changes at their own pace."),
            Blog(4, "Making Good Friends", "How to build healthy friendships", "Healthy Relationships", "👫", "Good friends make you feel happy and safe. They respect you and support you.")
        )
    }
}

class NLPService(private val contentService: ContentService) {
    fun processQuestion(userInput: String, age: Int): String {
        val keywords = extractKeywords(userInput.lowercase())
        
        if (containsInappropriateContent(userInput)) {
            return "I'm here to help you learn about safe and healthy topics. Please ask about body parts, personal safety, growing up, or healthy relationships."
        }
        
        val topics = contentService.getTopicsForAge(age)
        val bestMatch = findBestTopicMatch(keywords, topics)
        
        if (bestMatch != null) {
            val questions = contentService.getQuestionsForTopic(bestMatch.id, age)
            if (questions.isNotEmpty()) {
                return "Great question! Here are some topics about ${bestMatch.title.lowercase()} that might help:\n\n" +
                        questions.take(3).joinToString("\n") { "• ${it.questionText}" }
            }
        }
        
        return "I'd love to help you learn! You can ask about:\n• Body parts and keeping clean\n• Personal safety and boundaries\n• Growing up and changes\n• Healthy friendships and relationships\n\nWhat would you like to know more about?"
    }
    
    private fun extractKeywords(input: String): List<String> {
        val keywords = mutableListOf<String>()
        
        if (input.contains("body") || input.contains("private") || input.contains("clean")) {
            keywords.add("body")
        }
        if (input.contains("safe") || input.contains("safety") || input.contains("touch")) {
            keywords.add("safety")
        }
        if (input.contains("grow") || input.contains("change") || input.contains("puberty")) {
            keywords.add("growing")
        }
        if (input.contains("friend") || input.contains("relationship") || input.contains("bully")) {
            keywords.add("friends")
        }
        
        return keywords
    }
    
    private fun containsInappropriateContent(input: String): Boolean {
        val inappropriateKeywords = listOf("inappropriate", "explicit", "adult", "sexual")
        return inappropriateKeywords.any { input.contains(it) }
    }
    
    private fun findBestTopicMatch(keywords: List<String>, topics: List<Topic>): Topic? {
        return topics.find { topic ->
            keywords.any { keyword ->
                topic.title.lowercase().contains(keyword) || 
                topic.description.lowercase().contains(keyword)
            }
        }
    }
}

class LanguageService {
    companion object {
        val instance = LanguageService()
    }
    
    enum class Language { English, Burmese }
    
    private var currentLanguage = Language.English
    private val translations = mapOf(
        Language.English to getEnglishTranslations(),
        Language.Burmese to getBurmeseTranslations()
    )
    
    fun getText(key: String): String {
        return translations[currentLanguage]?.get(key) ?: key
    }
    
    fun setLanguage(language: Language) {
        currentLanguage = language
    }
    
    private fun getEnglishTranslations(): Map<String, String> {
        return mapOf(
            "welcome_title" to "⭐ Little Star",
            "welcome_subtitle" to "Child-Safe Education",
            "enter_name" to "What's your name?",
            "enter_age" to "How old are you?",
            "start_learning" to "Start Learning",
            "topics" to "Topics",
            "chat" to "Chat",
            "ask_question" to "Ask me anything...",
            "send" to "Send",
            "back" to "Back"
        )
    }
    
    private fun getBurmeseTranslations(): Map<String, String> {
        return mapOf(
            "welcome_title" to "⭐ ကြယ်ငယ်",
            "welcome_subtitle" to "ကလေးများအတွက် ဘေးကင်းသော ပညာရေး",
            "enter_name" to "သင့်နာမည်က ဘာလဲ?",
            "enter_age" to "သင့်အသက်က ဘယ်လောက်လဲ?",
            "start_learning" to "သင်ယူပါ",
            "topics" to "ခေါင်းစဉ်များ",
            "chat" to "စကားပြောပါ",
            "ask_question" to "မေးခွန်းမေးပါ...",
            "send" to "ပို့ပါ",
            "back" to "နောက်သို့"
        )
    }
}
