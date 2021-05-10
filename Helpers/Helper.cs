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
    }
}