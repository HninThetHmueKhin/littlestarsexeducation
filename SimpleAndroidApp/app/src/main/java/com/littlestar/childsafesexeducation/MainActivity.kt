package com.littlestar.childsafesexeducation

import android.os.Bundle
import android.view.View
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.google.android.material.button.MaterialButton
import com.google.android.material.textfield.TextInputEditText

class MainActivity : AppCompatActivity() {
    
    private lateinit var welcomeScreen: View
    private lateinit var mainScreen: View
    private lateinit var topicsScreen: View
    
    private lateinit var nameInput: TextInputEditText
    private lateinit var ageInput: TextInputEditText
    private lateinit var startButton: MaterialButton
    
    private lateinit var userGreeting: android.widget.TextView
    private lateinit var topicsButton: MaterialButton
    private lateinit var chatRecyclerView: RecyclerView
    private lateinit var chatInput: TextInputEditText
    private lateinit var sendButton: MaterialButton
    private lateinit var backToChatButton: MaterialButton
    private lateinit var topicsRecyclerView: RecyclerView
    
    private var currentUser: User? = null
    private lateinit var chatAdapter: ChatAdapter
    private lateinit var topicAdapter: TopicAdapter
    
    // Reusing DesktopApp services
    private lateinit var contentService: ContentService
    private lateinit var nlpService: NLPService
    private lateinit var languageService: LanguageService
    
    private var currentTopics = listOf<Topic>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        
        // Initialize DesktopApp services
        contentService = ContentService()
        nlpService = NLPService(contentService)
        languageService = LanguageService.instance
        
        initializeViews()
        setupAgeSpinner()
        setupRecyclerViews()
        setupClickListeners()
        
        // Add welcome message to chat
        chatAdapter.addMessage(ChatMessage(
            "Hello! I'm here to help you learn about safe and healthy topics. You can ask me questions or click the Topics button to see what we can learn about together!",
            false
        ))
    }
    
    private fun initializeViews() {
        welcomeScreen = findViewById(R.id.welcomeScreen)
        mainScreen = findViewById(R.id.mainScreen)
        topicsScreen = findViewById(R.id.topicsScreen)
        
        nameInput = findViewById(R.id.nameInput)
        ageInput = findViewById(R.id.ageInput)
        startButton = findViewById(R.id.startButton)
        
        userGreeting = findViewById(R.id.userGreeting)
        topicsButton = findViewById(R.id.topicsButton)
        chatRecyclerView = findViewById(R.id.chatRecyclerView)
        chatInput = findViewById(R.id.chatInput)
        sendButton = findViewById(R.id.sendButton)
        backToChatButton = findViewById(R.id.backToChatButton)
        topicsRecyclerView = findViewById(R.id.topicsRecyclerView)
    }
    
    private fun setupAgeSpinner() {
        val ageOptions = (8..15).map { "$it years old" }
        val adapter = ArrayAdapter(this, android.R.layout.simple_dropdown_item_1line, ageOptions)
        ageInput.setAdapter(adapter)
    }
    
    private fun setupRecyclerViews() {
        // Chat RecyclerView
        chatAdapter = ChatAdapter(mutableListOf())
        chatRecyclerView.layoutManager = LinearLayoutManager(this)
        chatRecyclerView.adapter = chatAdapter
        
        // Topics RecyclerView - using DesktopApp content service
        currentTopics = contentService.getTopicsForAge(15) // Get all topics for display
        topicAdapter = TopicAdapter(currentTopics) { topic ->
            showTopicQuestions(topic)
        }
        topicsRecyclerView.layoutManager = LinearLayoutManager(this)
        topicsRecyclerView.adapter = topicAdapter
    }
    
    private fun setupClickListeners() {
        startButton.setOnClickListener {
            startLearning()
        }
        
        topicsButton.setOnClickListener {
            showTopics()
        }
        
        sendButton.setOnClickListener {
            sendMessage()
        }
        
        backToChatButton.setOnClickListener {
            showMainScreen()
        }
        
        chatInput.setOnEditorActionListener { _, _, _ ->
            sendMessage()
            true
        }
    }
    
    private fun startLearning() {
        val name = nameInput.text.toString().trim()
        val ageText = ageInput.text.toString()
        
        if (name.isEmpty()) {
            Toast.makeText(this, getString(R.string.name_required), Toast.LENGTH_SHORT).show()
            return
        }
        
        if (ageText.isEmpty()) {
            Toast.makeText(this, getString(R.string.age_required), Toast.LENGTH_SHORT).show()
            return
        }
        
        val age = ageText.substringBefore(" ").toIntOrNull() ?: 0
        
        if (age < 8 || age > 15) {
            Toast.makeText(this, getString(R.string.age_validation), Toast.LENGTH_SHORT).show()
            return
        }
        
        // Create user using DesktopApp model
        currentUser = User(
            name = name,
            age = age,
            username = name.lowercase().replace(" ", ""),
            password = "default", // Simplified for mobile
            preferredLanguage = "English"
        )
        
        // Load age-appropriate topics using DesktopApp service
        currentTopics = contentService.getTopicsForAge(age)
        topicAdapter = TopicAdapter(currentTopics) { topic ->
            showTopicQuestions(topic)
        }
        topicsRecyclerView.adapter = topicAdapter
        
        userGreeting.text = "Hi $name! 👋"
        showMainScreen()
    }
    
    private fun showMainScreen() {
        welcomeScreen.visibility = View.GONE
        mainScreen.visibility = View.VISIBLE
        topicsScreen.visibility = View.GONE
    }
    
    private fun showTopics() {
        welcomeScreen.visibility = View.GONE
        mainScreen.visibility = View.GONE
        topicsScreen.visibility = View.VISIBLE
    }
    
    private fun showTopicQuestions(topic: Topic) {
        // Use DesktopApp service to get questions for this topic
        val questions = contentService.getQuestionsForTopic(topic.id, currentUser?.age ?: 15)
        val message = "Let's learn about ${topic.title}! ${topic.description}\n\n" +
                if (questions.isNotEmpty()) {
                    "Here are some questions you can ask:\n" + 
                    questions.take(3).joinToString("\n") { "• ${it.questionText}" }
                } else {
                    "You can ask me specific questions about this topic!"
                }
        
        chatAdapter.addMessage(ChatMessage(message, false))
        showMainScreen()
    }
    
    private fun sendMessage() {
        val message = chatInput.text.toString().trim()
        if (message.isEmpty()) return
        
        // Add user message
        chatAdapter.addMessage(ChatMessage(message, true))
        chatInput.text?.clear()
        
        // Use DesktopApp NLP service to generate response
        val response = if (currentUser != null) {
            nlpService.processQuestion(message, currentUser!!.age)
        } else {
            "Please register first to start learning!"
        }
        
        chatAdapter.addMessage(ChatMessage(response, false))
        
        // Scroll to bottom
        chatRecyclerView.scrollToPosition(chatAdapter.itemCount - 1)
    }
}
