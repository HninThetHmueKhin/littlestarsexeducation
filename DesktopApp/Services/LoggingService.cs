using ChildSafeSexEducation.Desktop.Models;
using System.Text.Json;
using System.IO;

namespace ChildSafeSexEducation.Desktop.Services
{
    public class LoggingService
    {
        private readonly List<ActivityLog> _activityLogs;
        private readonly string _logFilePath;
        private readonly EmailService _emailService;

        public LoggingService()
        {
            _activityLogs = new List<ActivityLog>();
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "activity_logs.json");
            _emailService = new EmailService();
            LoadLogsFromFile();
        }

        public void LogActivity(User user, string activityType, string activityId, string activityTitle, string activityDescription, int timeSpentSeconds = 0)
        {
            var activity = new ActivityLog
            {
                Id = _activityLogs.Count + 1,
                ChildName = user.Name,
                ChildAge = user.Age,
                ActivityType = activityType,
                ActivityId = activityId,
                ActivityTitle = activityTitle,
                ActivityDescription = activityDescription,
                Timestamp = DateTime.Now,
                TimeSpentSeconds = timeSpentSeconds,
                Language = user.PreferredLanguage,
                SessionId = GetCurrentSessionId()
            };

            _activityLogs.Add(activity);
            SaveLogsToFile();
        }

        public List<ActivityLog> GetTodayActivities(string childName)
        {
            var today = DateTime.Today;
            return _activityLogs.Where(log => 
                log.ChildName == childName && 
                log.Timestamp.Date == today).ToList();
        }

        public List<ActivityLog> GetActivitiesForDate(string childName, DateTime date)
        {
            return _activityLogs.Where(log => 
                log.ChildName == childName && 
                log.Timestamp.Date == date.Date).ToList();
        }

        public async Task<bool> SendDailyLogToParentAsync(User user)
        {
            if (!user.EmailNotificationsEnabled || string.IsNullOrEmpty(user.ParentEmail))
                return false;

            var todayActivities = GetTodayActivities(user.Name);
            if (!todayActivities.Any())
                return false; // No activities to report

            var dailyLog = new DailyLogSummary
            {
                ChildName = user.Name,
                ChildAge = user.Age,
                ParentName = user.ParentName,
                ParentEmail = user.ParentEmail,
                Date = DateTime.Today,
                Activities = todayActivities,
                TotalTimeSpentMinutes = todayActivities.Sum(a => a.TimeSpentSeconds) / 60,
                TopicsViewed = todayActivities.Count(a => a.ActivityType == "Topic"),
                QuestionsAsked = todayActivities.Count(a => a.ActivityType == "Question"),
                BlogsRead = todayActivities.Count(a => a.ActivityType == "Blog"),
                Language = user.PreferredLanguage
            };

            return await _emailService.SendDailyLogAsync(dailyLog);
        }

        public async Task SendDailyLogsToAllParentsAsync(List<User> users)
        {
            var tasks = users.Where(u => u.EmailNotificationsEnabled && !string.IsNullOrEmpty(u.ParentEmail))
                            .Select(u => SendDailyLogToParentAsync(u));
            
            await Task.WhenAll(tasks);
        }

        public void ClearOldLogs(int daysToKeep = 30)
        {
            var cutoffDate = DateTime.Today.AddDays(-daysToKeep);
            _activityLogs.RemoveAll(log => log.Timestamp.Date < cutoffDate);
            SaveLogsToFile();
        }

        private string GetCurrentSessionId()
        {
            // Generate a simple session ID based on current hour
            return $"session_{DateTime.Now:yyyyMMdd_HH}";
        }

        private void LoadLogsFromFile()
        {
            try
            {
                if (File.Exists(_logFilePath))
                {
                    var json = File.ReadAllText(_logFilePath);
                    var logs = JsonSerializer.Deserialize<List<ActivityLog>>(json);
                    if (logs != null)
                    {
                        _activityLogs.AddRange(logs);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading logs: {ex.Message}");
            }
        }

        private void SaveLogsToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(_activityLogs, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(_logFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving logs: {ex.Message}");
            }
        }

        // Method to get activity statistics
        public Dictionary<string, int> GetActivityStats(string childName, DateTime startDate, DateTime endDate)
        {
            var activities = _activityLogs.Where(log => 
                log.ChildName == childName && 
                log.Timestamp.Date >= startDate.Date && 
                log.Timestamp.Date <= endDate.Date).ToList();

            return new Dictionary<string, int>
            {
                ["TotalActivities"] = activities.Count,
                ["TopicsViewed"] = activities.Count(a => a.ActivityType == "Topic"),
                ["QuestionsAsked"] = activities.Count(a => a.ActivityType == "Question"),
                ["BlogsRead"] = activities.Count(a => a.ActivityType == "Blog"),
                ["TotalTimeMinutes"] = activities.Sum(a => a.TimeSpentSeconds) / 60
            };
        }
    }
}
