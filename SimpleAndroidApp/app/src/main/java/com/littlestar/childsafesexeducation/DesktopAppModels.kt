package com.littlestar.childsafesexeducation

// Reusing DesktopApp models - converted to Kotlin
data class User(
    val name: String = "",
    val username: String = "",
    val password: String = "",
    val age: Int = 0,
    val createdAt: Long = System.currentTimeMillis(),
    val parentName: String = "",
    val parentEmail: String = "",
    val emailNotificationsEnabled: Boolean = true,
    val preferredLanguage: String = "English"
)

data class Topic(
    val id: Int = 0,
    val title: String = "",
    val description: String = "",
    val minAge: Int = 8,
    val maxAge: Int = 15,
    val icon: String = ""
)

data class Question(
    val id: Int = 0,
    val questionText: String = "",
    val answer: String = "",
    val topicId: Int = 0,
    val minAge: Int = 8,
    val maxAge: Int = 15
)

data class Blog(
    val id: Int = 0,
    val title: String = "",
    val description: String = "",
    val category: String = "",
    val icon: String = "",
    val content: String = ""
)

data class ChatMessage(
    val text: String = "",
    val isUser: Boolean = true,
    val timestamp: Long = System.currentTimeMillis()
)
