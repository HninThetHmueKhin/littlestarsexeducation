using ChildSafeSexEducation.Desktop.Models;
using ChildSafeSexEducation.Desktop.Services;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace ChildSafeSexEducation.Desktop
{
    public partial class ContentEditor : Window
    {
        private readonly ContentService _contentService;
        private List<Question> _questions;

        public ContentEditor()
        {
            InitializeComponent();
            _contentService = new ContentService();
            _questions = new List<Question>();
            
            LoadData();
        }

        private void LoadData()
        {
            // Load topics for combo box
            var topics = _contentService.GetTopicsForAge(15); // Get all topics
            TopicComboBox.ItemsSource = topics;
            if (topics.Any())
            {
                TopicComboBox.SelectedIndex = 0;
            }

            // Set default ages
            MinAgeComboBox.SelectedIndex = 0; // 8
            MaxAgeComboBox.SelectedIndex = 4; // 15

            // Load questions
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            try
            {
                // Get all questions from all topics
                _questions.Clear();
                for (int topicId = 1; topicId <= 4; topicId++)
                {
                    var topicQuestions = _contentService.GetQuestionsForTopic(topicId, 15);
                    _questions.AddRange(topicQuestions);
                }
                
                QuestionsListBox.ItemsSource = _questions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading questions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(QuestionTextTextBox.Text))
                {
                    MessageBox.Show("Please enter a question text.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(AnswerTextTextBox.Text))
                {
                    MessageBox.Show("Please enter an answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (TopicComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a topic.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create new question
                var newQuestion = new Question
                {
                    Id = _questions.Count > 0 ? _questions.Max(q => q.Id) + 1 : 1,
                    QuestionText = QuestionTextTextBox.Text.Trim(),
                    Answer = AnswerTextTextBox.Text.Trim(),
                    TopicId = (int)TopicComboBox.SelectedValue,
                    MinAge = int.Parse(((ComboBoxItem)MinAgeComboBox.SelectedItem).Tag.ToString()!),
                    MaxAge = int.Parse(((ComboBoxItem)MaxAgeComboBox.SelectedItem).Tag.ToString()!)
                };

                // Add to list
                _questions.Add(newQuestion);
                QuestionsListBox.ItemsSource = null;
                QuestionsListBox.ItemsSource = _questions;

                // Clear form
                QuestionTextTextBox.Text = "";
                AnswerTextTextBox.Text = "";

                MessageBox.Show("Question added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding question: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadQuestions();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Save questions to JSON file
                var questionsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "questions.json");
                var json = JsonSerializer.Serialize(_questions, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(questionsFilePath, json);
                
                MessageBox.Show($"Questions saved successfully to {questionsFilePath}!", 
                              "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving questions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}