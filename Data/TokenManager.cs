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
                Expiry = DateTime.Now.AddSeconds(_tokenExpiry).Ticks,
                Owner = "Normal"
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
    }
}