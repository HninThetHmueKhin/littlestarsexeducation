using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace ChildSafeSexEducation.Desktop.Services
{
    public class PasswordEncryptionService
    {
        private static readonly byte[] Salt = new byte[] 
        { 
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 
            0x76, 0x65, 0x64, 0x65, 0x76, 0x20, 0x4b, 0x65, 
            0x79, 0x20, 0x41, 0x45, 0x53, 0x20, 0x53, 0x61, 
            0x6c, 0x74, 0x20, 0x56, 0x61, 0x6c, 0x75, 0x65 
        };
        
        private const string MasterKey = "ChildSafeSexEducation2024!@#SecureKey";
        private const int Iterations = 10000;

        /// <summary>
        /// Encrypts a password using AES encryption
        /// </summary>
        /// <param name="password">The plain text password to encrypt</param>
        /// <returns>Base64 encoded encrypted password</returns>
        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            try
            {
                using (var aes = Aes.Create())
                {
                    // Generate a random IV for each encryption
                    aes.GenerateIV();
                    
                    // Derive key from master key and salt
                    var key = new Rfc2898DeriveBytes(MasterKey, Salt, Iterations, HashAlgorithmName.SHA256).GetBytes(32);
                    aes.Key = key;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var encryptor = aes.CreateEncryptor())
                    using (var msEncrypt = new MemoryStream())
                    {
                        // Write IV to the beginning of the stream
                        msEncrypt.Write(aes.IV, 0, aes.IV.Length);
                        
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(password);
                        }
                        
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encrypting password: {ex.Message}");
                throw new InvalidOperationException("Failed to encrypt password", ex);
            }
        }

        /// <summary>
        /// Decrypts a password using AES decryption
        /// </summary>
        /// <param name="encryptedPassword">The base64 encoded encrypted password</param>
        /// <returns>Decrypted plain text password</returns>
        public static string DecryptPassword(string encryptedPassword)
        {
            if (string.IsNullOrEmpty(encryptedPassword))
                return string.Empty;

            try
            {
                var cipherText = Convert.FromBase64String(encryptedPassword);
                
                using (var aes = Aes.Create())
                {
                    // Extract IV from the beginning of the cipher text
                    var iv = new byte[16];
                    Array.Copy(cipherText, 0, iv, 0, 16);
                    
                    // Extract the actual encrypted data
                    var encryptedData = new byte[cipherText.Length - 16];
                    Array.Copy(cipherText, 16, encryptedData, 0, encryptedData.Length);
                    
                    // Derive key from master key and salt
                    var key = new Rfc2898DeriveBytes(MasterKey, Salt, Iterations, HashAlgorithmName.SHA256).GetBytes(32);
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var decryptor = aes.CreateDecryptor())
                    using (var msDecrypt = new MemoryStream(encryptedData))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decrypting password: {ex.Message}");
                throw new InvalidOperationException("Failed to decrypt password", ex);
            }
        }

        /// <summary>
        /// Verifies if a plain text password matches an encrypted password
        /// </summary>
        /// <param name="plainPassword">The plain text password to verify</param>
        /// <param name="encryptedPassword">The encrypted password to compare against</param>
        /// <returns>True if passwords match, false otherwise</returns>
        public static bool VerifyPassword(string plainPassword, string encryptedPassword)
        {
            try
            {
                var decryptedPassword = DecryptPassword(encryptedPassword);
                return string.Equals(plainPassword, decryptedPassword, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a password is already encrypted (contains base64 characters and is longer than typical plain text)
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <returns>True if the password appears to be encrypted</returns>
        public static bool IsEncrypted(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 20)
                return false;

            try
            {
                // Try to decode as base64 - if it works and is long enough, it's likely encrypted
                var decoded = Convert.FromBase64String(password);
                return decoded.Length > 16; // Encrypted passwords with IV should be longer than 16 bytes
            }
            catch
            {
                return false;
            }
        }
    }
}
