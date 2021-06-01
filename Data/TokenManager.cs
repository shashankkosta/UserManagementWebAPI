using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UserManagement.Helpers;
using UserManagement.Models;

namespace UserManagement.Data
{
    public class TokenManager : ITokenManager
    {
        private readonly string _secretKey;
        private readonly long _tokenExpiry;

        public TokenManager(IConfiguration config)
        {
            _secretKey = config["SecretKey"];
            _tokenExpiry = Convert.ToInt64(config["TokenExpiry"]);
        }

        public string GenerateToken(int id)
        {
            // Token First Part - Start

            CustomToken customToken = new CustomToken
            {
                Id = id,
                Expiry = DateTime.Now.AddSeconds(_tokenExpiry).Ticks
            };

            string tokenString = JsonConvert.SerializeObject(customToken);

            // CustomToken custToken = JsonConvert.DeserializeObject<CustomToken>(tokenString);

            System.Console.WriteLine(tokenString);

            var tokenBytes = Encoding.UTF8.GetBytes(tokenString);
            var tokenHash = Convert.ToBase64String(tokenBytes);

            // Token First Part - Complete

            // Token Second Part - Start
            var hash = Helper.ComputeHash(tokenHash, _secretKey);

            return tokenHash + "|" + hash;
        }

        public int ValidateToken(string token)
        {
            try
            {
                var tokenParameter = token.Split("|");
                if (tokenParameter.Length != 2)
                {
                    Console.WriteLine("Invalid Token");
                    return -2;
                }

                var tokenBytes = Convert.FromBase64String(tokenParameter[0]);
                var tokenString = Encoding.UTF8.GetString(tokenBytes);

                CustomToken customToken = JsonConvert.DeserializeObject<CustomToken>(tokenString);

                var tokenExpiry = new DateTime(customToken.Expiry);
                if (DateTime.Compare(DateTime.Now, tokenExpiry) > 0)
                {
                    Console.WriteLine("Invalid Token - Token is expired");
                    return -3;
                }

                var tokenHash = Helper.ComputeHash(tokenParameter[0], _secretKey);
                if (tokenHash != tokenParameter[1])
                {
                    Console.WriteLine("Invalid Token - Hash Mismatch");
                    return -4;
                }

                return customToken.Id;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Invalid Token - " + ex.Message);
                return -1;
            }
        }
    }
}