using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChildSafeSexEducation.Desktop.Services;

namespace ChildSafeSexEducation.Desktop.Managers
{
    public class UIManager
    {
        private readonly LanguageService _languageService;
        private readonly MainWindow _mainWindow;

        public UIManager(MainWindow mainWindow, LanguageService languageService)
        {
            _mainWindow = mainWindow;
            _languageService = languageService;
        }

        public System.Windows.Media.FontFamily GetAppFontFamily()
        {
            if (_languageService.CurrentLanguage == Services.Language.English)
            {
                return new System.Windows.Media.FontFamily("Segoe UI, Arial, sans-serif");
            }
            else
            {
                return new System.Windows.Media.FontFamily("\"Myanmar Text\", \"Noto Sans Myanmar\", \"Padauk\", Segoe UI, Arial, sans-serif");
            }
        }

        public void UpdateFormLabels()
        {
            if (_languageService.CurrentLanguage == Services.Language.English)
            {
                _mainWindow.NameLabel.Text = "Enter your name:";
                _mainWindow.UsernameLabel.Text = "Enter username:";
                _mainWindow.PasswordLabel.Text = "Enter password:";
                _mainWindow.AgeLabel.Text = "Select your age:";
                _mainWindow.StartButton.Content = "Start Learning";
            }
            else
            {
                _mainWindow.NameLabel.Text = "သင့်နာမည်ကို ရိုက်ထည့်ပါ:";
                _mainWindow.UsernameLabel.Text = "အသုံးပြုသူအမည်ကို ရိုက်ထည့်ပါ:";
                _mainWindow.PasswordLabel.Text = "စကားဝှက်ကို ရိုက်ထည့်ပါ:";
                _mainWindow.AgeLabel.Text = "သင့်အသက်ကို ရွေးချယ်ပါ:";
                _mainWindow.StartButton.Content = "သင်ယူမှု စတင်ရန်";
            }
        }

        public void UpdateLoginLabels()
        {
            if (_languageService.CurrentLanguage == Services.Language.English)
            {
                _mainWindow.LoginTitle.Text = "Welcome Back!";
                _mainWindow.LoginSubtitle.Text = "Please sign in to continue your learning journey";
                _mainWindow.LoginUsernameLabel.Text = "Username:";
                _mainWindow.LoginPasswordLabel.Text = "Password:";
                _mainWindow.LoginButton.Content = "Login";
                _mainWindow.SwitchToRegisterButton.Content = "Create Account";
                _mainWindow.SwitchToLoginButton.Content = "Already have an account?";
            }
            else
            {
                _mainWindow.LoginTitle.Text = "ပြန်လည်ကြိုဆိုပါတယ်!";
                _mainWindow.LoginSubtitle.Text = "သင့်သင်ယူမှု ခရီးကို ဆက်လက်လုပ်ဆောင်ရန် ကျေးဇူးပြု၍ ဝင်ရောက်ပါ";
                _mainWindow.LoginUsernameLabel.Text = "အသုံးပြုသူအမည်:";
                _mainWindow.LoginPasswordLabel.Text = "စကားဝှက်:";
                _mainWindow.LoginButton.Content = "လော့ဂ်အင်ဝင်ရန်";
                _mainWindow.SwitchToRegisterButton.Content = "အကောင့်ဖွင့်ရန်";
                _mainWindow.SwitchToLoginButton.Content = "အကောင့်ရှိပြီးသားလား?";
            }
        }

        public void UpdateAllUIElementsToEnglish()
        {
            // Language selection screen
            if (_mainWindow.LanguageTitle != null)
                _mainWindow.LanguageTitle.Text = "Choose Language";
            if (_mainWindow.ContinueButton != null)
                _mainWindow.ContinueButton.Content = "✨ Continue";
            
            // Main interface buttons
            if (_mainWindow.TopicsButton != null)
                _mainWindow.TopicsButton.Content = "📚 Topics";
            if (_mainWindow.SendDailyLogButton != null)
                _mainWindow.SendDailyLogButton.Content = "📧 Send Daily Log";
            if (_mainWindow.TestEmailButton != null)
                _mainWindow.TestEmailButton.Content = "🧪 Test";
            if (_mainWindow.ExitButton != null)
                _mainWindow.ExitButton.Content = "🚪 Exit";
            
            // Form labels
            if (_mainWindow.NameLabel != null)
                _mainWindow.NameLabel.Text = "Enter your name:";
            if (_mainWindow.UsernameLabel != null)
                _mainWindow.UsernameLabel.Text = "Enter username:";
            if (_mainWindow.PasswordLabel != null)
                _mainWindow.PasswordLabel.Text = "Enter password:";
            if (_mainWindow.AgeLabel != null)
                _mainWindow.AgeLabel.Text = "Select your age:";
            if (_mainWindow.ParentNameLabel != null)
                _mainWindow.ParentNameLabel.Text = "Parent/Guardian Name:";
            if (_mainWindow.ParentEmailLabel != null)
                _mainWindow.ParentEmailLabel.Text = "Parent/Guardian Email:";
            if (_mainWindow.StartButton != null)
                _mainWindow.StartButton.Content = "Start Learning";
            if (_mainWindow.LoginButton != null)
                _mainWindow.LoginButton.Content = "Login";
            if (_mainWindow.LoginTitle != null)
                _mainWindow.LoginTitle.Text = "Welcome Back!";
            if (_mainWindow.LoginSubtitle != null)
                _mainWindow.LoginSubtitle.Text = "Please sign in to continue your learning journey";
            if (_mainWindow.WelcomeTitle != null)
                _mainWindow.WelcomeTitle.Text = "⭐ Little Star";
            if (_mainWindow.WelcomeSubtitle != null)
                _mainWindow.WelcomeSubtitle.Text = "Safe and Healthy Learning Platform for Children";
            if (_mainWindow.LoginUsernameLabel != null)
                _mainWindow.LoginUsernameLabel.Text = "Username:";
            if (_mainWindow.LoginPasswordLabel != null)
                _mainWindow.LoginPasswordLabel.Text = "Password:";
            
            // Language buttons
            if (_mainWindow.EnglishButton != null)
                _mainWindow.EnglishButton.Content = "🇺🇸 English";
            if (_mainWindow.BurmeseButton != null)
                _mainWindow.BurmeseButton.Content = "🇲🇲 ဗမာ";
            
            // Chat input
            if (_mainWindow.ChatInput != null)
                _mainWindow.ChatInput.Text = "Type your message here...";
            if (_mainWindow.SendButton != null)
                _mainWindow.SendButton.Content = "Send";
            
            // Update age combobox
            UpdateAgeComboBox();
            
            // Topics popup
            if (_mainWindow.TopicsTitle != null)
                _mainWindow.TopicsTitle.Text = "Learning Topics";
            if (_mainWindow.TopicsTab != null)
                _mainWindow.TopicsTab.Content = "Topics";
            if (_mainWindow.QuestionsTab != null)
                _mainWindow.QuestionsTab.Content = "Questions";
            if (_mainWindow.BlogsTab != null)
                _mainWindow.BlogsTab.Content = "Blogs";
            if (_mainWindow.CloseTopicsButton != null)
                _mainWindow.CloseTopicsButton.Content = "Close";
        }

        public void UpdateAllUIElementsToBurmese()
        {
            // Language selection screen
            if (_mainWindow.LanguageTitle != null)
                _mainWindow.LanguageTitle.Text = "ဘာသာစကား ရွေးချယ်ပါ";
            if (_mainWindow.ContinueButton != null)
                _mainWindow.ContinueButton.Content = "ဆက်လက်လုပ်ဆောင်ရန်";
            
            // Main interface buttons
            if (_mainWindow.TopicsButton != null)
                _mainWindow.TopicsButton.Content = "📚 ခေါင်းစဉ်များ";
            if (_mainWindow.SendDailyLogButton != null)
                _mainWindow.SendDailyLogButton.Content = "📧 နေ့စဉ် လော့ဂ်ပို့ပေးရန်";
            if (_mainWindow.TestEmailButton != null)
                _mainWindow.TestEmailButton.Content = "🧪 စမ်းသပ်ရန်";
            if (_mainWindow.ExitButton != null)
                _mainWindow.ExitButton.Content = "🚪 ထွက်ရန်";
            
            // Form labels
            if (_mainWindow.NameLabel != null)
                _mainWindow.NameLabel.Text = "သင့်နာမည်ကို ရိုက်ထည့်ပါ:";
            if (_mainWindow.UsernameLabel != null)
                _mainWindow.UsernameLabel.Text = "အသုံးပြုသူအမည်ကို ရိုက်ထည့်ပါ:";
            if (_mainWindow.PasswordLabel != null)
                _mainWindow.PasswordLabel.Text = "စကားဝှက်ကို ရိုက်ထည့်ပါ:";
            if (_mainWindow.AgeLabel != null)
                _mainWindow.AgeLabel.Text = "သင့်အသက်ကို ရွေးချယ်ပါ:";
            if (_mainWindow.ParentNameLabel != null)
                _mainWindow.ParentNameLabel.Text = "မိဘ/အုပ်ထိန်းသူ၏ နာမည်:";
            if (_mainWindow.ParentEmailLabel != null)
                _mainWindow.ParentEmailLabel.Text = "မိဘ/အုပ်ထိန်းသူ၏ အီးမေးလ်:";
            if (_mainWindow.StartButton != null)
                _mainWindow.StartButton.Content = "သင်ယူမှု စတင်ရန်";
            if (_mainWindow.LoginButton != null)
                _mainWindow.LoginButton.Content = "လော့ဂ်အင်ဝင်ရန်";
            if (_mainWindow.LoginTitle != null)
                _mainWindow.LoginTitle.Text = "ပြန်လည်ကြိုဆိုပါတယ်!";
            if (_mainWindow.LoginSubtitle != null)
                _mainWindow.LoginSubtitle.Text = "သင့်သင်ယူမှု ခရီးကို ဆက်လက်လုပ်ဆောင်ရန် ကျေးဇူးပြု၍ ဝင်ရောက်ပါ";
            if (_mainWindow.WelcomeTitle != null)
                _mainWindow.WelcomeTitle.Text = "⭐ Little Star";
            if (_mainWindow.WelcomeSubtitle != null)
                _mainWindow.WelcomeSubtitle.Text = "ကလေးများအတွက် လုံခြုံပြီး ကျန်းမာသော သင်ယူမှု ပလက်ဖောင်း";
            if (_mainWindow.LoginUsernameLabel != null)
                _mainWindow.LoginUsernameLabel.Text = "အသုံးပြုသူအမည်:";
            if (_mainWindow.LoginPasswordLabel != null)
                _mainWindow.LoginPasswordLabel.Text = "စကားဝှက်:";
            
            // Language buttons
            if (_mainWindow.EnglishButton != null)
                _mainWindow.EnglishButton.Content = "🇺🇸 English";
            if (_mainWindow.BurmeseButton != null)
                _mainWindow.BurmeseButton.Content = "🇲🇲 ဗမာ";
            
            // Chat input
            if (_mainWindow.ChatInput != null)
                _mainWindow.ChatInput.Text = "သင့်စာကို ဤနေရာတွင် ရိုက်ထည့်ပါ...";
            if (_mainWindow.SendButton != null)
                _mainWindow.SendButton.Content = "ပို့ရန်";
            
            // Update age combobox
            UpdateAgeComboBox();
            
            // Topics popup
            if (_mainWindow.TopicsTitle != null)
                _mainWindow.TopicsTitle.Text = "သင်ယူမှု ခေါင်းစဉ်များ";
            if (_mainWindow.TopicsTab != null)
                _mainWindow.TopicsTab.Content = "ခေါင်းစဉ်များ";
            if (_mainWindow.QuestionsTab != null)
                _mainWindow.QuestionsTab.Content = "မေးခွန်းများ";
            if (_mainWindow.BlogsTab != null)
                _mainWindow.BlogsTab.Content = "ဘလော့များ";
            if (_mainWindow.CloseTopicsButton != null)
                _mainWindow.CloseTopicsButton.Content = "ပိတ်ရန်";
        }

        private void UpdateAgeComboBox()
        {
            _mainWindow.AgeComboBox.Items.Clear();
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
                _mainWindow.AgeComboBox.Items.Add(item);
            }
        }

        public void ResetLanguageButtonStyles()
        {
            // Reset English button to default style
            if (_mainWindow.EnglishButton != null)
            {
                _mainWindow.EnglishButton.Background = new SolidColorBrush(Colors.White);
                _mainWindow.EnglishButton.Foreground = new SolidColorBrush(Colors.Black);
                _mainWindow.EnglishButton.BorderBrush = new SolidColorBrush(Colors.DodgerBlue);
                _mainWindow.EnglishButton.BorderThickness = new Thickness(2);
                _mainWindow.EnglishButton.FontWeight = FontWeights.Normal;
                _mainWindow.EnglishButton.FontSize = 14;
                _mainWindow.EnglishButton.Content = "🇺🇸 English";
            }
            
            // Reset Burmese button to default style
            if (_mainWindow.BurmeseButton != null)
            {
                _mainWindow.BurmeseButton.Background = new SolidColorBrush(Colors.White);
                _mainWindow.BurmeseButton.Foreground = new SolidColorBrush(Colors.Black);
                _mainWindow.BurmeseButton.BorderBrush = new SolidColorBrush(Colors.DodgerBlue);
                _mainWindow.BurmeseButton.BorderThickness = new Thickness(2);
                _mainWindow.BurmeseButton.FontWeight = FontWeights.Normal;
                _mainWindow.BurmeseButton.FontSize = 14;
                _mainWindow.BurmeseButton.Content = "🇲🇲 ဗမာ";
            }
        }
    }
}