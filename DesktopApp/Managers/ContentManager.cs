using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ChildSafeSexEducation.Desktop.Models;
using ChildSafeSexEducation.Desktop.Services;

namespace ChildSafeSexEducation.Desktop.Managers
{
    public class ContentManager
    {
        private readonly ContentService _contentService;
        private readonly LanguageService _languageService;
        private readonly LoggingService _loggingService;
        private readonly MainWindow _mainWindow;
        private readonly UIManager _uiManager;

        public ContentManager(MainWindow mainWindow, ContentService contentService, LanguageService languageService, LoggingService loggingService, UIManager uiManager)
        {
            _mainWindow = mainWindow;
            _contentService = contentService;
            _languageService = languageService;
            _loggingService = loggingService;
            _uiManager = uiManager;
        }

        public void ShowTopicsTab()
        {
            // Update tab colors
            _mainWindow.TopicsTab.Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96));
            _mainWindow.QuestionsTab.Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125));
            _mainWindow.BlogsTab.Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125));
            
            _mainWindow.TopicsContent.Children.Clear();
            
            foreach (var topic in _mainWindow._currentTopics)
            {
                var topicCard = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(233, 236, 239)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(12),
                    Padding = new Thickness(20),
                    Margin = new Thickness(0, 0, 0, 15),
                    Cursor = Cursors.Hand
                };

                var stackPanel = new StackPanel();
                
                var titleText = new TextBlock
                {
                    Text = $"📚 {_languageService.GetText(topic.Title)}",
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 8),
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                var descText = new TextBlock
                {
                    Text = _languageService.GetText(topic.Description),
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125)),
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 20,
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                stackPanel.Children.Add(titleText);
                stackPanel.Children.Add(descText);
                topicCard.Child = stackPanel;
                
                topicCard.MouseLeftButtonDown += (s, e) => SelectTopic(topic);
                
                _mainWindow.TopicsContent.Children.Add(topicCard);
            }
        }

        public void ShowQuestionsTab()
        {
            // Update tab colors
            _mainWindow.TopicsTab.Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125));
            _mainWindow.QuestionsTab.Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96));
            _mainWindow.BlogsTab.Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125));
            
            _mainWindow.TopicsContent.Children.Clear();
            
            var allQuestions = new List<Question>();
            foreach (var topic in _mainWindow._currentTopics)
            {
                var questions = _contentService.GetQuestionsForTopic(topic.Id, _mainWindow._currentUser.Age);
                allQuestions.AddRange(questions);
            }
            
            foreach (var question in allQuestions.Take(10)) // Show first 10 questions
            {
                var questionCard = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(233, 236, 239)),
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
                    Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 22,
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                questionCard.Child = questionText;
                questionCard.MouseLeftButtonDown += (s, e) => ShowAnswerInChat(question);
                
                _mainWindow.TopicsContent.Children.Add(questionCard);
            }
        }

        public void ShowBlogsTab()
        {
            // Update tab colors
            _mainWindow.TopicsTab.Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125));
            _mainWindow.QuestionsTab.Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125));
            _mainWindow.BlogsTab.Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96));
            
            _mainWindow.TopicsContent.Children.Clear();
            
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
                    Background = Brushes.White,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(233, 236, 239)),
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
                    Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 5),
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                var titleText = new TextBlock
                {
                    Text = _languageService.GetText(blog.Title),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(44, 62, 80)),
                    TextWrapping = TextWrapping.Wrap,
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                titleStack.Children.Add(categoryText);
                titleStack.Children.Add(titleText);
                
                headerStack.Children.Add(iconText);
                headerStack.Children.Add(titleStack);
                
                var descText = new TextBlock
                {
                    Text = _languageService.GetText(blog.Description),
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125)),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 10, 0, 0),
                    LineHeight = 18,
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                stackPanel.Children.Add(headerStack);
                stackPanel.Children.Add(descText);
                blogCard.Child = stackPanel;
                
                blogCard.MouseLeftButtonDown += (s, e) => OpenBlogPage(blog.Title, blog.Description, blog.Category);
                
                _mainWindow.TopicsContent.Children.Add(blogCard);
            }
        }

        public void DisplayTopicsInPopup()
        {
            _mainWindow.TopicsContent.Children.Clear();
            
            foreach (var topic in _mainWindow._currentTopics)
            {
                var translatedTitle = _languageService.GetText(topic.Title);
                var translatedDesc = _languageService.GetText(topic.Description);
                
                var topicCard = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(248, 249, 250)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(233, 236, 239)),
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
                    Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 5),
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                var descText = new TextBlock
                {
                    Text = translatedDesc,
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125)),
                    TextWrapping = TextWrapping.Wrap,
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                stackPanel.Children.Add(titleText);
                stackPanel.Children.Add(descText);
                topicCard.Child = stackPanel;
                
                topicCard.MouseLeftButtonDown += (s, e) => SelectTopic(topic);
                
                _mainWindow.TopicsContent.Children.Add(topicCard);
            }
        }

        private void SelectTopic(Topic topic)
        {
            // Log topic selection activity
            if (_mainWindow._currentUser != null)
            {
                _loggingService.LogActivity(
                    _mainWindow._currentUser,
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
            _mainWindow.TopicsTab.Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125));
            _mainWindow.QuestionsTab.Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96));
            _mainWindow.BlogsTab.Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125));
            
            _mainWindow.TopicsContent.Children.Clear();
            
            // Add topic header
            var topicHeader = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(232, 245, 232)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
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
                Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = _uiManager.GetAppFontFamily()
            };
            
            topicHeader.Child = headerText;
            _mainWindow.TopicsContent.Children.Add(topicHeader);
            
            // Add back button
            var backButton = new Button
            {
                Content = "← Back to All Topics",
                Background = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                Foreground = Brushes.White,
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                Padding = new Thickness(15, 8, 15, 8),
                Margin = new Thickness(0, 0, 0, 15),
                HorizontalAlignment = HorizontalAlignment.Center,
                Cursor = Cursors.Hand,
                FontFamily = _uiManager.GetAppFontFamily()
            };
            
            backButton.Click += (s, e) => ShowTopicsTab();
            _mainWindow.TopicsContent.Children.Add(backButton);
            
            // Show questions for this topic
            var questions = _contentService.GetQuestionsForTopic(topic.Id, _mainWindow._currentUser.Age);
            
            foreach (var question in questions)
            {
                var questionCard = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(233, 236, 239)),
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
                    Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 22,
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                questionCard.Child = questionText;
                questionCard.MouseLeftButtonDown += (s, e) => ShowAnswerInChat(question);
                
                _mainWindow.TopicsContent.Children.Add(questionCard);
            }
        }

        private void ShowAnswerInChat(Question question)
        {
            // Log question selection activity
            if (_mainWindow._currentUser != null)
            {
                _loggingService.LogActivity(
                    _mainWindow._currentUser,
                    "Question",
                    question.Id.ToString(),
                    _languageService.GetText(question.QuestionText),
                    _languageService.GetText(question.Answer)
                );
            }
            
            _mainWindow.TopicsPopup.Visibility = Visibility.Collapsed;
            _mainWindow.AddMessageToChat($"Question: {_languageService.GetText(question.QuestionText)}", false);
            _mainWindow.AddMessageToChat($"Answer: {_languageService.GetText(question.Answer)}", false);
            
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
                    Background = new SolidColorBrush(Color.FromRgb(248, 249, 250)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(233, 236, 239)),
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
                    Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 15),
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                blogStack.Children.Add(headerText);
                
                foreach (var blog in relatedBlogs.Take(3))
                {
                    var blogCard = new Border
                    {
                        Background = Brushes.White,
                        BorderBrush = new SolidColorBrush(Color.FromRgb(233, 236, 239)),
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
                        Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                        Margin = new Thickness(0, 0, 0, 3),
                        FontFamily = _uiManager.GetAppFontFamily()
                    };
                    
                    var titleText = new TextBlock
                    {
                        Text = _languageService.GetText(blog.Item1),
                        FontSize = 14,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = new SolidColorBrush(Color.FromRgb(44, 62, 80)),
                        TextWrapping = TextWrapping.Wrap,
                        FontFamily = _uiManager.GetAppFontFamily()
                    };
                    
                    titleStack.Children.Add(categoryText);
                    titleStack.Children.Add(titleText);
                    
                    iconTitleStack.Children.Add(iconText);
                    iconTitleStack.Children.Add(titleStack);
                    
                    var descText = new TextBlock
                    {
                        Text = _languageService.GetText(blog.Item2),
                        FontSize = 12,
                        Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125)),
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 8, 0, 0),
                        LineHeight = 16,
                        FontFamily = _uiManager.GetAppFontFamily()
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
                    Foreground = new SolidColorBrush(Color.FromRgb(108, 117, 125)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 0),
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                blogStack.Children.Add(readMoreText);
                
                blogMessage.Child = blogStack;
                _mainWindow.ChatPanel.Children.Add(blogMessage);
                
                // Scroll to bottom
                _mainWindow.ChatScrollViewer.ScrollToEnd();
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

        private void OpenBlogPage(string title, string description, string category)
        {
            // Log blog selection activity
            if (_mainWindow._currentUser != null)
            {
                _loggingService.LogActivity(
                    _mainWindow._currentUser,
                    "Blog",
                    title,
                    _languageService.GetText(title),
                    _languageService.GetText(description)
                );
            }
            
            _mainWindow.TopicsPopup.Visibility = Visibility.Collapsed;
            ShowBlogPage(title, description, category);
        }

        private void ShowBlogPage(string title, string description, string category)
        {
            _mainWindow.BlogsPanel.Visibility = Visibility.Visible;
            _mainWindow.MainPanel.Visibility = Visibility.Collapsed;
            
            _mainWindow.BlogsList.Children.Clear();
            
            // Create blog content
            var blogContent = new Border
            {
                Background = Brushes.White,
                BorderBrush = new SolidColorBrush(Color.FromRgb(233, 236, 239)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(30),
                Margin = new Thickness(0, 0, 0, 20)
            };

            var contentStack = new StackPanel();
            
            // Category badge
            var categoryBadge = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(15, 8, 15, 8),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 0, 0, 20)
            };
            
            var categoryText = new TextBlock
            {
                Text = _languageService.GetText(category),
                Foreground = Brushes.White,
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                FontFamily = _uiManager.GetAppFontFamily()
            };
            
            categoryBadge.Child = categoryText;
            contentStack.Children.Add(categoryBadge);
            
            // Title
            var titleText = new TextBlock
            {
                Text = _languageService.GetText(title),
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 20),
                FontFamily = _uiManager.GetAppFontFamily()
            };
            contentStack.Children.Add(titleText);
            
            // Description
            var descText = new TextBlock
            {
                Text = _languageService.GetText(description),
                FontSize = 18,
                Foreground = new SolidColorBrush(Color.FromRgb(44, 62, 80)),
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 28,
                Margin = new Thickness(0, 0, 0, 30),
                FontFamily = _uiManager.GetAppFontFamily()
            };
            contentStack.Children.Add(descText);
            
            // Content sections based on category
            var contentSections = GetBlogContent(category);
            foreach (var section in contentSections)
            {
                var sectionBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(248, 249, 250)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(233, 236, 239)),
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
                    Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                    Margin = new Thickness(0, 0, 0, 10),
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                var sectionContent = new TextBlock
                {
                    Text = _languageService.GetText(section.Content),
                    FontSize = 16,
                    Foreground = new SolidColorBrush(Color.FromRgb(44, 62, 80)),
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 24,
                    FontFamily = _uiManager.GetAppFontFamily()
                };
                
                sectionStack.Children.Add(sectionTitle);
                sectionStack.Children.Add(sectionContent);
                sectionBorder.Child = sectionStack;
                
                contentStack.Children.Add(sectionBorder);
            }
            
            // Safety reminder
            var reminderBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 243, 205)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(255, 193, 7)),
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
                Foreground = new SolidColorBrush(Color.FromRgb(133, 100, 4)),
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 24,
                FontFamily = _uiManager.GetAppFontFamily()
            };
            
            reminderBorder.Child = reminderText;
            contentStack.Children.Add(reminderBorder);
            
            blogContent.Child = contentStack;
            _mainWindow.BlogsList.Children.Add(blogContent);
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
    }
}
