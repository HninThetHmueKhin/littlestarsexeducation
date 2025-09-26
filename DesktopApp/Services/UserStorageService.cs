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
            // Check if user already exists (by username)
            var existingUser = _users.FirstOrDefault(u => u.Username == user.Username);
            
            if (existingUser != null)
            {
                // Update existing user
                existingUser.Name = user.Name;
                // Encrypt password if it's not already encrypted
                existingUser.Password = PasswordEncryptionService.IsEncrypted(user.Password) 
                    ? user.Password 
                    : PasswordEncryptionService.EncryptPassword(user.Password);
                existingUser.Age = user.Age;
                existingUser.ParentName = user.ParentName;
                existingUser.ParentEmail = user.ParentEmail;
                existingUser.EmailNotificationsEnabled = user.EmailNotificationsEnabled;
                existingUser.PreferredLanguage = user.PreferredLanguage;
                existingUser.CreatedAt = user.CreatedAt;
            }
            else
            {
                // Encrypt password for new user
                user.Password = PasswordEncryptionService.EncryptPassword(user.Password);
                _users.Add(user);
            }
            
            SaveUsersToFile();
        }

        public User? GetUser(string name, int age)
        {
            return _users.FirstOrDefault(u => u.Name == name && u.Age == age);
        }

        public User? GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public bool UsernameExists(string username)
        {
            Console.WriteLine($"🔍 Checking if username '{username}' exists...");
            var exists = _users.Any(u => u.Username == username);
            Console.WriteLine($"🔍 Username '{username}' exists: {exists}");
            return exists;
        }

        public bool ValidateUser(string username, string password)
        {
            Console.WriteLine($"🔍 ValidateUser called with username: '{username}', password: '{"*".PadLeft(password.Length, '*')}'");
            var user = GetUserByUsername(username);
            Console.WriteLine($"🔍 User found: {user != null}");
            if (user != null)
            {
                Console.WriteLine($"🔍 User details - Name: '{user.Name}', Username: '{user.Username}', Password: '{"*".PadLeft(user.Password?.Length ?? 0, '*')}'");
                var passwordMatch = PasswordEncryptionService.VerifyPassword(password, user.Password);
                Console.WriteLine($"🔍 Password match: {passwordMatch}");
                return passwordMatch;
            }
            return false;
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
                Console.WriteLine($"🔍 Loading users from: {_usersFilePath}");
                if (File.Exists(_usersFilePath))
                {
                    var json = File.ReadAllText(_usersFilePath);
                    var users = JsonSerializer.Deserialize<List<User>>(json);
                    if (users != null)
                    {
                        _users.AddRange(users);
                        Console.WriteLine($"🔍 Loaded {users.Count} users from file");
                        foreach (var user in users)
                        {
                            Console.WriteLine($"🔍 User: Name='{user.Name}', Username='{user.Username}', Password='{"*".PadLeft(user.Password?.Length ?? 0, '*')}'");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"🔍 Users file not found at: {_usersFilePath}");
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

        // Method to migrate existing plain text passwords to encrypted ones
        public void MigratePasswordsToEncrypted()
        {
            bool hasChanges = false;
            foreach (var user in _users)
            {
                if (!PasswordEncryptionService.IsEncrypted(user.Password))
                {
                    Console.WriteLine($"🔐 Migrating password for user: {user.Username}");
                    user.Password = PasswordEncryptionService.EncryptPassword(user.Password);
                    hasChanges = true;
                }
            }
            
            if (hasChanges)
            {
                SaveUsersToFile();
                Console.WriteLine("🔐 Password migration completed successfully");
            }
            else
            {
                Console.WriteLine("🔐 All passwords are already encrypted");
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
