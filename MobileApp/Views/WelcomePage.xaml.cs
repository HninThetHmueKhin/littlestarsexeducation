using ChildSafeSexEducation.Mobile.Models;
using ChildSafeSexEducation.Mobile.Services;

namespace ChildSafeSexEducation.Mobile.Views;

public partial class WelcomePage : ContentPage
{
    private readonly LanguageService _languageService;

    public WelcomePage()
    {
        InitializeComponent();
        _languageService = LanguageService.Instance;
        
        // Initialize with English by default
        _languageService.SetLanguage(Language.English);
        UpdateUIWithCurrentLanguage();
    }

    private void EnglishButton_Clicked(object sender, EventArgs e)
    {
        _languageService.SetLanguage(Language.English);
        UpdateUIWithCurrentLanguage();
        ShowWelcomeForm();
    }

    private void BurmeseButton_Clicked(object sender, EventArgs e)
    {
        _languageService.SetLanguage(Language.Burmese);
        UpdateUIWithCurrentLanguage();
        ShowWelcomeForm();
    }

    private void ShowWelcomeForm()
    {
        LanguagePanel.IsVisible = false;
        WelcomePanel.IsVisible = true;
    }

    private void UpdateUIWithCurrentLanguage()
    {
        // Update language selection screen
        LanguageTitle.Text = _languageService.GetText("select_language");
        EnglishButton.Text = _languageService.GetText("language_english");
        BurmeseButton.Text = _languageService.GetText("language_burmese");

        // Update welcome form
        WelcomeTitle.Text = _languageService.GetText("welcome_title");
        WelcomeSubtitle.Text = _languageService.GetText("welcome_subtitle");
        NameLabel.Text = _languageService.GetText("enter_name");
        AgeLabel.Text = _languageService.GetText("select_age");
        StartButton.Text = _languageService.GetText("start_button");
        FooterLabel.Text = "Child-friendly education for ages 8-15";

        // Update age picker items
        AgePicker.Items.Clear();
        AgePicker.Items.Add(_languageService.GetText("select_age"));
        AgePicker.Items.Add(_languageService.GetText("age_8"));
        AgePicker.Items.Add(_languageService.GetText("age_9"));
        AgePicker.Items.Add(_languageService.GetText("age_10"));
        AgePicker.Items.Add(_languageService.GetText("age_11"));
        AgePicker.Items.Add(_languageService.GetText("age_12"));
        AgePicker.Items.Add(_languageService.GetText("age_13"));
        AgePicker.Items.Add(_languageService.GetText("age_14"));
        AgePicker.Items.Add(_languageService.GetText("age_15"));
    }

    private async void StartButton_Clicked(object sender, EventArgs e)
    {
        var name = NameEntry.Text?.Trim();
        var selectedAge = AgePicker.SelectedIndex;

        if (string.IsNullOrEmpty(name) || selectedAge <= 0)
        {
            await ModernDialogService.ShowWarning(
                _languageService.GetText("missing_name_age"), 
                _languageService.GetText("missing_information"));
            return;
        }

        var age = selectedAge + 7; // Convert picker index to age (8-15)

        if (age < 8 || age > 15)
        {
            await ModernDialogService.ShowWarning(
                _languageService.GetText("invalid_age_message"), 
                _languageService.GetText("invalid_age"));
            return;
        }

        var user = new User { Name = name, Age = age };
        
        // Navigate to main page
        await Navigation.PushAsync(new MainPage(user));
    }
}
