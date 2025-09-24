using ChildSafeSexEducation.Desktop.Models;
using System.Text.Json;
using System.IO;

namespace ChildSafeSexEducation.Desktop.Services
{
    public class UserStorageService
    {
        private readonly string _usersFilePath;
        private readonly List<User> _users;

        public UserStorageService()
        {
            _usersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
            _users = new List<User>();
            LoadUsersFromFile();
        }

        public void SaveUser(User user)
        {
            // Check if user already exists (by name and age)
            var existingUser = _users.FirstOrDefault(u => u.Name == user.Name && u.Age == user.Age);
            
            if (existingUser != null)
            {
                // Update existing user
                existingUser.ParentName = user.ParentName;
                existingUser.ParentEmail = user.ParentEmail;
                existingUser.EmailNotificationsEnabled = user.EmailNotificationsEnabled;
                existingUser.PreferredLanguage = user.PreferredLanguage;
                existingUser.CreatedAt = user.CreatedAt;
            }
            else
            {
                // Add new user
                _users.Add(user);
            }
            
            SaveUsersToFile();
        }

        public User? GetUser(string name, int age)
        {
            return _users.FirstOrDefault(u => u.Name == name && u.Age == age);
        }

        public List<User> GetAllUsers()
        {
            return _users.ToList();
        }

        public List<User> GetUsersWithEmailNotifications()
        {
            return _users.Where(u => u.EmailNotificationsEnabled && !string.IsNullOrEmpty(u.ParentEmail)).ToList();
        }

        public void DeleteUser(string name, int age)
        {
            var user = _users.FirstOrDefault(u => u.Name == name && u.Age == age);
            if (user != null)
            {
                _users.Remove(user);
                SaveUsersToFile();
            }
        }

        public void UpdateUserEmailPreferences(string name, int age, bool emailEnabled, string parentEmail = "", string parentName = "")
        {
            var user = _users.FirstOrDefault(u => u.Name == name && u.Age == age);
            if (user != null)
            {
                user.EmailNotificationsEnabled = emailEnabled;
                if (!string.IsNullOrEmpty(parentEmail))
                    user.ParentEmail = parentEmail;
                if (!string.IsNullOrEmpty(parentName))
                    user.ParentName = parentName;
                
                SaveUsersToFile();
            }
        }

        private void LoadUsersFromFile()
        {
            try
            {
                if (File.Exists(_usersFilePath))
                {
                    var json = File.ReadAllText(_usersFilePath);
                    var users = JsonSerializer.Deserialize<List<User>>(json);
                    if (users != null)
                    {
                        _users.AddRange(users);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
            }
        }

        private void SaveUsersToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(_usersFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }

        // Method to get user statistics
        public Dictionary<string, int> GetUserStats()
        {
            return new Dictionary<string, int>
            {
                ["TotalUsers"] = _users.Count,
                ["UsersWithEmail"] = _users.Count(u => !string.IsNullOrEmpty(u.ParentEmail)),
                ["EmailNotificationsEnabled"] = _users.Count(u => u.EmailNotificationsEnabled),
                ["EnglishUsers"] = _users.Count(u => u.PreferredLanguage == "English"),
                ["BurmeseUsers"] = _users.Count(u => u.PreferredLanguage == "Burmese")
            };
        }
    }
}
