using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChildSafeSexEducation.Desktop.Models;
using ChildSafeSexEducation.Desktop.Services;
using ChildSafeSexEducation.Desktop.Managers;
using Microsoft.Extensions.Configuration;

namespace ChildSafeSexEducation.Desktop
{
    public partial class MainWindow : Window
    {
        public User? _currentUser;
        public List<Topic> _currentTopics = new();
        public List<Question> _currentQuestions = new();
        private int _currentTopicId = 0;
        
        private readonly ContentService _contentService;
        private readonly NLPService _nlpService;
        private readonly LanguageService _languageService;
        private readonly LoggingService _loggingService;
        private readonly UserStorageService _userStorageService;

        // Managers
        private readonly UIManager _uiManager;
        private readonly ContentManager _contentManager;
        private readonly ChatManager _chatManager;


        public MainWindow()
        {
            InitializeComponent();
            _contentService = new ContentService();
            _nlpService = new NLPService(_contentService);
            _languageService = LanguageService.Instance;
            _loggingService = new LoggingService();
            _userStorageService = new UserStorageService();
            
            // Initialize managers
            _uiManager = new UIManager(this, _languageService);
            _contentManager = new ContentManager(this, _contentService, _languageService, _loggingService, _uiManager);
            _chatManager = new ChatManager(this, _contentService, _nlpService, _languageService, _loggingService, _uiManager);
            
            // Show language selection first
            ShowLanguageSelection();
            
            // Set up daily email sending timer
            SetupDailyEmailTimer();
            
            // Test encryption first
            TestEncryption.TestPasswordEncryption();
            
            // Migrate existing passwords to encrypted format
            _userStorageService.MigratePasswordsToEncrypted();
            
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
            UsernameTextBox.Text = "";
            PasswordBox.Password = "";
            AgeComboBox.SelectedIndex = -1;
            ParentNameTextBox.Text = "";
            ParentEmailTextBox.Text = "";
            EmailNotificationsCheckBox.IsChecked = false;
            Console.WriteLine("✅ Form fields cleared");
        }

        private void UpdateFormLabels()
        {
            _uiManager.UpdateFormLabels();
        }

        private void UpdateLoginLabels()
        {
            _uiManager.UpdateLoginLabels();
        }

        private void UpdateAllLabels()
        {
            Console.WriteLine($"🔤 UpdateAllLabels called - Current language: {_languageService.CurrentLanguage}");
            
            // Update form labels
            UpdateFormLabels();
            UpdateLoginLabels();
            
            // Use direct language update methods instead of translation service
            if (_languageService.CurrentLanguage == Services.Language.English)
            {
                _uiManager.UpdateAllUIElementsToEnglish();
                Console.WriteLine("✅ UpdateAllLabels - Updated to English");
            }
            else
            {
                _uiManager.UpdateAllUIElementsToBurmese();
                Console.WriteLine("✅ UpdateAllLabels - Updated to Burmese");
            }
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("🔐 LoginButton_Click called!");
            
            var username = LoginUsernameTextBox.Text.Trim();
            var password = LoginPasswordBox.Password.Trim();
            
            Console.WriteLine($"Username: '{username}'");
            Console.WriteLine($"Password: '{"*".PadLeft(password.Length, '*')}'");
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("❌ Login validation failed - missing username or password");
                ModernMessageBox.ShowWarning("Please enter both username and password.", "Missing Information");
                return;
            }

            // Check for admin login first
            if (username == "admin" && password == "admin")
            {
                Console.WriteLine("✅ Admin login successful");
                // Open admin dashboard directly
                var adminDashboard = new AdminDashboard();
                adminDashboard.Show();
                this.Close();
                return;
            }

            // Validate regular user credentials
            if (_userStorageService.ValidateUser(username, password))
            {
                Console.WriteLine("✅ User login successful");
                
                // Get user data
                _currentUser = _userStorageService.GetUserByUsername(username);
                if (_currentUser != null)
                {
                    Console.WriteLine($"✅ User loaded: {_currentUser.Name}");
                    Console.WriteLine($"🔤 User's preferred language: '{_currentUser.PreferredLanguage}'");
                    
                    // Keep the current language choice from language selection page
                    // Don't override with user preference - respect user's current choice
                    Console.WriteLine($"🔤 Keeping current language choice: {_languageService.CurrentLanguage}");
                    
                    // Update all UI labels with current language
                    if (_languageService.CurrentLanguage == Services.Language.English)
                    {
                        _uiManager.UpdateAllUIElementsToEnglish();
                        Console.WriteLine("✅ Updated UI to English based on current choice");
                    }
                    else
                    {
                        _uiManager.UpdateAllUIElementsToBurmese();
                        Console.WriteLine("✅ Updated UI to Burmese based on current choice");
                    }
                    
                    // Show main interface
                    WelcomeText.Text = $"Hi {_currentUser.Name}! 👋";
                    if (_languageService.CurrentLanguage == Services.Language.English)
                    {
                        AddMessageToChat("Hello! I'm here to help you learn about safe and healthy topics. You can ask me questions or click the Topics button to see what we can learn about together!", false);
                    }
                    else
                    {
                        AddMessageToChat("မင်္ဂလာပါ! ကျွန်တော်က လုံခြုံပြီး ကျန်းမာသော အကြောင်းအရာများအကြောင်း သင်ယူရန် ကူညီပေးရန် ဤနေရာတွင် ရှိပါတယ်။ သင်မေးခွန်းများ မေးနိုင်ပြီး သို့မဟုတ် ခေါင်းစဉ်များ ခလုတ်ကို နှိပ်ပြီး ကျွန်တော်တို့ အတူတကွ သင်ယူနိုင်သော အရာများကို ကြည့်နိုင်ပါတယ်!", false);
                    }
                    ShowMainPanel();
                }
            }
            else
            {
                Console.WriteLine("❌ Login failed - invalid credentials");
                ModernMessageBox.ShowWarning("Invalid username or password. Please try again.", "Login Failed");
                LoginPasswordBox.Password = ""; // Clear password field
            }
        }


        private void SwitchToRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("🔄 Switching to register form");
            LoginPanel.Visibility = Visibility.Collapsed;
            WelcomePanel.Visibility = Visibility.Visible;
            UpdateFormLabels();
            ClearFormFields();
            EnsureButtonVisible();
        }

        private void SwitchToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("🔄 Switching to login form");
            WelcomePanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            UpdateLoginLabels();
            LoginUsernameTextBox.Text = "";
            LoginPasswordBox.Password = "";
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("🚀 StartButton_Click called!");
            
            var name = NameTextBox.Text.Trim();
            var username = UsernameTextBox.Text.Trim();
            var password = PasswordBox.Password.Trim();
            var selectedItem = AgeComboBox.SelectedItem as ComboBoxItem;
            var parentEmail = ParentEmailTextBox.Text.Trim();
            
            Console.WriteLine($"Name: '{name}'");
            Console.WriteLine($"Username: '{username}'");
            Console.WriteLine($"Password: '{"*".PadLeft(password.Length, '*')}'");
            Console.WriteLine($"SelectedItem: {selectedItem?.Tag}");
            Console.WriteLine($"Parent Email: '{parentEmail}'");
            
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || selectedItem == null || selectedItem.Tag == null)
            {
                Console.WriteLine("❌ Validation failed - missing required fields");
                ModernMessageBox.ShowWarning(_languageService.GetText("missing_required_fields"), _languageService.GetText("missing_information"));
                return;
            }

            // Validate username (minimum 3 characters, alphanumeric only)
            if (username.Length < 3)
            {
                Console.WriteLine("❌ Username too short");
                ModernMessageBox.ShowWarning("Username must be at least 3 characters long.", "Invalid Username");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                Console.WriteLine("❌ Username contains invalid characters");
                ModernMessageBox.ShowWarning("Username can only contain letters, numbers, and underscores.", "Invalid Username");
                return;
            }

            // Validate password (minimum 6 characters)
            if (password.Length < 6)
            {
                Console.WriteLine("❌ Password too short");
                ModernMessageBox.ShowWarning("Password must be at least 6 characters long.", "Invalid Password");
                return;
            }

            // Check if username already exists
            if (_userStorageService.UsernameExists(username))
            {
                Console.WriteLine("❌ Username already exists");
                ModernMessageBox.ShowWarning($"Username '{username}' is already taken. Please choose a different username.", "Username Already Exists");
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
                Username = username,
                Password = password, // Store password (in real app, hash this)
                Age = age,
                ParentName = ParentNameTextBox.Text.Trim(),
                ParentEmail = ParentEmailTextBox.Text.Trim(),
                EmailNotificationsEnabled = EmailNotificationsCheckBox.IsChecked ?? false,
                PreferredLanguage = _languageService.CurrentLanguage.ToString()
            };
            
            Console.WriteLine($"✅ User created successfully with preferred language: '{_currentUser.PreferredLanguage}'");
            Console.WriteLine($"🔤 Current language service language: {_languageService.CurrentLanguage}");
            
            // Save user data to file
            _userStorageService.SaveUser(_currentUser);
            Console.WriteLine("✅ User saved to file");
            
            WelcomeText.Text = $"Hi {name}! 👋";
            
            // Add welcome message to chat
            if (_languageService.CurrentLanguage == Services.Language.English)
            {
                AddMessageToChat("Hello! I'm here to help you learn about safe and healthy topics. You can ask me questions or click the Topics button to see what we can learn about together!", false);
            }
            else
            {
                AddMessageToChat("မင်္ဂလာပါ! ကျွန်တော်က လုံခြုံပြီး ကျန်းမာသော အကြောင်းအရာများအကြောင်း သင်ယူရန် ကူညီပေးရန် ဤနေရာတွင် ရှိပါတယ်။ သင်မေးခွန်းများ မေးနိုင်ပြီး သို့မဟုတ် ခေါင်းစဉ်များ ခလုတ်ကို နှိပ်ပြီး ကျွန်တော်တို့ အတူတကွ သင်ယူနိုင်သော အရာများကို ကြည့်နိုင်ပါတယ်!", false);
            }
            Console.WriteLine("✅ Welcome message added to chat");
            
            Console.WriteLine("🔄 Calling ShowMainPanel()...");
            ShowMainPanel();
            Console.WriteLine("✅ ShowMainPanel() completed");
        }

        private void TopicsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null) return;
            
            _currentTopics = _contentService.GetTopicsForAge(_currentUser.Age);
            _contentManager.ShowTopicsTab();
            TopicsPopup.Visibility = Visibility.Visible;
            
            // Ensure topics are displayed with current language
            _contentManager.DisplayTopicsInPopup();
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
            _contentManager.ShowTopicsTab();
        }

        private void QuestionsTab_Click(object sender, RoutedEventArgs e)
        {
            _contentManager.ShowQuestionsTab();
        }

        private void BlogsTab_Click(object sender, RoutedEventArgs e)
        {
            _contentManager.ShowBlogsTab();
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
            _chatManager.ProcessUserMessage(ChatInput.Text.Trim());
        }

        private void DisplayTopicsInPopup()
        {
            TopicsContent.Children.Clear();
            
            foreach (var topic in _currentTopics)
            {
                var translatedTitle = _languageService.GetText(topic.Title);
                var translatedDesc = _languageService.GetText(topic.Description);
                
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
                    Text = $"📚 {translatedTitle}",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 5),
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                var descText = new TextBlock
                {
                    Text = translatedDesc,
                    FontSize = 12,
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(108, 117, 125)),
                    TextWrapping = TextWrapping.Wrap,
                    FontFamily = _uiManager.GetAppFontFamily()
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
            
            backButton.Click += (s, e) => _contentManager.ShowTopicsTab();
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

        public Border AddMessageToChat(string message, bool isUser)
        {
            return _chatManager.AddMessageToChat(message, isUser);
        }

        private void ShowMainPanel()
        {
            if (WelcomePanel == null || MainPanel == null)
            {
                return;
            }
            
            Console.WriteLine($"🔍 ShowMainPanel - Current language: {_languageService.CurrentLanguage}");
            
            // Hide all other panels to prevent overlapping
            LanguagePanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Collapsed;
            WelcomePanel.Visibility = Visibility.Collapsed;
            TopicsPopup.Visibility = Visibility.Collapsed;
            BlogsPanel.Visibility = Visibility.Collapsed;
            
            // Show only the main panel
            MainPanel.Visibility = Visibility.Visible;
            
            // Update ALL UI elements to the current language using direct methods
            if (_languageService.CurrentLanguage == Services.Language.English)
            {
                _uiManager.UpdateAllUIElementsToEnglish();
                Console.WriteLine("✅ Main panel updated to English");
            }
            else
            {
                _uiManager.UpdateAllUIElementsToBurmese();
                Console.WriteLine("✅ Main panel updated to Burmese");
            }
            
            // Add welcome message to chat in the current language
            if (ChatPanel != null)
            {
                ChatPanel.Children.Clear();
                _chatManager.AddWelcomeMessage();
            }
        }

        private void ShowLanguageSelection()
        {
            LanguagePanel.Visibility = Visibility.Visible;
            LoginPanel.Visibility = Visibility.Collapsed;
            WelcomePanel.Visibility = Visibility.Collapsed;
            MainPanel.Visibility = Visibility.Collapsed;
            TopicsPopup.Visibility = Visibility.Collapsed;
            BlogsPanel.Visibility = Visibility.Collapsed;
            
            // Reset button styles to default state for clear visibility
            ResetLanguageButtonStyles();
        }
        
        private void ResetLanguageButtonStyles()
        {
            _uiManager.ResetLanguageButtonStyles();
        }

        private void ShowLoginOrRegister()
        {
            // Check if any users exist
            var existingUsers = _userStorageService.GetAllUsers();
            
            if (existingUsers.Any())
            {
                // Show login form first
                Console.WriteLine("📋 Users exist - showing login form");
                LanguagePanel.Visibility = Visibility.Collapsed;
                LoginPanel.Visibility = Visibility.Visible;
                WelcomePanel.Visibility = Visibility.Collapsed;
                MainPanel.Visibility = Visibility.Collapsed;
                TopicsPopup.Visibility = Visibility.Collapsed;
                BlogsPanel.Visibility = Visibility.Collapsed;
                UpdateLoginLabels();
            }
            else
            {
                // Show registration form first
                Console.WriteLine("📋 No users exist - showing registration form");
                LanguagePanel.Visibility = Visibility.Collapsed;
                LoginPanel.Visibility = Visibility.Collapsed;
                WelcomePanel.Visibility = Visibility.Visible;
                MainPanel.Visibility = Visibility.Collapsed;
                TopicsPopup.Visibility = Visibility.Collapsed;
                BlogsPanel.Visibility = Visibility.Collapsed;
                UpdateFormLabels();
                ClearFormFields();
                EnsureButtonVisible();
            }
        }

        private void EnglishButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("🔤 ENGLISH BUTTON CLICKED - CAPTURING USER CHOICE");
            
            // Enhanced visual feedback - highlight the clicked button
            if (EnglishButton != null)
            {
                // Make English button very prominent when selected
                EnglishButton.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LimeGreen);
                EnglishButton.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                EnglishButton.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkGreen);
                EnglishButton.BorderThickness = new Thickness(3);
                EnglishButton.FontWeight = FontWeights.Bold;
                EnglishButton.FontSize = 16;
                EnglishButton.Content = "✅ ENGLISH SELECTED";
            }
            if (BurmeseButton != null)
            {
                // Make Burmese button less prominent when not selected
                BurmeseButton.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                BurmeseButton.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                BurmeseButton.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                BurmeseButton.BorderThickness = new Thickness(1);
                BurmeseButton.FontWeight = FontWeights.Normal;
                BurmeseButton.FontSize = 14;
                BurmeseButton.Content = "🇲🇲 ဗမာ";
            }
            
            // Capture user's language choice
            var chosenLanguage = _languageService.CaptureUserLanguageChoice(true, false);
            
            // Apply the user's choice
            _languageService.ApplyUserLanguageChoice(chosenLanguage);
            
            // Update all UI elements based on chosen language
            if (chosenLanguage == Services.Language.English)
            {
                _uiManager.UpdateAllUIElementsToEnglish();
            }
            else
            {
                _uiManager.UpdateAllUIElementsToBurmese();
            }
            
            // Force UI refresh
            this.InvalidateVisual();
            this.UpdateLayout();
            
            // Clear chat and add welcome message in chosen language
            if (ChatPanel != null)
            {
                ChatPanel.Children.Clear();
                if (chosenLanguage == Services.Language.English)
                {
                    AddMessageToChat("Hello! I'm here to help you learn about safe and healthy topics. You can ask me questions or click the Topics button to see what we can learn about together!", false);
                }
                else
                {
                    AddMessageToChat("မင်္ဂလာပါ! ကျွန်တော်က လုံခြုံပြီး ကျန်းမာသော အကြောင်းအရာများအကြောင်း သင်ယူရန် ကူညီပေးရန် ဤနေရာတွင် ရှိပါတယ်။ သင်မေးခွန်းများ မေးနိုင်ပြီး သို့မဟုတ် ခေါင်းစဉ်များ ခလုတ်ကို နှိပ်ပြီး ကျွန်တော်တို့ အတူတကွ သင်ယူနိုင်သော အရာများကို ကြည့်နိုင်ပါတယ်!", false);
                }
            }
            
            // Refresh content to show translations in chosen language
            if (_currentUser != null)
            {
                _currentTopics = _contentService.GetTopicsForAge(_currentUser.Age);
                Console.WriteLine($"✅ Refreshed {_currentTopics.Count} topics in {chosenLanguage}");
                
                // Refresh visible content if topics popup is open
                if (TopicsPopup.Visibility == Visibility.Visible)
                {
                    DisplayTopicsInPopup();
                    Console.WriteLine($"✅ Refreshed visible topics popup in {chosenLanguage}");
                }
            }
            
            Console.WriteLine($"✅ USER LANGUAGE CHOICE APPLIED: {chosenLanguage}");
            
            // Refresh topics and questions content
            if (_currentUser != null)
            {
                _currentTopics = _contentService.GetTopicsForAge(_currentUser.Age);
                
                // Refresh the currently visible tab
                if (TopicsTab != null && TopicsTab.Foreground.ToString().Contains("39, 174, 96")) // Topics tab is active
                {
                    _contentManager.ShowTopicsTab();
                }
                else if (QuestionsTab != null && QuestionsTab.Foreground.ToString().Contains("39, 174, 96")) // Questions tab is active
                {
                    _contentManager.ShowQuestionsTab();
                }
            }
        }
        
        private void UpdateAllUIElementsToEnglish()
        {
            // Language selection screen
            if (LanguageTitle != null)
                LanguageTitle.Text = "Choose Language";
            if (ContinueButton != null)
                ContinueButton.Content = "✨ Continue";
            
            // Main interface buttons
            if (TopicsButton != null)
                TopicsButton.Content = "📚 Topics";
            if (SendDailyLogButton != null)
                SendDailyLogButton.Content = "📧 Send Daily Log";
            if (TestEmailButton != null)
                TestEmailButton.Content = "🧪 Test";
            if (ExitButton != null)
                ExitButton.Content = "🚪 Exit";
            
            // Form labels
            if (NameLabel != null)
                NameLabel.Text = "Enter your name:";
            if (UsernameLabel != null)
                UsernameLabel.Text = "Enter username:";
            if (PasswordLabel != null)
                PasswordLabel.Text = "Enter password:";
            if (AgeLabel != null)
                AgeLabel.Text = "Select your age:";
            if (ParentNameLabel != null)
                ParentNameLabel.Text = "Parent/Guardian Name:";
            if (ParentEmailLabel != null)
                ParentEmailLabel.Text = "Parent/Guardian Email:";
            if (StartButton != null)
                StartButton.Content = "Start Learning";
            if (LoginButton != null)
                LoginButton.Content = "Login";
            if (LoginTitle != null)
                LoginTitle.Text = "Welcome Back!";
            if (LoginSubtitle != null)
                LoginSubtitle.Text = "Please sign in to continue your learning journey";
            if (WelcomeTitle != null)
                WelcomeTitle.Text = "⭐ Little Star";
            if (WelcomeSubtitle != null)
                WelcomeSubtitle.Text = "Safe and Healthy Learning Platform for Children";
            if (LoginUsernameLabel != null)
                LoginUsernameLabel.Text = "Username:";
            if (LoginPasswordLabel != null)
                LoginPasswordLabel.Text = "Password:";
            
            // Language buttons
            if (EnglishButton != null)
                EnglishButton.Content = "🇺🇸 English";
            if (BurmeseButton != null)
                BurmeseButton.Content = "🇲🇲 ဗမာ";
            
            // Chat input
            if (ChatInput != null)
                ChatInput.Text = "Type your message here...";
            if (SendButton != null)
                SendButton.Content = "Send";
            
            // Update age combobox
            UpdateAgeComboBox();
            
            // Topics popup
            if (TopicsTitle != null)
                TopicsTitle.Text = "Learning Topics";
            if (TopicsTab != null)
                TopicsTab.Content = "Topics";
            if (QuestionsTab != null)
                QuestionsTab.Content = "Questions";
            if (BlogsTab != null)
                BlogsTab.Content = "Blogs";
            if (CloseTopicsButton != null)
                CloseTopicsButton.Content = "Close";
        }

        private void BurmeseButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("🇲🇲 BURMESE BUTTON CLICKED - CAPTURING USER CHOICE");
            
            // Enhanced visual feedback - highlight the clicked button
            if (BurmeseButton != null)
            {
                // Make Burmese button very prominent when selected
                BurmeseButton.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LimeGreen);
                BurmeseButton.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                BurmeseButton.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkGreen);
                BurmeseButton.BorderThickness = new Thickness(3);
                BurmeseButton.FontWeight = FontWeights.Bold;
                BurmeseButton.FontSize = 16;
                BurmeseButton.Content = "✅ ဗမာ ရွေးချယ်ပြီး";
            }
            if (EnglishButton != null)
            {
                // Make English button less prominent when not selected
                EnglishButton.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                EnglishButton.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                EnglishButton.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                EnglishButton.BorderThickness = new Thickness(1);
                EnglishButton.FontWeight = FontWeights.Normal;
                EnglishButton.FontSize = 14;
                EnglishButton.Content = "🇺🇸 English";
            }
            
            // Capture user's language choice
            var chosenLanguage = _languageService.CaptureUserLanguageChoice(false, true);
            
            // Apply the user's choice
            _languageService.ApplyUserLanguageChoice(chosenLanguage);
            
            // Update all UI elements based on chosen language
            if (chosenLanguage == Services.Language.English)
            {
                _uiManager.UpdateAllUIElementsToEnglish();
            }
            else
            {
                _uiManager.UpdateAllUIElementsToBurmese();
            }
            
            // Force UI refresh
            this.InvalidateVisual();
            this.UpdateLayout();
            
            // Clear chat and add welcome message in chosen language
            if (ChatPanel != null)
            {
                ChatPanel.Children.Clear();
                if (chosenLanguage == Services.Language.English)
                {
                    AddMessageToChat("Hello! I'm here to help you learn about safe and healthy topics. You can ask me questions or click the Topics button to see what we can learn about together!", false);
                }
                else
                {
                    AddMessageToChat("မင်္ဂလာပါ! ကျွန်တော်က လုံခြုံပြီး ကျန်းမာသော အကြောင်းအရာများအကြောင်း သင်ယူရန် ကူညီပေးရန် ဤနေရာတွင် ရှိပါတယ်။ သင်မေးခွန်းများ မေးနိုင်ပြီး သို့မဟုတ် ခေါင်းစဉ်များ ခလုတ်ကို နှိပ်ပြီး ကျွန်တော်တို့ အတူတကွ သင်ယူနိုင်သော အရာများကို ကြည့်နိုင်ပါတယ်!", false);
                }
            }
            
            // Refresh content to show translations in chosen language
            if (_currentUser != null)
            {
                _currentTopics = _contentService.GetTopicsForAge(_currentUser.Age);
                Console.WriteLine($"✅ Refreshed {_currentTopics.Count} topics in {chosenLanguage}");
                
                // Refresh visible content if topics popup is open
                if (TopicsPopup.Visibility == Visibility.Visible)
                {
                    DisplayTopicsInPopup();
                    Console.WriteLine($"✅ Refreshed visible topics popup in {chosenLanguage}");
                }
            }
            
            Console.WriteLine($"✅ USER LANGUAGE CHOICE APPLIED: {chosenLanguage}");
            
            // Refresh topics and questions content
            if (_currentUser != null)
            {
                _currentTopics = _contentService.GetTopicsForAge(_currentUser.Age);
                
                // Refresh the currently visible tab
                if (TopicsTab != null && TopicsTab.Foreground.ToString().Contains("39, 174, 96")) // Topics tab is active
                {
                    _contentManager.ShowTopicsTab();
                }
                else if (QuestionsTab != null && QuestionsTab.Foreground.ToString().Contains("39, 174, 96")) // Questions tab is active
                {
                    _contentManager.ShowQuestionsTab();
                }
            }
        }
        
        private void UpdateAllUIElementsToBurmese()
        {
            // Language selection screen
            if (LanguageTitle != null)
                LanguageTitle.Text = "ဘာသာစကား ရွေးချယ်ပါ";
            if (ContinueButton != null)
                ContinueButton.Content = "ဆက်လက်လုပ်ဆောင်ရန်";
            
            // Main interface buttons
            if (TopicsButton != null)
                TopicsButton.Content = "📚 ခေါင်းစဉ်များ";
            if (SendDailyLogButton != null)
                SendDailyLogButton.Content = "📧 နေ့စဉ် လော့ဂ်ပို့ပေးရန်";
            if (TestEmailButton != null)
                TestEmailButton.Content = "🧪 စမ်းသပ်ရန်";
            if (ExitButton != null)
                ExitButton.Content = "🚪 ထွက်ရန်";
            
            // Form labels
            if (NameLabel != null)
                NameLabel.Text = "သင့်နာမည်ကို ရိုက်ထည့်ပါ:";
            if (UsernameLabel != null)
                UsernameLabel.Text = "အသုံးပြုသူအမည်ကို ရိုက်ထည့်ပါ:";
            if (PasswordLabel != null)
                PasswordLabel.Text = "စကားဝှက်ကို ရိုက်ထည့်ပါ:";
            if (AgeLabel != null)
                AgeLabel.Text = "သင့်အသက်ကို ရွေးချယ်ပါ:";
            if (ParentNameLabel != null)
                ParentNameLabel.Text = "မိဘ/အုပ်ထိန်းသူ၏ နာမည်:";
            if (ParentEmailLabel != null)
                ParentEmailLabel.Text = "မိဘ/အုပ်ထိန်းသူ၏ အီးမေးလ်:";
            if (StartButton != null)
                StartButton.Content = "သင်ယူမှု စတင်ရန်";
            if (LoginButton != null)
                LoginButton.Content = "လော့ဂ်အင်ဝင်ရန်";
            if (LoginTitle != null)
                LoginTitle.Text = "ပြန်လည်ကြိုဆိုပါတယ်!";
            if (LoginSubtitle != null)
                LoginSubtitle.Text = "သင့်သင်ယူမှု ခရီးကို ဆက်လက်လုပ်ဆောင်ရန် ကျေးဇူးပြု၍ ဝင်ရောက်ပါ";
            if (WelcomeTitle != null)
                WelcomeTitle.Text = "⭐ Little Star";
            if (WelcomeSubtitle != null)
                WelcomeSubtitle.Text = "ကလေးများအတွက် လုံခြုံပြီး ကျန်းမာသော သင်ယူမှု ပလက်ဖောင်း";
            if (LoginUsernameLabel != null)
                LoginUsernameLabel.Text = "အသုံးပြုသူအမည်:";
            if (LoginPasswordLabel != null)
                LoginPasswordLabel.Text = "စကားဝှက်:";
            
            // Language buttons
            if (EnglishButton != null)
                EnglishButton.Content = "🇺🇸 English";
            if (BurmeseButton != null)
                BurmeseButton.Content = "🇲🇲 ဗမာ";
            
            // Chat input
            if (ChatInput != null)
                ChatInput.Text = "သင့်စာကို ဤနေရာတွင် ရိုက်ထည့်ပါ...";
            if (SendButton != null)
                SendButton.Content = "ပို့ရန်";
            
            // Update age combobox
            UpdateAgeComboBox();
            
            // Topics popup
            if (TopicsTitle != null)
                TopicsTitle.Text = "သင်ယူမှု ခေါင်းစဉ်များ";
            if (TopicsTab != null)
                TopicsTab.Content = "ခေါင်းစဉ်များ";
            if (QuestionsTab != null)
                QuestionsTab.Content = "မေးခွန်းများ";
            if (BlogsTab != null)
                BlogsTab.Content = "ဘလော့များ";
            if (CloseTopicsButton != null)
                CloseTopicsButton.Content = "ပိတ်ရန်";
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("🔄 ContinueButton_Click - Checking for existing users");
            
            // Use the direct language update methods instead of translation service
            if (_languageService.CurrentLanguage == Services.Language.English)
            {
                _uiManager.UpdateAllUIElementsToEnglish();
                Console.WriteLine("✅ Updated to English using direct method");
            }
            else
            {
                _uiManager.UpdateAllUIElementsToBurmese();
                Console.WriteLine("✅ Updated to Burmese using direct method");
            }
            
            ShowLoginOrRegister();
        }

        private void UpdateUIWithCurrentLanguage()
        {
            // Use direct language update methods instead of translation service
            if (_languageService.CurrentLanguage == Services.Language.English)
            {
                _uiManager.UpdateAllUIElementsToEnglish();
                Console.WriteLine("✅ UpdateUIWithCurrentLanguage - Updated to English");
            }
            else
            {
                _uiManager.UpdateAllUIElementsToBurmese();
                Console.WriteLine("✅ UpdateUIWithCurrentLanguage - Updated to Burmese");
            }

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
            
            // Always refresh the main content area (topics and questions tabs)
            if (_currentUser != null)
            {
                _currentTopics = _contentService.GetTopicsForAge(_currentUser.Age);
                
                // Refresh the currently visible tab
                if (TopicsTab.Foreground.ToString().Contains("39, 174, 96")) // Topics tab is active
                {
                    _contentManager.ShowTopicsTab();
                }
                else if (QuestionsTab.Foreground.ToString().Contains("39, 174, 96")) // Questions tab is active
                {
                    _contentManager.ShowQuestionsTab();
                }
            }
        }

        private void UpdateAgeComboBox()
        {
            AgeComboBox.Items.Clear();
            for (int age = 8; age <= 15; age++)
            {
                string ageText;
                if (_languageService.CurrentLanguage == Services.Language.English)
                {
                    ageText = $"{age} years old";
                }
                else
                {
                    ageText = $"{age} နှစ်";
                }
                
                var item = new ComboBoxItem
                {
                    Content = ageText,
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
