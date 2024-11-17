using System.Security.Cryptography;
using System.Text;

namespace real_estate_api.Shared
{
    public class Protect
    {
        // This key should be stored in configuration, not hardcoded
        private static readonly string _encryptionKey = "YourSecure32CharacterKeyHere12345!";

        public static string EncryptPassword(string password)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    // First, create a salt by combining the password with the encryption key
                    string saltedPassword = password + _encryptionKey;
                    // Convert the salted password to bytes
                    byte[] bytes = Encoding.UTF8.GetBytes(saltedPassword);
                    // Compute the hash
                    byte[] hash = sha256.ComputeHash(bytes);
                    // Convert to base64 string for storage
                    return Convert.ToBase64String(hash);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error encrypting password: " + ex.Message);
            }
        }

        public static bool VerifyPassword(string inputPassword, string storedPassword)
        {
            try
            {
                // Encrypt the input password using the same method
                string encryptedInput = EncryptPassword(inputPassword);

                // Compare the encrypted input with stored password
                return encryptedInput == storedPassword;
            }
            catch (Exception ex)
            {
                throw new Exception("Error verifying password: " + ex.Message);
            }
        }
    }
}