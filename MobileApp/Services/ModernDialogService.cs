using Microsoft.Maui.Controls;

namespace ChildSafeSexEducation.Mobile.Services
{
    public enum DialogType
    {
        Info,
        Warning,
        Error,
        Question,
        Confirmation
    }

    public static class ModernDialogService
    {
        public static async Task<bool> ShowInfo(string message, string title)
        {
            return await ShowDialog(message, title, DialogType.Info, "OK");
        }

        public static async Task<bool> ShowWarning(string message, string title)
        {
            return await ShowDialog(message, title, DialogType.Warning, "OK");
        }

        public static async Task<bool> ShowError(string message, string title)
        {
            return await ShowDialog(message, title, DialogType.Error, "OK");
        }

        public static async Task<bool> ShowQuestion(string message, string title)
        {
            return await ShowDialog(message, title, DialogType.Question, "Yes", "No");
        }

        public static async Task<bool> ShowConfirmation(string message, string title)
        {
            return await ShowDialog(message, title, DialogType.Confirmation, "Yes", "No");
        }

        private static async Task<bool> ShowDialog(string message, string title, DialogType type, string acceptButton, string? cancelButton = null)
        {
            var currentPage = Application.Current?.MainPage;
            if (currentPage == null) return false;

            // Create modern dialog content
            var dialogContent = CreateDialogContent(message, title, type, acceptButton, cancelButton);

            // Show as popup
            var result = await currentPage.DisplayAlert(title, message, acceptButton, cancelButton);
            return result;
        }

        private static View CreateDialogContent(string message, string title, DialogType type, string acceptButton, string? cancelButton)
        {
            var mainStack = new StackLayout
            {
                BackgroundColor = Colors.White,
                Padding = new Thickness(20),
                Spacing = 15
            };

            // Header with icon and title
            var headerStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10
            };

            var iconLabel = new Label
            {
                Text = GetIconForType(type),
                FontSize = 24,
                VerticalOptions = LayoutOptions.Center
            };

            var titleLabel = new Label
            {
                Text = title,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = GetColorForType(type),
                VerticalOptions = LayoutOptions.Center
            };

            headerStack.Children.Add(iconLabel);
            headerStack.Children.Add(titleLabel);

            // Message
            var messageLabel = new Label
            {
                Text = message,
                FontSize = 14,
                TextColor = Colors.Black,
                LineBreakMode = LineBreakMode.WordWrap
            };

            // Buttons
            var buttonStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End,
                Spacing = 10
            };

            if (!string.IsNullOrEmpty(cancelButton))
            {
                var cancelBtn = new Button
                {
                    Text = cancelButton,
                    BackgroundColor = Colors.LightGray,
                    TextColor = Colors.Black,
                    Padding = new Thickness(20, 10),
                    CornerRadius = 8
                };
                buttonStack.Children.Add(cancelBtn);
            }

            var acceptBtn = new Button
            {
                Text = acceptButton,
                BackgroundColor = GetColorForType(type),
                TextColor = Colors.White,
                Padding = new Thickness(20, 10),
                CornerRadius = 8
            };
            buttonStack.Children.Add(acceptBtn);

            mainStack.Children.Add(headerStack);
            mainStack.Children.Add(messageLabel);
            mainStack.Children.Add(buttonStack);

            return mainStack;
        }

        private static string GetIconForType(DialogType type)
        {
            return type switch
            {
                DialogType.Info => "ℹ️",
                DialogType.Warning => "⚠️",
                DialogType.Error => "❌",
                DialogType.Question => "❓",
                DialogType.Confirmation => "❓",
                _ => "ℹ️"
            };
        }

        private static Color GetColorForType(DialogType type)
        {
            return type switch
            {
                DialogType.Info => Color.FromArgb("#0984E3"),
                DialogType.Warning => Color.FromArgb("#F39C12"),
                DialogType.Error => Color.FromArgb("#E74C3C"),
                DialogType.Question => Color.FromArgb("#0984E3"),
                DialogType.Confirmation => Color.FromArgb("#0984E3"),
                _ => Color.FromArgb("#0984E3")
            };
        }
    }
}
