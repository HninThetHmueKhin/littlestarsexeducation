using ChildSafeSexEducation.Mobile.Models;
using ChildSafeSexEducation.Mobile.Services;

namespace ChildSafeSexEducation.Mobile.Views;

public partial class MainPage : ContentPage
{
    private readonly User _user;
    private readonly ContentService _contentService;
    private readonly NLPService _nlpService;
    private readonly LanguageService _languageService;

    public MainPage(User user)
    {
        InitializeComponent();
        _user = user;
        _contentService = new ContentService();
        _nlpService = new NLPService(_contentService);
        _languageService = LanguageService.Instance;
        
        UpdateUIWithCurrentLanguage();
        
        WelcomeLabel.Text = $"Hi {user.Name}! 👋";
        
        // Add welcome message
        AddMessageToChat(_languageService.GetText("welcome_chat_message"), false);
    }

    private void UpdateUIWithCurrentLanguage()
    {
        TopicsButton.Text = _languageService.GetText("topics_button");
        ChatEntry.Placeholder = _languageService.GetText("chat_input_placeholder");
        SendButton.Text = _languageService.GetText("send_button");
    }

    private async void TopicsButton_Clicked(object sender, EventArgs e)
    {
        var topics = _contentService.GetTopicsForAge(_user.Age);
        await Navigation.PushAsync(new TopicsPage(topics, _user));
    }

    private void SendButton_Clicked(object sender, EventArgs e)
    {
        SendMessage();
    }

    private void ChatEntry_Completed(object sender, EventArgs e)
    {
        SendMessage();
    }

    private void SendMessage()
    {
        var message = ChatEntry.Text?.Trim();
        if (string.IsNullOrEmpty(message)) return;

        // Add user message
        AddMessageToChat(message, true);
        ChatEntry.Text = "";

        // Add loading message
        var loadingLabel = AddMessageToChat("Thinking...", false);
        
        // Process the question
        var response = _nlpService.ProcessQuestion(message, _user.Age);
        
        // Remove loading message and add response
        ChatStackLayout.Children.Remove(loadingLabel);
        AddMessageToChat(response, false);
    }

    private Label AddMessageToChat(string message, bool isUser)
    {
        var messageLabel = new Label
        {
            Text = message,
            FontSize = 14,
            TextColor = isUser ? Colors.White : Color.FromArgb("#2C3E50"),
            BackgroundColor = isUser ? Color.FromArgb("#27AE60") : Color.FromArgb("#E8F5E8"),
            Padding = new Thickness(15),
            Margin = new Thickness(isUser ? 50 : 0, 0, isUser ? 0 : 50, 0),
            HorizontalOptions = isUser ? LayoutOptions.End : LayoutOptions.Start,
            HorizontalTextAlignment = isUser ? TextAlignment.End : TextAlignment.Start,
            VerticalTextAlignment = TextAlignment.Center,
            LineBreakMode = LineBreakMode.WordWrap,
            MaximumWidthRequest = 300
        };

        ChatStackLayout.Children.Add(messageLabel);
        
        // Scroll to bottom
        Device.BeginInvokeOnMainThread(async () =>
        {
            await ChatScrollView.ScrollToAsync(0, ChatScrollView.ContentSize.Height, false);
        });

        return messageLabel;
    }
}
