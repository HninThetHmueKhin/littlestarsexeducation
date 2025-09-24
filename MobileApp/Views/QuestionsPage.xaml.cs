using ChildSafeSexEducation.Mobile.Models;
using ChildSafeSexEducation.Mobile.Services;

namespace ChildSafeSexEducation.Mobile.Views;

public partial class QuestionsPage : ContentPage
{
    private readonly Topic _topic;
    private readonly User _user;
    private readonly ContentService _contentService;
    private readonly LanguageService _languageService;

    public QuestionsPage(Topic topic, User user)
    {
        InitializeComponent();
        _topic = topic;
        _user = user;
        _contentService = new ContentService();
        _languageService = LanguageService.Instance;
        
        TopicTitleLabel.Text = _languageService.GetText(_topic.Title);
        DisplayQuestions();
    }

    private void DisplayQuestions()
    {
        var questions = _contentService.GetQuestionsForTopic(_topic.Id, _user.Age);
        
        foreach (var question in questions)
        {
            var questionCard = new Frame
            {
                BackgroundColor = Color.FromArgb("#F8F9FA"),
                BorderColor = Color.FromArgb("#E9ECEF"),
                CornerRadius = 15,
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 15),
                HasShadow = true
            };

            var questionLabel = new Label
            {
                Text = _languageService.GetText(question.QuestionText),
                FontSize = 16,
                FontAttributes = FontAttributes.SemiBold,
                TextColor = Color.FromArgb("#00D4AA"),
                LineBreakMode = LineBreakMode.WordWrap
            };

            questionCard.Content = questionLabel;

            // Add tap gesture
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) => await ShowAnswer(question);
            questionCard.GestureRecognizers.Add(tapGesture);

            QuestionsStackLayout.Children.Add(questionCard);
        }
    }

    private async Task ShowAnswer(Question question)
    {
        await Navigation.PushAsync(new AnswerPage(question));
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
