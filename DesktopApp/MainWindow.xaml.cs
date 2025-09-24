using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChildSafeSexEducation.Desktop.Models;
using ChildSafeSexEducation.Desktop.Services;
using Microsoft.Extensions.Configuration;

namespace ChildSafeSexEducation.Desktop
{
    public partial class MainWindow : Window
    {
        private User? _currentUser;
        private List<Topic> _currentTopics = new();
        private List<Question> _currentQuestions = new();
        private int _currentTopicId = 0;
        
        private readonly ContentService _contentService;
        private readonly NLPService _nlpService;
        private readonly LanguageService _languageService;
        private readonly LoggingService _loggingService;
        private readonly UserStorageService _userStorageService;

        public MainWindow()
        {
            InitializeComponent();
            _contentService = new ContentService();
            _nlpService = new NLPService(_contentService);
            _languageService = LanguageService.Instance;
            _loggingService = new LoggingService();
            _userStorageService = new UserStorageService();
            
            // Show language selection first
            ShowLanguageSelection();
            
            // Set up daily email sending timer
            SetupDailyEmailTimer();
            
            // Load existing user data if available
            LoadExistingUserData();
        }
        
        private void SetupDailyEmailTimer()
        {
            // Set up a timer to send daily emails at 6 PM
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromHours(24); // Check every 24 hours
            timer.Tick += async (s, e) => await SendDailyLogs();
            timer.Start();
        }
        
        private async Task SendDailyLogs()
        {
            try
            {
                // Get all users with email notifications enabled
                var usersWithEmail = _userStorageService.GetUsersWithEmailNotifications();
                
                if (usersWithEmail.Any())
                {
                    await _loggingService.SendDailyLogsToAllParentsAsync(usersWithEmail);
                    Console.WriteLine($"Daily logs sent to {usersWithEmail.Count} parents");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending daily logs: {ex.Message}");
            }
        }
        
        private void LoadExistingUserData()
        {
            try
            {
                var allUsers = _userStorageService.GetAllUsers();
                if (allUsers.Any())
                {
                    Console.WriteLine($"Loaded {allUsers.Count} existing users from storage");
                    
                    // Don't pre-fill the form - let users enter fresh data
                    // Data is still saved to JSON for refresh functionality
                    Console.WriteLine("User data available for refresh, but form will start empty");
                }
                
                // Ensure form fields are completely empty
                ClearFormFields();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading existing user data: {ex.Message}");
            }
        }
        
        private void ClearFormFields()
        {
            NameTextBox.Text = "";
            AgeComboBox.SelectedIndex = -1;
            ParentNameTextBox.Text = "";
            ParentEmailTextBox.Text = "";
            EmailNotificationsCheckBox.IsChecked = false;
            Console.WriteLine("✅ Form fields cleared");
        }
        
        private void EnsureButtonVisible()
        {
            // Make sure the Start button is visible and properly positioned
            if (StartButton != null)
            {
                StartButton.Visibility = Visibility.Visible;
                StartButton.IsEnabled = true;
                Console.WriteLine("✅ Start button ensured visible");
            }
            else
            {
                Console.WriteLine("❌ Start button is null!");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("🚀 StartButton_Click called!");
            
            var name = NameTextBox.Text.Trim();
            var selectedItem = AgeComboBox.SelectedItem as ComboBoxItem;
            var parentEmail = ParentEmailTextBox.Text.Trim();
            
            Console.WriteLine($"Name: '{name}'");
            Console.WriteLine($"SelectedItem: {selectedItem?.Tag}");
            Console.WriteLine($"Parent Email: '{parentEmail}'");
            
            if (string.IsNullOrEmpty(name) || selectedItem == null || selectedItem.Tag == null)
            {
                Console.WriteLine("❌ Validation failed - missing name or age");
                ModernMessageBox.ShowWarning(_languageService.GetText("missing_name_age"), _languageService.GetText("missing_information"));
                return;
            }

            var age = int.Parse(selectedItem.Tag.ToString()!);
            Console.WriteLine($"Age: {age}");
            
            if (age < 8 || age > 15)
            {
                Console.WriteLine("❌ Invalid age");
                ModernMessageBox.ShowWarning(_languageService.GetText("invalid_age_message"), _languageService.GetText("invalid_age"));
                return;
            }

            // Validate email format and check if it's Gmail
            if (!string.IsNullOrEmpty(parentEmail))
            {
                if (!IsValidEmail(parentEmail))
                {
                    Console.WriteLine("❌ Invalid email format");
                    ModernMessageBox.ShowWarning(_languageService.GetText("invalid_email_message"), _languageService.GetText("invalid_email"));
                    return;
                }

                if (!IsGmailEmail(parentEmail))
                {
                    Console.WriteLine("❌ Not a Gmail email");
                    var result = ModernMessageBox.ShowQuestion(
                        _languageService.GetText("email_recommendation_message"), 
                        _languageService.GetText("email_recommendation"));
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        ParentEmailTextBox.Focus();
                        return;
                    }
                }
            }

            _currentUser = new User 
            { 
                Name = name, 
                Age = age,
                ParentName = ParentNameTextBox.Text.Trim(),
                ParentEmail = ParentEmailTextBox.Text.Trim(),
                EmailNotificationsEnabled = EmailNotificationsCheckBox.IsChecked ?? false,
                PreferredLanguage = _languageService.CurrentLanguage.ToString()
            };
            
            Console.WriteLine("✅ User created successfully");
            
            // Save user data to file
            _userStorageService.SaveUser(_currentUser);
            Console.WriteLine("✅ User saved to file");
            
            WelcomeText.Text = $"Hi {name}! 👋";
            
            // Add welcome message to chat
            AddMessageToChat(_languageService.GetText("welcome_chat_message"), false);
            Console.WriteLine("✅ Welcome message added to chat");
            
            Console.WriteLine("🔄 Calling ShowMainPanel()...");
            ShowMainPanel();
            Console.WriteLine("✅ ShowMainPanel() completed");
        }

        private void TopicsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null) return;
            
            _currentTopics = _contentService.GetTopicsForAge(_currentUser.Age);
            ShowTopicsTab();
            TopicsPopup.Visibility = Visibility.Visible;
            
            // Ensure topics are displayed with current language
            DisplayTopicsInPopup();
        }

        private async void SendDailyLogButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                ModernMessageBox.ShowError(_languageService.GetText("no_user_logged_in"), _languageService.GetText("error"));
                return;
            }

            if (string.IsNullOrWhiteSpace(_currentUser.ParentEmail))
            {
                ModernMessageBox.ShowError(_languageService.GetText("no_parent_email"), _languageService.GetText("error"));
                return;
            }

            try
            {
                // Show loading message
                AddMessageToChat(_languageService.GetText("sending_daily_log"), false);
                
                // Send the daily log
                await _loggingService.SendDailyLogToParentAsync(_currentUser);
                
                // Show success message
                AddMessageToChat(_languageService.GetText("daily_log_sent"), false);
                
                ModernMessageBox.ShowInfo(string.Format(_languageService.GetText("daily_log_sent_success"), _currentUser.ParentEmail), _languageService.GetText("success"));
            }
            catch (Exception ex)
            {
                // Show error message
                AddMessageToChat(string.Format(_languageService.GetText("daily_log_error"), ex.Message), false);
                
                ModernMessageBox.ShowError(string.Format(_languageService.GetText("error_sending_daily_log"), ex.Message), _languageService.GetText("error"));
            }
        }

        private async void TestEmailButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var emailService = new Services.EmailService();
                var (success, message) = await emailService.SendTestEmailAsync("jennifernovack65@gmail.com");
                
                if (success)
                {
                    ModernMessageBox.ShowInfo(string.Format(_languageService.GetText("test_email_success_message"), message), _languageService.GetText("test_email_success"));
                }
                else
                {
                    ModernMessageBox.ShowError(string.Format(_languageService.GetText("test_email_failed"), message), _languageService.GetText("test_email_error"));
                }
            }
            catch (Exception ex)
            {
                ModernMessageBox.ShowError(string.Format(_languageService.GetText("unexpected_error"), ex.Message, ex.GetType().Name), _languageService.GetText("test_email_error"));
            }
        }

        private void CloseTopicsButton_Click(object sender, RoutedEventArgs e)
        {
            TopicsPopup.Visibility = Visibility.Collapsed;
        }

        private void TopicsTab_Click(object sender, RoutedEventArgs e)
        {
            ShowTopicsTab();
        }

        private void QuestionsTab_Click(object sender, RoutedEventArgs e)
        {
            ShowQuestionsTab();
        }

        private void BlogsTab_Click(object sender, RoutedEventArgs e)
        {
            ShowBlogsTab();
        }

        private void ShowTopicsTab()
        {
            // Update tab colors
            TopicsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96));
            QuestionsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125));
            BlogsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125));
            
            TopicsContent.Children.Clear();
            
            foreach (var topic in _currentTopics)
            {
                var topicCard = new Border
                {
                    Background = System.Windows.Media.Brushes.White,
                    BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 236, 239)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(12),
                    Padding = new Thickness(20),
                    Margin = new Thickness(0, 0, 0, 15),
                    Cursor = Cursors.Hand
                };

                var stackPanel = new StackPanel();
                
                var titleText = new TextBlock
                {
                    Text = $"📚 {topic.Title}",
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 8)
                };
                
                var descText = new TextBlock
                {
                    Text = topic.Description,
                    FontSize = 14,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125)),
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 20
                };
                
                stackPanel.Children.Add(titleText);
                stackPanel.Children.Add(descText);
                topicCard.Child = stackPanel;
                
                topicCard.MouseLeftButtonDown += (s, e) => SelectTopic(topic);
                
                TopicsContent.Children.Add(topicCard);
            }
        }

        private void ShowQuestionsTab()
        {
            // Update tab colors
            TopicsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125));
            QuestionsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96));
            BlogsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125));
            
            TopicsContent.Children.Clear();
            
            var allQuestions = new List<Question>();
            foreach (var topic in _currentTopics)
            {
                var questions = _contentService.GetQuestionsForTopic(topic.Id, _currentUser.Age);
                allQuestions.AddRange(questions);
            }
            
            foreach (var question in allQuestions.Take(10)) // Show first 10 questions
            {
                var questionCard = new Border
                {
                    Background = System.Windows.Media.Brushes.White,
                    BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 236, 239)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(12),
                    Padding = new Thickness(20),
                    Margin = new Thickness(0, 0, 0, 15),
                    Cursor = Cursors.Hand
                };

                var questionText = new TextBlock
                {
                    Text = $"❓ {_languageService.GetText(question.QuestionText)}",
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 22
                };
                
                questionCard.Child = questionText;
                questionCard.MouseLeftButtonDown += (s, e) => ShowAnswerInChat(question);
                
                TopicsContent.Children.Add(questionCard);
            }
        }

        private void ShowBlogsTab()
        {
            // Update tab colors
            TopicsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125));
            QuestionsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125));
            BlogsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96));
            
            TopicsContent.Children.Clear();
            
            var blogs = new[]
            {
                new { Title = "blog_body_awareness", Description = "blog_body_awareness_desc", Category = "category_body_parts", Icon = "🧸" },
                new { Title = "blog_safety_rules", Description = "blog_safety_rules_desc", Category = "category_personal_safety", Icon = "🛡️" },
                new { Title = "blog_growing_changes", Description = "blog_growing_changes_desc", Category = "category_growing_up", Icon = "🌱" },
                new { Title = "blog_healthy_friendships", Description = "blog_healthy_friendships_desc", Category = "category_healthy_relationships", Icon = "👫" },
                new { Title = "blog_talking_adults", Description = "blog_talking_adults_desc", Category = "category_personal_safety", Icon = "👨‍👩‍👧‍👦" },
                new { Title = "blog_body_boundaries", Description = "blog_body_boundaries_desc", Category = "category_personal_safety", Icon = "🚫" }
            };
            
            foreach (var blog in blogs)
            {
                var blogCard = new Border
                {
                    Background = System.Windows.Media.Brushes.White,
                    BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 236, 239)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(12),
                    Padding = new Thickness(20),
                    Margin = new Thickness(0, 0, 0, 15),
                    Cursor = Cursors.Hand
                };

                var stackPanel = new StackPanel();
                
                var headerStack = new StackPanel { Orientation = Orientation.Horizontal };
                
                var iconText = new TextBlock
                {
                    Text = blog.Icon,
                    FontSize = 24,
                    Margin = new Thickness(0, 0, 15, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };
                
                var titleStack = new StackPanel();
                
                var categoryText = new TextBlock
                {
                    Text = _languageService.GetText(blog.Category),
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 5)
                };
                
                var titleText = new TextBlock
                {
                    Text = _languageService.GetText(blog.Title),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(44, 62, 80)),
                    TextWrapping = TextWrapping.Wrap
                };
                
                titleStack.Children.Add(categoryText);
                titleStack.Children.Add(titleText);
                
                headerStack.Children.Add(iconText);
                headerStack.Children.Add(titleStack);
                
                var descText = new TextBlock
                {
                    Text = _languageService.GetText(blog.Description),
                    FontSize = 14,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125)),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 10, 0, 0),
                    LineHeight = 18
                };
                
                stackPanel.Children.Add(headerStack);
                stackPanel.Children.Add(descText);
                blogCard.Child = stackPanel;
                
                blogCard.MouseLeftButtonDown += (s, e) => OpenBlogPage(blog.Title, blog.Description, blog.Category);
                
                TopicsContent.Children.Add(blogCard);
            }
        }

        private void BackToChatButton_Click(object sender, RoutedEventArgs e)
        {
            BlogsPanel.Visibility = Visibility.Collapsed;
            MainPanel.Visibility = Visibility.Visible;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Show confirmation dialog
            var result = ModernMessageBox.ShowConfirmation(_languageService.GetText("back_to_start_confirm"), 
                                                          _languageService.GetText("back_to_start_title"));
            
            if (result == MessageBoxResult.Yes)
            {
                // Reset user data
                _currentUser = null;
                
                // Clear chat
                ChatPanel.Children.Clear();
                
                // Reset input fields
                NameTextBox.Text = "";
                AgeComboBox.SelectedIndex = -1;
                
                // Show language selection screen
                ShowLanguageSelection();
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void ChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            var message = ChatInput.Text.Trim();
            if (string.IsNullOrEmpty(message) || _currentUser == null) return;

            // Add user message
            AddMessageToChat(message, true);
            ChatInput.Text = "";

            // Direct users to click topics instead of typing
            var response = _languageService.GetText("typing_guidance_message");
            AddMessageToChat(response, false);
        }

        private void DisplayTopicsInPopup()
        {
            TopicsContent.Children.Clear();
            
            foreach (var topic in _currentTopics)
            {
                var topicCard = new Border
                {
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(248, 249, 250)),
                    BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 236, 239)),
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 0, 0, 10),
                    Cursor = Cursors.Hand
                };

                var stackPanel = new StackPanel();
                
                var titleText = new TextBlock
                {
                    Text = $"📚 {_languageService.GetText(topic.Title)}",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 5)
                };
                
                var descText = new TextBlock
                {
                    Text = _languageService.GetText(topic.Description),
                    FontSize = 12,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125)),
                    TextWrapping = TextWrapping.Wrap
                };
                
                stackPanel.Children.Add(titleText);
                stackPanel.Children.Add(descText);
                topicCard.Child = stackPanel;
                
                topicCard.MouseLeftButtonDown += (s, e) => SelectTopic(topic);
                
                TopicsContent.Children.Add(topicCard);
            }
        }

        private void SelectTopic(Topic topic)
        {
            // Log topic selection activity
            if (_currentUser != null)
            {
                _loggingService.LogActivity(
                    _currentUser,
                    "Topic",
                    topic.Id.ToString(),
                    _languageService.GetText(topic.Title),
                    _languageService.GetText(topic.Description)
                );
            }
            
            // Show questions for this topic in the popup
            ShowQuestionsForTopic(topic);
        }

        private void ShowQuestionsForTopic(Topic topic)
        {
            // Update tab colors to show we're in questions mode
            TopicsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125));
            QuestionsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96));
            BlogsTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125));
            
            TopicsContent.Children.Clear();
            
            // Add topic header
            var topicHeader = new Border
            {
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(232, 245, 232)),
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 0, 0, 15)
            };

            var headerText = new TextBlock
            {
                Text = $"📚 Questions about {_languageService.GetText(topic.Title)}",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            
            topicHeader.Child = headerText;
            TopicsContent.Children.Add(topicHeader);
            
            // Add back button
            var backButton = new Button
            {
                Content = "← Back to All Topics",
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                Padding = new Thickness(15, 8, 15, 8),
                Margin = new Thickness(0, 0, 0, 15),
                HorizontalAlignment = HorizontalAlignment.Center,
                Cursor = Cursors.Hand
            };
            
            backButton.Click += (s, e) => ShowTopicsTab();
            TopicsContent.Children.Add(backButton);
            
            // Show questions for this topic
            var questions = _contentService.GetQuestionsForTopic(topic.Id, _currentUser.Age);
            
            foreach (var question in questions)
            {
                var questionCard = new Border
                {
                    Background = System.Windows.Media.Brushes.White,
                    BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 236, 239)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(12),
                    Padding = new Thickness(20),
                    Margin = new Thickness(0, 0, 0, 15),
                    Cursor = Cursors.Hand
                };

                var questionText = new TextBlock
                {
                    Text = $"❓ {_languageService.GetText(question.QuestionText)}",
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 22
                };
                
                questionCard.Child = questionText;
                questionCard.MouseLeftButtonDown += (s, e) => ShowAnswerInChat(question);
                
                TopicsContent.Children.Add(questionCard);
            }
        }

        private void ShowAnswerInChat(Question question)
        {
            // Log question selection activity
            if (_currentUser != null)
            {
                _loggingService.LogActivity(
                    _currentUser,
                    "Question",
                    question.Id.ToString(),
                    _languageService.GetText(question.QuestionText),
                    _languageService.GetText(question.Answer)
                );
            }
            
            TopicsPopup.Visibility = Visibility.Collapsed;
            AddMessageToChat($"Question: {_languageService.GetText(question.QuestionText)}", false);
            AddMessageToChat($"Answer: {_languageService.GetText(question.Answer)}", false);
            
            // Add related blogs
            ShowRelatedBlogs(question);
        }

        private void ShowRelatedBlogs(Question question)
        {
            var relatedBlogs = GetRelatedBlogs(question.QuestionText);
            
            if (relatedBlogs.Any())
            {
                var blogMessage = new Border
                {
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(248, 249, 250)),
                    BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 236, 239)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(15),
                    Padding = new Thickness(20),
                    Margin = new Thickness(0, 0, 0, 15),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    MaxWidth = 400
                };

                var blogStack = new StackPanel();
                
                var headerText = new TextBlock
                {
                    Text = _languageService.GetText("related_articles"),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 15)
                };
                blogStack.Children.Add(headerText);
                
                foreach (var blog in relatedBlogs.Take(3))
                {
                    var blogCard = new Border
                    {
                        Background = System.Windows.Media.Brushes.White,
                        BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 236, 239)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(8),
                        Padding = new Thickness(15),
                        Margin = new Thickness(0, 0, 0, 10),
                        Cursor = Cursors.Hand
                    };

                    var cardStack = new StackPanel();
                    
                    var iconTitleStack = new StackPanel { Orientation = Orientation.Horizontal };
                    
                    var iconText = new TextBlock
                    {
                        Text = blog.Item4,
                        FontSize = 20,
                        Margin = new Thickness(0, 0, 10, 0),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    
                    var titleStack = new StackPanel();
                    
                    var categoryText = new TextBlock
                    {
                        Text = _languageService.GetText(blog.Item3),
                        FontSize = 10,
                        FontWeight = FontWeights.Bold,
                        Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                        Margin = new Thickness(0, 0, 0, 3)
                    };
                    
                    var titleText = new TextBlock
                    {
                        Text = _languageService.GetText(blog.Item1),
                        FontSize = 14,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(44, 62, 80)),
                        TextWrapping = TextWrapping.Wrap
                    };
                    
                    titleStack.Children.Add(categoryText);
                    titleStack.Children.Add(titleText);
                    
                    iconTitleStack.Children.Add(iconText);
                    iconTitleStack.Children.Add(titleStack);
                    
                    var descText = new TextBlock
                    {
                        Text = _languageService.GetText(blog.Item2),
                        FontSize = 12,
                        Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125)),
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 8, 0, 0),
                        LineHeight = 16
                    };
                    
                    cardStack.Children.Add(iconTitleStack);
                    cardStack.Children.Add(descText);
                    blogCard.Child = cardStack;
                    
                    blogCard.MouseLeftButtonDown += (s, e) => OpenBlogPage(blog.Item1, blog.Item2, blog.Item3);
                    
                    blogStack.Children.Add(blogCard);
                }
                
                var readMoreText = new TextBlock
                {
                    Text = "💡 Click any article to read more!",
                    FontSize = 12,
                    FontStyle = FontStyles.Italic,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 0)
                };
                blogStack.Children.Add(readMoreText);
                
                blogMessage.Child = blogStack;
                ChatPanel.Children.Add(blogMessage);
                
                // Scroll to bottom
                ChatScrollViewer.ScrollToEnd();
            }
        }

        private List<(string Title, string Description, string Category, string Icon)> GetRelatedBlogs(string questionText)
        {
            var allBlogs = new List<(string, string, string, string)>
            {
                ("blog_body_awareness", "blog_body_awareness_desc", "category_body_parts", "🧸"),
                ("blog_safety_rules", "blog_safety_rules_desc", "category_personal_safety", "🛡️"),
                ("blog_growing_changes", "blog_growing_changes_desc", "category_growing_up", "🌱"),
                ("blog_healthy_friendships", "blog_healthy_friendships_desc", "category_healthy_relationships", "👫"),
                ("blog_talking_adults", "blog_talking_adults_desc", "category_personal_safety", "👨‍👩‍👧‍👦"),
                ("blog_body_boundaries", "blog_body_boundaries_desc", "category_personal_safety", "🚫")
            };
            
            // Simple keyword matching to find related blogs
            var questionLower = questionText.ToLower();
            var relatedBlogs = new List<(string, string, string, string)>();
            
            foreach (var blog in allBlogs)
            {
                var isRelated = false;
                
                // Check for body-related keywords
                if (questionLower.Contains("body") || questionLower.Contains("part") || questionLower.Contains("clean"))
                {
                    if (blog.Item3 == "category_body_parts")
                        isRelated = true;
                }
                
                // Check for safety-related keywords
                if (questionLower.Contains("safe") || questionLower.Contains("boundary") || questionLower.Contains("touch") || questionLower.Contains("private"))
                {
                    if (blog.Item3 == "category_personal_safety")
                        isRelated = true;
                }
                
                // Check for growing up keywords
                if (questionLower.Contains("grow") || questionLower.Contains("change") || questionLower.Contains("older"))
                {
                    if (blog.Item3 == "category_growing_up")
                        isRelated = true;
                }
                
                // Check for relationship keywords
                if (questionLower.Contains("friend") || questionLower.Contains("relationship") || questionLower.Contains("talk"))
                {
                    if (blog.Item3 == "category_healthy_relationships")
                        isRelated = true;
                }
                
                if (isRelated)
                {
                    relatedBlogs.Add(blog);
                }
            }
            
            // If no specific matches, return general safety and body blogs
            if (!relatedBlogs.Any())
            {
                relatedBlogs.Add(allBlogs[0]); // Body Parts
                relatedBlogs.Add(allBlogs[1]); // Personal Safety
            }
            
            return relatedBlogs;
        }

        private void ShowBlogs()
        {
            BlogsPanel.Visibility = Visibility.Visible;
            MainPanel.Visibility = Visibility.Collapsed;
            
            BlogsList.Children.Clear();
            
            var blogs = new[]
            {
                new { Title = "Understanding Your Body", Description = "Learn about body parts and keeping clean", Category = "Body Parts" },
                new { Title = "Staying Safe", Description = "Important safety tips for children", Category = "Personal Safety" },
                new { Title = "Growing Up Changes", Description = "What to expect as you grow older", Category = "Growing Up" },
                new { Title = "Making Good Friends", Description = "How to build healthy friendships", Category = "Healthy Relationships" },
                new { Title = "Talking to Adults", Description = "When and how to ask for help", Category = "Personal Safety" },
                new { Title = "Body Boundaries", Description = "Understanding personal space and privacy", Category = "Personal Safety" }
            };
            
            foreach (var blog in blogs)
            {
                var blogCard = new Border
                {
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(248, 249, 250)),
                    BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 236, 239)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 0, 0, 10),
                    Cursor = Cursors.Hand,
                    MaxWidth = 300
                };

                var stackPanel = new StackPanel();
                
                var categoryText = new TextBlock
                {
                    Text = blog.Category,
                    FontSize = 10,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 5)
                };
                
                var titleText = new TextBlock
                {
                    Text = blog.Title,
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(44, 62, 80)),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                
                var descText = new TextBlock
                {
                    Text = blog.Description,
                    FontSize = 12,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125)),
                    TextWrapping = TextWrapping.Wrap
                };
                
                stackPanel.Children.Add(categoryText);
                stackPanel.Children.Add(titleText);
                stackPanel.Children.Add(descText);
                blogCard.Child = stackPanel;
                
                blogCard.MouseLeftButtonDown += (s, e) => OpenBlogPage(blog.Title, blog.Description, blog.Category);
                
                BlogsList.Children.Add(blogCard);
            }
        }

        private void OpenBlogPage(string title, string description, string category)
        {
            // Log blog selection activity
            if (_currentUser != null)
            {
                _loggingService.LogActivity(
                    _currentUser,
                    "Blog",
                    title,
                    _languageService.GetText(title),
                    _languageService.GetText(description)
                );
            }
            
            TopicsPopup.Visibility = Visibility.Collapsed;
            ShowBlogPage(title, description, category);
        }

        private void ShowBlogPage(string title, string description, string category)
        {
            BlogsPanel.Visibility = Visibility.Visible;
            MainPanel.Visibility = Visibility.Collapsed;
            
            BlogsList.Children.Clear();
            
            // Create blog content
            var blogContent = new Border
            {
                Background = System.Windows.Media.Brushes.White,
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 236, 239)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(30),
                Margin = new Thickness(0, 0, 0, 20)
            };

            var contentStack = new StackPanel();
            
            // Category badge
            var categoryBadge = new Border
            {
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(15, 8, 15, 8),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 0, 0, 20)
            };
            
            var categoryText = new TextBlock
            {
                Text = _languageService.GetText(category),
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 12,
                FontWeight = FontWeights.Bold
            };
            
            categoryBadge.Child = categoryText;
            contentStack.Children.Add(categoryBadge);
            
            // Title
            var titleText = new TextBlock
            {
                Text = _languageService.GetText(title),
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 20)
            };
            contentStack.Children.Add(titleText);
            
            // Description
            var descText = new TextBlock
            {
                Text = _languageService.GetText(description),
                FontSize = 18,
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(44, 62, 80)),
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 28,
                Margin = new Thickness(0, 0, 0, 30)
            };
            contentStack.Children.Add(descText);
            
            // Content sections based on category
            var contentSections = GetBlogContent(category);
            foreach (var section in contentSections)
            {
                var sectionBorder = new Border
                {
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(248, 249, 250)),
                    BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 236, 239)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(20),
                    Margin = new Thickness(0, 0, 0, 15)
                };
                
                var sectionStack = new StackPanel();
                
                var sectionTitle = new TextBlock
                {
                    Text = _languageService.GetText(section.Title),
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 10)
                };
                
                var sectionContent = new TextBlock
                {
                    Text = _languageService.GetText(section.Content),
                    FontSize = 16,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(44, 62, 80)),
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 24
                };
                
                sectionStack.Children.Add(sectionTitle);
                sectionStack.Children.Add(sectionContent);
                sectionBorder.Child = sectionStack;
                
                contentStack.Children.Add(sectionBorder);
            }
            
            // Safety reminder
            var reminderBorder = new Border
            {
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 243, 205)),
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 193, 7)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 20, 0, 0)
            };
            
            var reminderText = new TextBlock
            {
                Text = "💡 Remember: Always talk to a trusted adult (like your parents, teachers, or doctors) if you have any questions or concerns!",
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(133, 100, 4)),
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 24
            };
            
            reminderBorder.Child = reminderText;
            contentStack.Children.Add(reminderBorder);
            
            blogContent.Child = contentStack;
            BlogsList.Children.Add(blogContent);
        }

        private List<(string Title, string Content)> GetBlogContent(string category)
        {
            return category switch
            {
                "category_body_parts" => new List<(string, string)>
                {
                    ("blog_body_parts_what", "blog_body_parts_what_content"),
                    ("blog_body_parts_clean", "blog_body_parts_clean_content"),
                    ("blog_body_parts_privacy", "blog_body_parts_privacy_content")
                },
                "category_personal_safety" => new List<(string, string)>
                {
                    ("blog_safety_what", "blog_safety_what_content"),
                    ("blog_safety_touches", "blog_safety_touches_content"),
                    ("blog_safety_saying_no", "blog_safety_saying_no_content")
                },
                "category_growing_up" => new List<(string, string)>
                {
                    ("blog_growing_gradually", "blog_growing_gradually_content"),
                    ("blog_growing_physical", "blog_growing_physical_content"),
                    ("blog_growing_emotional", "blog_growing_emotional_content")
                },
                "category_healthy_relationships" => new List<(string, string)>
                {
                    ("blog_relationships_friends", "blog_relationships_friends_content"),
                    ("blog_relationships_respect", "blog_relationships_respect_content"),
                    ("blog_relationships_communication", "blog_relationships_communication_content")
                },
                _ => new List<(string, string)>
                {
                    ("blog_default_learning", "blog_default_learning_content"),
                    ("blog_default_adults", "blog_default_adults_content"),
                    ("blog_default_feelings", "blog_default_feelings_content")
                }
            };
        }

        private Border AddMessageToChat(string message, bool isUser)
        {
            var messageBorder = new Border
            {
                Background = isUser ? 
                    new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)) :
                    new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(232, 245, 232)),
                BorderBrush = isUser ? 
                    new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)) :
                    new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(195, 230, 195)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 0, 0, 15),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 400
            };

            var messageText = new TextBlock
            {
                Text = message,
                Foreground = isUser ? 
                    System.Windows.Media.Brushes.White :
                    new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(44, 62, 80)),
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14
            };

            messageBorder.Child = messageText;
            ChatPanel.Children.Add(messageBorder);
            
            // Scroll to bottom
            ChatScrollViewer.ScrollToEnd();
            
            return messageBorder;
        }

        private void ShowMainPanel()
        {
            Console.WriteLine("🔄 ShowMainPanel() called");
            Console.WriteLine($"WelcomePanel visibility before: {WelcomePanel.Visibility}");
            Console.WriteLine($"MainPanel visibility before: {MainPanel.Visibility}");
            
            WelcomePanel.Visibility = Visibility.Collapsed;
            MainPanel.Visibility = Visibility.Visible;
            
            Console.WriteLine($"WelcomePanel visibility after: {WelcomePanel.Visibility}");
            Console.WriteLine($"MainPanel visibility after: {MainPanel.Visibility}");
            
            // Update button text with current language
            SendDailyLogButton.Content = _languageService.GetText("send_daily_log");
            Console.WriteLine("✅ ShowMainPanel() completed");
        }

        private void ShowLanguageSelection()
        {
            LanguagePanel.Visibility = Visibility.Visible;
            WelcomePanel.Visibility = Visibility.Collapsed;
            MainPanel.Visibility = Visibility.Collapsed;
            TopicsPopup.Visibility = Visibility.Collapsed;
            BlogsPanel.Visibility = Visibility.Collapsed;
        }

        private void EnglishButton_Click(object sender, RoutedEventArgs e)
        {
            _languageService.SetLanguage(Services.Language.English);
            UpdateUIWithCurrentLanguage();
            
            // Force refresh topics if they exist
            if (_currentUser != null && _currentTopics != null && _currentTopics.Count > 0)
            {
                _currentTopics = _contentService.GetTopicsForAge(_currentUser.Age);
                if (TopicsPopup.Visibility == Visibility.Visible)
                {
                    DisplayTopicsInPopup();
                }
            }
        }

        private void BurmeseButton_Click(object sender, RoutedEventArgs e)
        {
            _languageService.SetLanguage(Services.Language.Burmese);
            UpdateUIWithCurrentLanguage();
            
            // Force refresh topics if they exist
            if (_currentUser != null && _currentTopics != null && _currentTopics.Count > 0)
            {
                _currentTopics = _contentService.GetTopicsForAge(_currentUser.Age);
                if (TopicsPopup.Visibility == Visibility.Visible)
                {
                    DisplayTopicsInPopup();
                }
            }
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("🔄 ContinueButton_Click - Showing welcome panel");
            LanguagePanel.Visibility = Visibility.Collapsed;
            WelcomePanel.Visibility = Visibility.Visible;
            UpdateUIWithCurrentLanguage();
            
            // Ensure form is cleared and button is visible
            ClearFormFields();
            EnsureButtonVisible();
        }

        private void UpdateUIWithCurrentLanguage()
        {
            // Update language selection screen
            LanguageTitle.Text = _languageService.GetText("select_language");
            EnglishButton.Content = _languageService.GetText("language_english");
            BurmeseButton.Content = _languageService.GetText("language_burmese");
            ContinueButton.Content = _languageService.GetText("continue_button");

            // Update welcome screen
            WelcomeTitle.Text = _languageService.GetText("welcome_title");
            WelcomeSubtitle.Text = _languageService.GetText("welcome_subtitle");
            NameLabel.Text = _languageService.GetText("enter_name");
            AgeLabel.Text = _languageService.GetText("select_age");
            ParentNameLabel.Text = _languageService.GetText("parent_name");
            ParentEmailLabel.Text = _languageService.GetText("parent_email");
            EmailNotificationsCheckBox.Content = _languageService.GetText("email_notifications");
            StartButton.Content = _languageService.GetText("start_button");

            // Update main screen - MainTitle doesn't exist in new UI
            TopicsButton.Content = _languageService.GetText("topics_button");
            ChatInput.Text = _languageService.GetText("chat_input_placeholder");
            SendButton.Content = _languageService.GetText("send_button");

            // Update topics popup
            TopicsTitle.Text = _languageService.GetText("topics_title");
            TopicsTab.Content = _languageService.GetText("topics_tab");
            QuestionsTab.Content = _languageService.GetText("questions_tab");
            BlogsTab.Content = _languageService.GetText("blogs_tab");
            CloseTopicsButton.Content = _languageService.GetText("close_button");

            // Update age combo box items
            UpdateAgeComboBox();

            // Refresh topics content if topics popup is visible
            if (TopicsPopup.Visibility == Visibility.Visible)
            {
                // Reload topics for current user's age to get fresh data
                if (_currentUser != null)
                {
                    _currentTopics = _contentService.GetTopicsForAge(_currentUser.Age);
                }
                DisplayTopicsInPopup();
            }
        }

        private void UpdateAgeComboBox()
        {
            AgeComboBox.Items.Clear();
            for (int age = 8; age <= 15; age++)
            {
                var item = new ComboBoxItem
                {
                    Content = _languageService.GetText($"age_{age}"),
                    Tag = age
                };
                AgeComboBox.Items.Add(item);
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            // This method is no longer needed as we removed the test button
            // But keeping it to avoid compilation errors
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsGmailEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
                
            var domain = email.Split('@').LastOrDefault()?.ToLower();
            return domain == "gmail.com";
        }
    }
}
