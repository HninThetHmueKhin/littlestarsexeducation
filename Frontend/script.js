// Global variables
let currentUser = null;
let currentTopics = [];
let currentQuestions = [];
let currentTopicId = null;

// API Base URL
const API_BASE = 'http://localhost:5000/api';

// DOM Elements
const welcomeScreen = document.getElementById('welcomeScreen');
const mainScreen = document.getElementById('mainScreen');
const topicsScreen = document.getElementById('topicsScreen');
const questionsScreen = document.getElementById('questionsScreen');
const answerScreen = document.getElementById('answerScreen');

// Initialize the application
document.addEventListener('DOMContentLoaded', function() {
    initializeEventListeners();
});

function initializeEventListeners() {
    // Welcome screen
    document.getElementById('startBtn').addEventListener('click', startLearning);
    
    // Main screen
    document.getElementById('topicsBtn').addEventListener('click', showTopics);
    document.getElementById('sendBtn').addEventListener('click', sendMessage);
    document.getElementById('chatInput').addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            sendMessage();
        }
    });
    
    // Navigation buttons
    document.getElementById('backToChatBtn').addEventListener('click', showMainScreen);
    document.getElementById('backToTopicsBtn').addEventListener('click', showTopics);
    document.getElementById('backToQuestionsBtn').addEventListener('click', showQuestions);
}

async function startLearning() {
    const name = document.getElementById('userName').value.trim();
    const age = parseInt(document.getElementById('userAge').value);
    
    if (!name || !age) {
        alert('Please enter your name and select your age.');
        return;
    }
    
    if (age < 8 || age > 15) {
        alert('You must be between 8 and 15 years old to use this app.');
        return;
    }
    
    try {
        // Register user
        const response = await fetch(`${API_BASE}/chat/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ name, age })
        });
        
        if (response.ok) {
            currentUser = { name, age };
            document.getElementById('userDisplayName').textContent = name;
            showMainScreen();
        } else {
            const error = await response.json();
            alert(error.message || 'Registration failed. Please try again.');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Unable to connect to the server. Please make sure the backend is running.');
    }
}

function showMainScreen() {
    hideAllScreens();
    mainScreen.classList.add('active');
}

async function showTopics() {
    if (!currentUser) return;
    
    try {
        const response = await fetch(`${API_BASE}/chat/topics/${currentUser.age}`);
        if (response.ok) {
            currentTopics = await response.json();
            displayTopics();
            hideAllScreens();
            topicsScreen.classList.add('active');
        } else {
            alert('Unable to load topics. Please try again.');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Unable to connect to the server.');
    }
}

function displayTopics() {
    const topicsList = document.getElementById('topicsList');
    topicsList.innerHTML = '';
    
    currentTopics.forEach(topic => {
        const topicCard = document.createElement('div');
        topicCard.className = 'topic-card';
        topicCard.innerHTML = `
            <h3>${topic.title}</h3>
            <p>${topic.description}</p>
        `;
        topicCard.addEventListener('click', () => showQuestions(topic.id, topic.title));
        topicsList.appendChild(topicCard);
    });
}

async function showQuestions(topicId, topicTitle) {
    if (!currentUser) return;
    
    currentTopicId = topicId;
    document.getElementById('topicTitle').textContent = topicTitle;
    
    try {
        const response = await fetch(`${API_BASE}/chat/questions/${topicId}/${currentUser.age}`);
        if (response.ok) {
            currentQuestions = await response.json();
            displayQuestions();
            hideAllScreens();
            questionsScreen.classList.add('active');
        } else {
            alert('Unable to load questions. Please try again.');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Unable to connect to the server.');
    }
}

function displayQuestions() {
    const questionsList = document.getElementById('questionsList');
    questionsList.innerHTML = '';
    
    currentQuestions.forEach(question => {
        const questionCard = document.createElement('div');
        questionCard.className = 'question-card';
        questionCard.innerHTML = `
            <h3>${question.questionText}</h3>
        `;
        questionCard.addEventListener('click', () => showAnswer(question.id));
        questionsList.appendChild(questionCard);
    });
}

async function showAnswer(questionId) {
    try {
        const response = await fetch(`${API_BASE}/chat/answer/${questionId}`);
        if (response.ok) {
            const data = await response.json();
            document.getElementById('questionText').textContent = data.question;
            document.getElementById('answerText').textContent = data.answer;
            hideAllScreens();
            answerScreen.classList.add('active');
        } else {
            alert('Unable to load answer. Please try again.');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Unable to connect to the server.');
    }
}

async function sendMessage() {
    const input = document.getElementById('chatInput');
    const message = input.value.trim();
    
    if (!message || !currentUser) return;
    
    // Add user message to chat
    addMessageToChat(message, 'user');
    input.value = '';
    
    // Show loading
    const loadingDiv = addMessageToChat('Thinking...', 'bot');
    const loadingElement = loadingDiv.querySelector('.loading');
    
    try {
        const response = await fetch(`${API_BASE}/chat/ask`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                question: message,
                age: currentUser.age
            })
        });
        
        if (response.ok) {
            const data = await response.json();
            // Remove loading message
            loadingDiv.remove();
            // Add bot response
            addMessageToChat(data.response, 'bot');
        } else {
            loadingDiv.remove();
            addMessageToChat('Sorry, I had trouble understanding your question. Please try asking about body parts, personal safety, growing up, or healthy relationships.', 'bot');
        }
    } catch (error) {
        console.error('Error:', error);
        loadingDiv.remove();
        addMessageToChat('I\'m having trouble connecting right now. Please try again later.', 'bot');
    }
}

function addMessageToChat(message, sender) {
    const chatMessages = document.getElementById('chatMessages');
    const messageDiv = document.createElement('div');
    messageDiv.className = `message ${sender}-message`;
    
    if (message === 'Thinking...') {
        messageDiv.innerHTML = '<div class="loading"></div>';
    } else {
        // Convert line breaks to HTML
        const formattedMessage = message.replace(/\n/g, '<br>');
        messageDiv.innerHTML = `<p>${formattedMessage}</p>`;
    }
    
    chatMessages.appendChild(messageDiv);
    chatMessages.scrollTop = chatMessages.scrollHeight;
    
    return messageDiv;
}

function hideAllScreens() {
    document.querySelectorAll('.screen').forEach(screen => {
        screen.classList.remove('active');
    });
}
