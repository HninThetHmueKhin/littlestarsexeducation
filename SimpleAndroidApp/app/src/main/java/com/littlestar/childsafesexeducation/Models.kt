package com.littlestar.childsafesexeducation

data class User(
    val name: String = "",
    val age: Int = 0,
    val createdAt: Long = System.currentTimeMillis()
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

data class ChatMessage(
    val text: String = "",
    val isUser: Boolean = true,
    val timestamp: Long = System.currentTimeMillis()
)
