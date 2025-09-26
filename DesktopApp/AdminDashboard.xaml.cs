using ChildSafeSexEducation.Desktop.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ChildSafeSexEducation.Desktop
{
    public partial class AdminDashboard : Window
    {
        private readonly UserStorageService _userStorageService;
        private readonly ContentService _contentService;

        public AdminDashboard()
        {
            InitializeComponent();
            _userStorageService = new UserStorageService();
            _contentService = new ContentService();
            
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                // Load user count
                var users = _userStorageService.GetAllUsers();
                TotalUsersText.Text = users.Count.ToString();

                // Load questions count
                var questions = _contentService.GetQuestionsForTopic(1, 15); // Get all questions
                TotalQuestionsText.Text = questions.Count.ToString();

                // Set open tickets (hardcoded for now)
                OpenTicketsText.Text = "3";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            ShowContent(DashboardContent);
            SetActiveButton(DashboardButton);
        }

        private void ContentEditorButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open ContentEditor as a dialog window
                var contentEditor = new ContentEditor();
                contentEditor.Owner = this; // Set this window as owner
                contentEditor.ShowDialog();
                
                // Refresh dashboard data after editor is closed
                LoadDashboardData();
                
                // Refresh questions in ContentService
                _contentService.RefreshQuestions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening content editor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UserManagementButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowContent(UserManagementContent);
                SetActiveButton(UserManagementButton);
                
                // Load user management
                var userManagement = new UserManagement();
                UserManagementFrame.Content = userManagement;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user management: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ErrorTicketsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowContent(ErrorTicketsContent);
                SetActiveButton(ErrorTicketsButton);
                
                // Load error tickets
                var errorTickets = new ErrorTickets();
                ErrorTicketsFrame.Content = errorTickets;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading error tickets: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", 
                                       "Logout Confirmation", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void ShowContent(StackPanel content)
        {
            // Hide all content
            DashboardContent.Visibility = Visibility.Collapsed;
            ContentEditorContent.Visibility = Visibility.Collapsed;
            UserManagementContent.Visibility = Visibility.Collapsed;
            ErrorTicketsContent.Visibility = Visibility.Collapsed;

            // Show selected content
            content.Visibility = Visibility.Visible;
        }

        private void SetActiveButton(Button activeButton)
        {
            // Reset all buttons
            DashboardButton.Style = (Style)FindResource("NavButton");
            ContentEditorButton.Style = (Style)FindResource("NavButton");
            UserManagementButton.Style = (Style)FindResource("NavButton");
            ErrorTicketsButton.Style = (Style)FindResource("NavButton");

            // Set active button
            activeButton.Style = (Style)FindResource("ActiveNavButton");
        }
    }
}
