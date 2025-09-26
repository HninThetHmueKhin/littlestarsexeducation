using ChildSafeSexEducation.Desktop.Services;

namespace ChildSafeSexEducation.Desktop
{
    public class TestEncryption
    {
        public static void TestPasswordEncryption()
        {
            Console.WriteLine("🔐 Testing Password Encryption...");
            
            string testPassword = "test123";
            Console.WriteLine($"Original password: {testPassword}");
            
            string encrypted = PasswordEncryptionService.EncryptPassword(testPassword);
            Console.WriteLine($"Encrypted password: {encrypted}");
            
            string decrypted = PasswordEncryptionService.DecryptPassword(encrypted);
            Console.WriteLine($"Decrypted password: {decrypted}");
            
            bool isMatch = PasswordEncryptionService.VerifyPassword(testPassword, encrypted);
            Console.WriteLine($"Passwords match: {isMatch}");
            
            bool isEncrypted = PasswordEncryptionService.IsEncrypted(encrypted);
            Console.WriteLine($"Is encrypted: {isEncrypted}");
        }
    }
}
