using ChildSafeSexEducation.Mobile.Models;
using ChildSafeSexEducation.Mobile.Services;

namespace ChildSafeSexEducation.Mobile.Views;

public partial class TopicsPage : ContentPage
{
    private readonly List<Topic> _topics;
    private readonly User _user;
    private readonly LanguageService _languageService;

    public TopicsPage(List<Topic> topics, User user)
    {
        InitializeComponent();
        _topics = topics;
        _user = user;
        _languageService = LanguageService.Instance;
        DisplayTopics();
    }

    private void DisplayTopics()
    {
        foreach (var topic in _topics)
        {
            var topicCard = new Frame
            {
                BackgroundColor = Color.FromArgb("#F8F9FA"),
                BorderColor = Color.FromArgb("#E9ECEF"),
                CornerRadius = 15,
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 15),
                HasShadow = true
            };

            var stackPanel = new StackLayout();

            var titleLabel = new Label
            {
                Text = _languageService.GetText(topic.Title),
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#00D4AA"),
                Margin = new Thickness(0, 0, 0, 10)
            };

            var descLabel = new Label
            {
                Text = _languageService.GetText(topic.Description),
                FontSize = 14,
                TextColor = Color.FromArgb("#636E72"),
                LineBreakMode = LineBreakMode.WordWrap
            };

            stackPanel.Children.Add(titleLabel);
            stackPanel.Children.Add(descLabel);
            topicCard.Content = stackPanel;

            // Add tap gesture
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) => await ShowQuestions(topic);
            topicCard.GestureRecognizers.Add(tapGesture);

            TopicsStackLayout.Children.Add(topicCard);
        }
    }

    private async Task ShowQuestions(Topic topic)
    {
        await Navigation.PushAsync(new QuestionsPage(topic, _user));
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
