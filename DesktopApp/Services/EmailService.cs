using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using ChildSafeSexEducation.Desktop.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net.Http;
using SendGrid;
using SendGrid.Helpers.Mail;
using Renci.SshNet;

namespace ChildSafeSexEducation.Desktop.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly string _sendGridApiKey;
        private readonly bool _useSendGrid;
        private readonly string _sftpServer;
        private readonly int _sftpPort;
        private readonly string _sftpUsername;
        private readonly string _sftpPassword;
        private readonly string _sftpPath;
        private readonly bool _useSftp;

        public EmailService()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // SMTP settings (fallback)
            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? "your-email@gmail.com";
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? "your-app-password";
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "your-email@gmail.com";
            var fromName = _configuration["EmailSettings:FromName"] ?? "Little Star";

            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _fromEmail = fromEmail;
            _fromName = fromName;

            // SendGrid settings (primary)
            _sendGridApiKey = _configuration["EmailSettings:SendGridApiKey"] ?? "";
            _useSendGrid = _configuration["EmailSettings:UseSendGrid"] == "true" && !string.IsNullOrEmpty(_sendGridApiKey);

            // SFTP settings (alternative delivery)
            var sftpServer = _configuration["EmailSettings:SftpServer"] ?? "";
            var sftpPort = int.Parse(_configuration["EmailSettings:SftpPort"] ?? "22");
            var sftpUsername = _configuration["EmailSettings:SftpUsername"] ?? "";
            var sftpPassword = _configuration["EmailSettings:SftpPassword"] ?? "";
            var sftpPath = _configuration["EmailSettings:SftpPath"] ?? "/email-queue/";

            _sftpServer = sftpServer;
            _sftpPort = sftpPort;
            _sftpUsername = sftpUsername;
            _sftpPassword = sftpPassword;
            _sftpPath = sftpPath;
            _useSftp = _configuration["EmailSettings:UseSftp"] == "true" && !string.IsNullOrEmpty(_sftpServer);

            // Log configuration for debugging
            Console.WriteLine("=== EMAIL CONFIGURATION ===");
            Console.WriteLine($"From Email: {_fromEmail}");
            Console.WriteLine($"From Name: {_fromName}");
            Console.WriteLine($"Use SendGrid: {_useSendGrid}");
            Console.WriteLine($"SendGrid API Key: {(string.IsNullOrEmpty(_sendGridApiKey) ? "NOT SET" : "SET")}");
            Console.WriteLine($"Use SFTP: {_useSftp}");
            Console.WriteLine($"SFTP Server: {_sftpServer}");
            Console.WriteLine($"Test Mode: {_configuration["EmailSettings:TestMode"]}");
            Console.WriteLine("==========================");
        }

        public async Task<bool> SendDailyLogAsync(DailyLogSummary dailyLog)
        {
            // Check if test mode is enabled
            var testMode = _configuration["EmailSettings:TestMode"] == "true";
            if (testMode)
            {
                Console.WriteLine("=== EMAIL TEST MODE ===");
                Console.WriteLine($"Would send email to: {dailyLog.ParentEmail}");
                Console.WriteLine($"Subject: Daily Learning Report for {dailyLog.ChildName} - {dailyLog.Date:MMM dd, yyyy}");
                Console.WriteLine($"Activities: {dailyLog.Activities.Count} activities logged");
                Console.WriteLine($"Total time: {dailyLog.TotalTimeSpentMinutes} minutes");
                Console.WriteLine("=== END TEST MODE ===");
                return true; // Simulate success
            }

            Console.WriteLine($"Sending daily report via email...");
            
            // Try SendGrid first (if configured)
            if (_useSendGrid)
            {
                try
                {
                    return await SendEmailViaSendGrid(dailyLog);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ SendGrid failed: {ex.Message}");
                    Console.WriteLine("🔄 Falling back to SMTP...");
                }
            }

            // Fallback to SMTP
            try
            {
                return await SendEmailViaSMTP(dailyLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ SMTP also failed: {ex.Message}");
                Console.WriteLine("🔄 Falling back to SFTP...");
            }

            // Try SFTP (if configured)
            if (_useSftp)
            {
                try
                {
                    return await SendEmailViaSFTP(dailyLog);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ SFTP also failed: {ex.Message}");
                    Console.WriteLine("💾 Falling back to file generation...");
                }
            }

            // Final fallback to file generation
            return await SaveEmailToFile(dailyLog);
        }

        private async Task<bool> SendEmailViaSendGrid(DailyLogSummary dailyLog)
        {
            try
            {
                Console.WriteLine("📧 Sending email via SendGrid...");
                
                var client = new SendGridClient(_sendGridApiKey);
                var from = new EmailAddress(_fromEmail, _fromName);
                var to = new EmailAddress(dailyLog.ParentEmail);
                var subject = GetEmailSubject(dailyLog);
                var htmlContent = GenerateEmailBody(dailyLog);
                var plainTextContent = GeneratePlainTextBody(dailyLog);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                
                // Add custom headers
                msg.AddHeader("X-Mailer", "Little Star v1.0");
                msg.AddHeader("X-Priority", "3");

                var response = await client.SendEmailAsync(msg);
                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ SendGrid email sent successfully to {dailyLog.ParentEmail}");
                    return true;
                }
                else
                {
                    var errorBody = await response.Body.ReadAsStringAsync();
                    throw new Exception($"SendGrid API returned status {response.StatusCode}: {errorBody}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ SendGrid error: {ex.Message}");
                throw; // Re-throw to be caught by the main SendDailyLogAsync handler
            }
        }

        private async Task<bool> SendEmailViaSMTP(DailyLogSummary dailyLog)
        {
            try
            {
                Console.WriteLine("📧 Sending email via SMTP...");
                Console.WriteLine($"📧 SMTP Server: {_smtpServer}:{_smtpPort}");
                Console.WriteLine($"📧 From: {_fromEmail}");
                Console.WriteLine($"📧 To: {dailyLog.ParentEmail}");
                
                using var client = new SmtpClient(_smtpServer, _smtpPort);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.Timeout = 30000;
                
                // Additional Gmail-specific settings
                client.TargetName = "STARTTLS/smtp.gmail.com";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = GetEmailSubject(dailyLog),
                    Body = GenerateEmailBody(dailyLog),
                    IsBodyHtml = true,
                    Priority = MailPriority.Normal
                };

                mailMessage.To.Add(dailyLog.ParentEmail);
                
                // Add headers for better deliverability
                mailMessage.Headers.Add("X-Mailer", "Little Star v1.0");
                mailMessage.Headers.Add("X-Priority", "3");

                await client.SendMailAsync(mailMessage);
                Console.WriteLine($"✅ SMTP email sent successfully to {dailyLog.ParentEmail}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ SMTP error: {ex.Message}");
                Console.WriteLine($"❌ SMTP Stack Trace: {ex.StackTrace}");
                throw; // Re-throw to be caught by the main SendDailyLogAsync handler
            }
        }

        private async Task<bool> SendEmailViaSFTP(DailyLogSummary dailyLog)
        {
            try
            {
                Console.WriteLine("📁 Uploading email via SFTP...");
                
                using var client = new SftpClient(_sftpServer, _sftpPort, _sftpUsername, _sftpPassword);
                await Task.Run(() => client.Connect());
                
                if (!client.IsConnected)
                {
                    throw new Exception("Failed to connect to SFTP server");
                }

                // Create email content as JSON for processing by server
                var emailData = new
                {
                    To = dailyLog.ParentEmail,
                    From = _fromEmail,
                    FromName = _fromName,
                    Subject = GetEmailSubject(dailyLog),
                    HtmlBody = GenerateEmailBody(dailyLog),
                    PlainTextBody = GeneratePlainTextBody(dailyLog),
                    ChildName = dailyLog.ChildName,
                    ChildAge = dailyLog.ChildAge,
                    Date = dailyLog.Date,
                    Language = dailyLog.Language,
                    Activities = dailyLog.Activities,
                    TopicsViewed = dailyLog.TopicsViewed,
                    QuestionsAsked = dailyLog.QuestionsAsked,
                    BlogsRead = dailyLog.BlogsRead,
                    TotalTimeSpentMinutes = dailyLog.TotalTimeSpentMinutes,
                    Timestamp = DateTime.Now
                };

                var jsonContent = System.Text.Json.JsonSerializer.Serialize(emailData, new System.Text.Json.JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });

                // Create filename with timestamp
                var fileName = $"email_{dailyLog.ChildName}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                var remotePath = _sftpPath.TrimEnd('/') + "/" + fileName;

                // Upload file to SFTP server
                using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonContent));
                await Task.Run(() => client.UploadFile(stream, remotePath));

                Console.WriteLine($"✅ Email data uploaded successfully to SFTP: {remotePath}");
                Console.WriteLine($"📧 Server should process and send email to: {dailyLog.ParentEmail}");
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ SFTP error: {ex.Message}");
                throw; // Re-throw to be caught by the main SendDailyLogAsync handler
            }
        }

        public async Task<(bool success, string errorMessage)> SendTestEmailAsync(string toEmail)
        {
            try
            {
                // Check if test mode is enabled
                var testMode = _configuration["EmailSettings:TestMode"] == "true";
                if (testMode)
                {
                    return (true, $"TEST MODE: Would send email to {toEmail}\nSendGrid: {_useSendGrid}\nSMTP: {_smtpServer}:{_smtpPort}\nUser: {_smtpUsername}\nPassword Length: {_smtpPassword?.Length ?? 0}");
                }

                Console.WriteLine($"Sending test email to {toEmail}...");
                
                // Try SendGrid first (if configured)
                if (_useSendGrid)
                {
                    try
                    {
                        return await SendTestEmailViaSendGrid(toEmail);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ SendGrid test email failed: {ex.Message}");
                        Console.WriteLine("🔄 Falling back to SMTP...");
                    }
                }

                // Fallback to SMTP
                try
                {
                    return await SendTestEmailViaSMTP(toEmail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ SMTP test email also failed: {ex.Message}");
                    Console.WriteLine("🔄 Falling back to SFTP...");
                }

                // Try SFTP (if configured)
                if (_useSftp)
                {
                    try
                    {
                        return await SendTestEmailViaSFTP(toEmail);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ SFTP test email also failed: {ex.Message}");
                        return await SaveTestEmailToFile(toEmail, $"All methods failed. SendGrid, SMTP, and SFTP all failed.");
                    }
                }

                // Final fallback to file
                return await SaveTestEmailToFile(toEmail, $"SendGrid and SMTP failed. SFTP not configured.");
            }
            catch (Exception ex)
            {
                var errorMsg = $"❌ Test email failed completely: {ex.Message}\n❌ Type: {ex.GetType().Name}";
                Console.WriteLine(errorMsg);
                return await SaveTestEmailToFile(toEmail, errorMsg);
            }
        }

        private async Task<(bool success, string errorMessage)> SendTestEmailViaSendGrid(string toEmail)
        {
            try
            {
                Console.WriteLine("📧 Sending test email via SendGrid...");
                
                var client = new SendGridClient(_sendGridApiKey);
                var from = new EmailAddress(_fromEmail, _fromName);
                var to = new EmailAddress(toEmail);
                var subject = "Test Email from Little Star";
                var htmlContent = $@"
                <h1>🧪 Test Email</h1>
                <p>This is a test email from Little Star using SendGrid.</p>
                <p>If you receive this, the SendGrid email system is working correctly!</p>
                <p><strong>Sent to:</strong> {toEmail}</p>
                <p><strong>Time:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
                <p><strong>From:</strong> {_fromName} &lt;{_fromEmail}&gt;</p>
                <p>This email was sent using SendGrid API.</p>";
                
                var plainTextContent = $"Test Email from Little Star\n\nSent to: {toEmail}\nTime: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\nFrom: {_fromName} <{_fromEmail}>\n\nThis email was sent using SendGrid API.";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                msg.AddHeader("X-Mailer", "Little Star v1.0");

                var response = await client.SendEmailAsync(msg);
                
                if (response.IsSuccessStatusCode)
                {
                    return (true, $"✅ Test email sent successfully via SendGrid to {toEmail}\n\n📧 From: {_fromName} <{_fromEmail}>\n📧 To: {toEmail}\n📧 Method: SendGrid API\n\nCheck your inbox (and spam folder) for the test email!");
                }
                else
                {
                    var errorBody = await response.Body.ReadAsStringAsync();
                    throw new Exception($"SendGrid API returned status {response.StatusCode}: {errorBody}");
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"❌ SendGrid test email failed: {ex.Message}";
                Console.WriteLine(errorMsg);
                throw; // Re-throw to be caught by the main SendTestEmailAsync handler
            }
        }

        private async Task<(bool success, string errorMessage)> SendTestEmailViaSMTP(string toEmail)
        {
            try
            {
                Console.WriteLine("📧 Sending test email via SMTP...");
                Console.WriteLine($"📧 SMTP Server: {_smtpServer}:{_smtpPort}");
                Console.WriteLine($"📧 From: {_fromEmail}");
                Console.WriteLine($"📧 To: {toEmail}");
                
                using var client = new SmtpClient(_smtpServer, _smtpPort);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.Timeout = 30000;
                
                // Additional Gmail-specific settings
                client.TargetName = "STARTTLS/smtp.gmail.com";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = "Test Email from Little Star",
                    Body = $@"
                    <h1>🧪 Test Email</h1>
                    <p>This is a test email from Little Star using SMTP.</p>
                    <p>If you receive this, the SMTP email system is working correctly!</p>
                    <p><strong>Sent to:</strong> {toEmail}</p>
                    <p><strong>Time:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
                    <p><strong>From:</strong> {_fromName} &lt;{_fromEmail}&gt;</p>
                    <p>This email was sent using SMTP configuration.</p>",
                    IsBodyHtml = true,
                    Priority = MailPriority.Normal
                };
                mailMessage.To.Add(toEmail);
                
                // Add headers for better deliverability
                mailMessage.Headers.Add("X-Mailer", "Little Star v1.0");
                mailMessage.Headers.Add("X-Priority", "3");

                await client.SendMailAsync(mailMessage);
                return (true, $"✅ Test email sent successfully via SMTP to {toEmail}\n\n📧 From: {_fromName} <{_fromEmail}>\n📧 To: {toEmail}\n📧 Method: SMTP\n\nCheck your inbox (and spam folder) for the test email!");
            }
            catch (Exception ex)
            {
                var errorMsg = $"❌ SMTP test email failed: {ex.Message}";
                Console.WriteLine(errorMsg);
                Console.WriteLine($"❌ SMTP Stack Trace: {ex.StackTrace}");
                throw; // Re-throw to be caught by the main SendTestEmailAsync handler
            }
        }

        private async Task<(bool success, string errorMessage)> SendTestEmailViaSFTP(string toEmail)
        {
            try
            {
                Console.WriteLine("📁 Uploading test email via SFTP...");
                
                using var client = new SftpClient(_sftpServer, _sftpPort, _sftpUsername, _sftpPassword);
                await Task.Run(() => client.Connect());
                
                if (!client.IsConnected)
                {
                    throw new Exception("Failed to connect to SFTP server");
                }

                // Create test email data as JSON
                var testEmailData = new
                {
                    To = toEmail,
                    From = _fromEmail,
                    FromName = _fromName,
                    Subject = "Test Email from Little Star",
                    HtmlBody = $@"
                    <h1>🧪 Test Email</h1>
                    <p>This is a test email from Little Star using SFTP.</p>
                    <p>If you receive this, the SFTP email system is working correctly!</p>
                    <p><strong>Sent to:</strong> {toEmail}</p>
                    <p><strong>Time:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
                    <p><strong>From:</strong> {_fromName} &lt;{_fromEmail}&gt;</p>
                    <p>This email was sent using SFTP upload method.</p>",
                    PlainTextBody = $"Test Email from Little Star\n\nSent to: {toEmail}\nTime: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\nFrom: {_fromName} <{_fromEmail}>\n\nThis email was sent using SFTP upload method.",
                    IsTestEmail = true,
                    Timestamp = DateTime.Now
                };

                var jsonContent = System.Text.Json.JsonSerializer.Serialize(testEmailData, new System.Text.Json.JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });

                // Create filename with timestamp
                var fileName = $"test_email_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                var remotePath = _sftpPath.TrimEnd('/') + "/" + fileName;

                // Upload file to SFTP server
                using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonContent));
                await Task.Run(() => client.UploadFile(stream, remotePath));

                return (true, $"✅ Test email data uploaded successfully via SFTP to {remotePath}\n\n📧 From: {_fromName} <{_fromEmail}>\n📧 To: {toEmail}\n📧 Method: SFTP Upload\n\n📁 File uploaded to server. The server should process and send the email!\n\nCheck your inbox (and spam folder) for the test email!");
            }
            catch (Exception ex)
            {
                var errorMsg = $"❌ SFTP test email failed: {ex.Message}";
                Console.WriteLine(errorMsg);
                throw; // Re-throw to be caught by the main SendTestEmailAsync handler
            }
        }

        private async Task<(bool success, string errorMessage)> SaveTestEmailToFile(string toEmail, string failureReason)
        {
            try
            {
                Console.WriteLine("💾 Saving test email to file as fallback...");
                
                var testEmailContent = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Test Email - Little Star</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }}
        .header {{ background-color: #FFC107; color: white; padding: 20px; border-radius: 10px; margin-bottom: 20px; }}
        .content {{ background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }}
        .footer {{ margin-top: 30px; font-size: 12px; color: #666; text-align: center; }}
    </style>
</head>
<body>
    <div class=""header"">
        <h1>⚠️ Test Email - Fallback to File</h1>
        <p>Little Star Email System Test</p>
    </div>
    
    <div class=""content"">
        <h2>Email System Status: ❌ Failed to Send via Network</h2>
        <p>This test email could not be sent via network methods (SendGrid or SMTP).</p>
        <p>The content has been saved to a local HTML file for review.</p>
        
        <h3>Failure Details:</h3>
        <pre>{failureReason}</pre>
        
        <h3>Configuration Details:</h3>
        <ul>
            <li><strong>To:</strong> {toEmail}</li>
            <li><strong>From:</strong> {_fromName} &lt;{_fromEmail}&gt;</li>
            <li><strong>Test Time:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</li>
            <li><strong>Method:</strong> File Fallback</li>
            <li><strong>SendGrid Enabled:</strong> {_useSendGrid}</li>
            <li><strong>SMTP Server:</strong> {_smtpServer}:{_smtpPort}</li>
        </ul>
    </div>
    
    <div class=""footer"">
        <p>This test email was generated by Little Star</p>
        <p>📧 Email was saved to file due to network sending issues. Please share this file.</p>
    </div>
</body>
</html>";

                var testEmailFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"test_email_{DateTime.Now:yyyyMMdd_HHmmss}.html");
                await File.WriteAllTextAsync(testEmailFilePath, testEmailContent);
                
                return (false, $"❌ Test email failed to send via network. Content saved to file: {Path.GetFileName(testEmailFilePath)}\n\n📁 File location: {testEmailFilePath}\n📧 To: {toEmail}\n📧 From: {_fromName} <{_fromEmail}>\n\nPlease share this file with the recipient!\n\nFailure Reason: {failureReason}");
            }
            catch (Exception ex)
            {
                return (false, $"❌ Critical error: Test email file save also failed: {ex.Message}\nOriginal Failure: {failureReason}");
            }
        }

        private async Task<bool> SaveEmailToFile(DailyLogSummary dailyLog)
        {
            try
            {
                Console.WriteLine("💾 Saving email to file as fallback...");
                
                var emailContent = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Daily Learning Report - {dailyLog.ChildName}</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }}
        .header {{ background-color: #4CAF50; color: white; padding: 20px; border-radius: 10px; margin-bottom: 20px; }}
        .content {{ background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }}
        .activity {{ background-color: #f9f9f9; padding: 15px; margin: 10px 0; border-radius: 5px; border-left: 4px solid #4CAF50; }}
        .footer {{ margin-top: 30px; font-size: 12px; color: #666; text-align: center; }}
        .summary {{ display: flex; justify-content: space-around; margin: 20px 0; }}
        .summary-item {{ text-align: center; padding: 10px; background-color: #e8f5e8; border-radius: 5px; }}
    </style>
</head>
<body>
    <div class=""header"">
        <h1>📚 Daily Learning Report</h1>
        <p><strong>Child:</strong> {dailyLog.ChildName} (Age: {dailyLog.ChildAge})</p>
        <p><strong>Date:</strong> {dailyLog.Date:MMMM dd, yyyy}</p>
        <p><strong>Language:</strong> {dailyLog.Language}</p>
        <p><strong>Parent Email:</strong> {dailyLog.ParentEmail}</p>
    </div>
    
    <div class=""content"">
        <div class=""summary"">
            <div class=""summary-item"">
                <h3>{dailyLog.TopicsViewed}</h3>
                <p>Topics Viewed</p>
            </div>
            <div class=""summary-item"">
                <h3>{dailyLog.QuestionsAsked}</h3>
                <p>Questions Asked</p>
            </div>
            <div class=""summary-item"">
                <h3>{dailyLog.BlogsRead}</h3>
                <p>Blogs Read</p>
            </div>
            <div class=""summary-item"">
                <h3>{dailyLog.TotalTimeSpentMinutes}</h3>
                <p>Minutes Spent</p>
            </div>
        </div>
        
        <h2>📝 Today's Activities</h2>
        {string.Join("", dailyLog.Activities.Select(a => $@"
        <div class=""activity"">
            <strong>{a.ActivityType}:</strong> {a.ActivityTitle}<br>
            <em>{a.ActivityDescription}</em><br>
            <small>Time: {a.Timestamp:HH:mm} | Language: {a.Language}</small>
        </div>"))}
    </div>
    
    <div class=""footer"">
        <p>This report was generated by Little Star on {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
        <p>📧 Email was saved to file due to API issues. Please share this file with the parent.</p>
        <p>📁 File: daily_report_{dailyLog.ChildName}_{dailyLog.Date:yyyyMMdd}.html</p>
    </div>
</body>
</html>";

                var emailFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"daily_report_{dailyLog.ChildName}_{dailyLog.Date:yyyyMMdd}.html");
                await File.WriteAllTextAsync(emailFilePath, emailContent);
                
                Console.WriteLine($"✅ Email content saved to file: {Path.GetFileName(emailFilePath)}");
                Console.WriteLine($"📁 File location: {emailFilePath}");
                Console.WriteLine($"📧 Please share this file with parent: {dailyLog.ParentEmail}");
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ File save failed: {ex.Message}");
                return false;
            }
        }

        private string GetEmailSubject(DailyLogSummary dailyLog)
        {
            LanguageService.Instance.SetLanguage(dailyLog.Language == "Burmese" ? Language.Burmese : Language.English);
            var subjectTemplate = LanguageService.Instance.GetText("email_subject_daily_report");
            return string.Format(subjectTemplate, dailyLog.ChildName, dailyLog.Date.ToShortDateString());
        }

        private string GenerateEmailBody(DailyLogSummary dailyLog)
        {
            LanguageService.Instance.SetLanguage(dailyLog.Language == "Burmese" ? Language.Burmese : Language.English);
            return dailyLog.Language == "Burmese" ? GenerateBurmeseEmailBody(dailyLog) : GenerateEnglishEmailBody(dailyLog);
        }

        private string GeneratePlainTextBody(DailyLogSummary dailyLog)
        {
            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine($"Daily Learning Report for {dailyLog.ChildName}");
            bodyBuilder.AppendLine($"Date: {dailyLog.Date.ToShortDateString()}");
            bodyBuilder.AppendLine($"Total time spent: {dailyLog.TotalTimeSpentMinutes} minutes");
            bodyBuilder.AppendLine("Activities:");
            foreach (var activity in dailyLog.Activities)
            {
                bodyBuilder.AppendLine($"- {activity.ActivityType}: {activity.ActivityTitle} - {activity.ActivityDescription} ({activity.Timestamp:HH:mm})");
            }
            return bodyBuilder.ToString();
        }

        private string GenerateEnglishEmailBody(DailyLogSummary dailyLog)
        {
            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append("<html><body>");
            bodyBuilder.Append($"<h1>Daily Learning Report for {dailyLog.ChildName}</h1>");
            bodyBuilder.Append($"<p>Date: {dailyLog.Date.ToShortDateString()}</p>");
            bodyBuilder.Append($"<p>Total time spent: {dailyLog.TotalTimeSpentMinutes} minutes</p>");
            bodyBuilder.Append("<h2>Activities:</h2><ul>");
            foreach (var activity in dailyLog.Activities)
            {
                bodyBuilder.Append($"<li><strong>{activity.ActivityType}:</strong> {activity.ActivityTitle} - {activity.ActivityDescription} ({activity.Timestamp:HH:mm})</li>");
            }
            bodyBuilder.Append("</ul></body></html>");
            return bodyBuilder.ToString();
        }

        private string GenerateBurmeseEmailBody(DailyLogSummary dailyLog)
        {
            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append("<html><body>");
            bodyBuilder.Append($"<h1>{LanguageService.Instance.GetText("email_header_title")} {dailyLog.ChildName} အတွက် နေ့စဉ် သင်ယူမှု အစီရင်ခံစာ</h1>");
            bodyBuilder.Append($"<p>{LanguageService.Instance.GetText("email_date")}: {dailyLog.Date.ToShortDateString()}</p>");
            bodyBuilder.Append($"<p>{LanguageService.Instance.GetText("email_total_time_spent")}: {dailyLog.TotalTimeSpentMinutes} {LanguageService.Instance.GetText("email_minutes")}</p>");
            bodyBuilder.Append($"<h2>{LanguageService.Instance.GetText("email_activities_title")}:</h2><ul>");
            foreach (var activity in dailyLog.Activities)
            {
                bodyBuilder.Append($"<li><strong>{LanguageService.Instance.GetText(activity.ActivityType)}:</strong> {LanguageService.Instance.GetText(activity.ActivityTitle)} - {LanguageService.Instance.GetText(activity.ActivityDescription)} ({activity.Timestamp:HH:mm})</li>");
            }
            bodyBuilder.Append("</ul></body></html>");
            return bodyBuilder.ToString();
        }
    }
}