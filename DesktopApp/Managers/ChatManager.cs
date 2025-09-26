using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChildSafeSexEducation.Desktop.Models;
using ChildSafeSexEducation.Desktop.Services;

namespace ChildSafeSexEducation.Desktop.Managers
{
    public class ChatManager
    {
        private readonly MainWindow _mainWindow;
        private readonly ContentService _contentService;
        private readonly NLPService _nlpService;
        private readonly LanguageService _languageService;
        private readonly LoggingService _loggingService;
        private readonly UIManager _uiManager;

        public ChatManager(MainWindow mainWindow, ContentService contentService, NLPService nlpService, LanguageService languageService, LoggingService loggingService, UIManager uiManager)
        {
            _mainWindow = mainWindow;
            _contentService = contentService;
            _nlpService = nlpService;
            _languageService = languageService;
            _loggingService = loggingService;
            _uiManager = uiManager;
        }

        public void ProcessUserMessage(string message)
        {
            if (string.IsNullOrEmpty(message) || _mainWindow._currentUser == null) return;

            // Add user message
            AddMessageToChat(message, true);
            _mainWindow.ChatInput.Text = "";

            // Process with NLP service for content filtering
            var response = _nlpService.ProcessMessage(message);
            AddMessageToChat(response, false);
        }

        public Border AddMessageToChat(string message, bool isUser)
        {
            if (_mainWindow.ChatPanel == null || _mainWindow.ChatScrollViewer == null)
            {
                return null;
            }
            
            var messageBorder = new Border
            {
                Background = isUser ? 
                    new SolidColorBrush(Color.FromRgb(39, 174, 96)) :
                    new SolidColorBrush(Color.FromRgb(232, 245, 232)),
                BorderBrush = isUser ? 
                    new SolidColorBrush(Color.FromRgb(39, 174, 96)) :
                    new SolidColorBrush(Color.FromRgb(195, 230, 195)),
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
                    Brushes.White :
                    new SolidColorBrush(Color.FromRgb(44, 62, 80)),
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14,
                FontFamily = _uiManager.GetAppFontFamily()
            };

            messageBorder.Child = messageText;
            _mainWindow.ChatPanel.Children.Add(messageBorder);
            
            // Scroll to bottom
            _mainWindow.ChatScrollViewer.ScrollToEnd();
            
            return messageBorder;
        }

        public void AddWelcomeMessage()
        {
            if (_mainWindow.ChatPanel != null)
            {
                _mainWindow.ChatPanel.Children.Clear();
                if (_languageService.CurrentLanguage == Services.Language.English)
                {
                    AddMessageToChat("Hello! I'm here to help you learn about safe and healthy topics. You can ask me questions or click the Topics button to see what we can learn about together!", false);
                }
                else
                {
                    AddMessageToChat("မင်္ဂလာပါ! ကျွန်တော်က လုံခြုံပြီး ကျန်းမာသော အကြောင်းအရာများအကြောင်း သင်ယူရန် ကူညီပေးရန် ဤနေရာတွင် ရှိပါတယ်။ သင်မေးခွန်းများ မေးနိုင်ပြီး သို့မဟုတ် ခေါင်းစဉ်များ ခလုတ်ကို နှိပ်ပြီး ကျွန်တော်တို့ အတူတကွ သင်ယူနိုင်သော အရာများကို ကြည့်နိုင်ပါတယ်!", false);
                }
            }
        }
    }
}