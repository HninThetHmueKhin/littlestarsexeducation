using ChildSafeSexEducation.Mobile.Models;
using ChildSafeSexEducation.Mobile.Services;

namespace ChildSafeSexEducation.Mobile.Views;

public partial class AnswerPage : ContentPage
{
    private readonly LanguageService _languageService;

    public AnswerPage(Question question)
    {
        InitializeComponent();
        _languageService = LanguageService.Instance;
        
        QuestionLabel.Text = _languageService.GetText(question.QuestionText);
        AnswerLabel.Text = _languageService.GetText(question.Answer);
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
