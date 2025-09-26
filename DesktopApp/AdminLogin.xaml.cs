using System.Windows;

namespace ChildSafeSexEducation.Desktop
{
    public partial class AdminLogin : Window
    {
        public AdminLogin()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text.Trim();
            var password = PasswordBox.Password.Trim();

            // Check admin credentials
            if (username == "admin" && password == "admin")
            {
                // Open admin dashboard
                var adminDashboard = new AdminDashboard();
                adminDashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.", 
                              "Login Failed", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Warning);
                PasswordBox.Password = "";
                UsernameTextBox.Focus();
            }
        }
    }
}
