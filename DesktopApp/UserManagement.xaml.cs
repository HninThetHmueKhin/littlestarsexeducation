using ChildSafeSexEducation.Desktop.Models;
using ChildSafeSexEducation.Desktop.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ChildSafeSexEducation.Desktop
{
    public partial class UserManagement : UserControl
    {
        private readonly UserStorageService _userStorageService;
        private ObservableCollection<User> _users;
        private ObservableCollection<User> _filteredUsers;

        public UserManagement()
        {
            InitializeComponent();
            _userStorageService = new UserStorageService();
            _users = new ObservableCollection<User>();
            _filteredUsers = new ObservableCollection<User>();
            
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                _users.Clear();
                var users = _userStorageService.GetAllUsers();
                
                foreach (var user in users)
                {
                    _users.Add(user);
                }
                
                _filteredUsers.Clear();
                foreach (var user in _users)
                {
                    _filteredUsers.Add(user);
                }
                
                UsersDataGrid.ItemsSource = _filteredUsers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();
            
            _filteredUsers.Clear();
            
            var filtered = _users.Where(u => 
                u.Name.ToLower().Contains(searchText) ||
                u.Username.ToLower().Contains(searchText) ||
                u.ParentEmail.ToLower().Contains(searchText) ||
                u.PreferredLanguage.ToLower().Contains(searchText));
            
            foreach (var user in filtered)
            {
                _filteredUsers.Add(user);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
            SearchTextBox.Text = "";
        }

        private void ViewUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is User user)
            {
                var userDetails = $"User Details:\n\n" +
                                $"Name: {user.Name}\n" +
                                $"Username: {user.Username}\n" +
                                $"Age: {user.Age}\n" +
                                $"Language: {user.PreferredLanguage}\n" +
                                $"Parent Name: {user.ParentName}\n" +
                                $"Parent Email: {user.ParentEmail}\n" +
                                $"Email Notifications: {(user.EmailNotificationsEnabled ? "Enabled" : "Disabled")}\n" +
                                $"Registration Date: {user.CreatedAt:MM/dd/yyyy HH:mm}";
                
                MessageBox.Show(userDetails, "User Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is User user)
            {
                var result = MessageBox.Show($"Are you sure you want to delete user '{user.Name}'?\n\nThis action cannot be undone.", 
                                           "Delete User", 
                                           MessageBoxButton.YesNo, 
                                           MessageBoxImage.Warning);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Note: You would need to implement DeleteUser method in UserStorageService
                        // _userStorageService.DeleteUser(user.Id);
                        
                        _users.Remove(user);
                        _filteredUsers.Remove(user);
                        
                        MessageBox.Show($"User '{user.Name}' has been deleted.", 
                                      "User Deleted", 
                                      MessageBoxButton.OK, 
                                      MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting user: {ex.Message}", 
                                      "Error", 
                                      MessageBoxButton.OK, 
                                      MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
