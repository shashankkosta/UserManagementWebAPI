using System;
using System.Text;

namespace UserManagement.Helpers
{
    public static class Helper
    {
        public static string CalculateSha(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);

            var sha = System.Security.Cryptography.SHA256.Create();
            var hashBytes = sha.ComputeHash(bytes);

            var hashedPassword = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

            // Console.WriteLine(BitConverter.ToString(hashBytes));
            // Console.WriteLine(hashedPassword);
            
            return hashedPassword;
        }

        public static string ComputeHash(string text, string key)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            var keyBytes = Encoding.UTF8.GetBytes(key);

            var hmacsha = new System.Security.Cryptography.HMACSHA256(keyBytes);
            
            var hashBytes = hmacsha.ComputeHash(textBytes);
            
            var hashText = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
            
            return hashText;
        }
    }
}