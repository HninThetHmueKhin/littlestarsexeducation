# Content Management Guide

This folder contains JSON files that store all the educational content for the Little Star application. You can easily add new questions, answers, topics, and blogs by editing these files.

## Files Overview

- **`topics.json`** - Contains all learning topics
- **`questions.json`** - Contains all questions and answers
- **`blogs.json`** - Contains all blog articles

## How to Add New Questions and Answers

### Method 1: Using the Content Editor (Recommended)
1. Run the application
2. Click the "📝 Editor" button in the main interface
3. Fill in the form:
   - **Question Text**: Enter your question in English
   - **Answer**: Enter the answer in English
   - **Topic**: Select which topic this question belongs to
   - **Min/Max Age**: Set the age range for this question
4. Click "➕ Add Question"
5. Click "💾 Save to JSON" to save your changes

### Method 2: Direct JSON Editing
1. Open `questions.json` in a text editor
2. Add a new question object following this format:

```json
{
  "id": 13,
  "questionText": "question_your_new_question",
  "answer": "answer_your_new_answer",
  "topicId": 1,
  "minAge": 8,
  "maxAge": 12,
  "difficulty": "easy"
}
```

3. Make sure to:
   - Use a unique ID (increment from the last question)
   - Use translation keys for questionText and answer (add them to LanguageService.cs)
   - Set the correct topicId (1=Body Parts, 2=Personal Safety, 3=Growing Up, 4=Friendships)
   - Set appropriate age ranges

## Adding Translation Keys

After adding new questions, you need to add the translation keys to the language service:

1. Open `Services/LanguageService.cs`
2. Add your new keys to both English and Burmese sections:

**English section:**
```csharp
["question_your_new_question"] = "What is your new question?",
["answer_your_new_answer"] = "This is the answer to your new question.",
```

**Burmese section:**
```csharp
["question_your_new_question"] = "သင့်မေးခွန်းအသစ်က ဘာလဲ?",
["answer_your_new_answer"] = "သင့်မေးခွန်းအသစ်၏ အဖြေမှာ ဤသည်ဖြစ်သည်။",
```

## Adding New Topics

To add a new topic:

1. Add to `topics.json`:
```json
{
  "id": 5,
  "title": "topic_new_topic",
  "description": "topic_new_topic_desc",
  "minAge": 8,
  "maxAge": 15,
  "icon": "🆕"
}
```

2. Add translation keys to `LanguageService.cs` for both English and Burmese

## Adding New Blogs

To add a new blog:

1. Add to `blogs.json`:
```json
{
  "id": 7,
  "title": "blog_new_blog",
  "description": "blog_new_blog_desc",
  "category": "category_new_category",
  "icon": "📝",
  "content": "blog_new_blog_content"
}
```

2. Add translation keys to `LanguageService.cs` for all the new keys

## Tips

- Always use translation keys (like `question_*`, `answer_*`) instead of hardcoded text
- Test your changes by running the application
- Keep questions age-appropriate
- Use clear, simple language for children
- Make sure all JSON syntax is valid (use a JSON validator if needed)

## File Locations

- Content files: `DesktopApp/Content/`
- Language service: `DesktopApp/Services/LanguageService.cs`
- Content editor: `DesktopApp/ContentEditor.xaml`
