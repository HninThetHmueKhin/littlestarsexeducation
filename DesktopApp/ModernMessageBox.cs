using System.Windows;

namespace ChildSafeSexEducation.Desktop
{
    public static class ModernMessageBox
    {
        public static MessageBoxResult Show(string message, string title = "Dialog", 
            MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None)
        {
            return ModernDialog.Show(message, title, buttons, icon);
        }

        public static MessageBoxResult ShowError(string message, string title = "Error")
        {
            return Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult ShowWarning(string message, string title = "Warning")
        {
            return Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static MessageBoxResult ShowInfo(string message, string title = "Information")
        {
            return Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult ShowQuestion(string message, string title = "Question")
        {
            return Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static MessageBoxResult ShowConfirmation(string message, string title = "Confirm")
        {
            return Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
