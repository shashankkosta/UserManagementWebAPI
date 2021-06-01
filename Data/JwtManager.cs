using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace UserManagement.Data
{
    public class JwtManager : ITokenManager
    {
        private readonly string _secretKey;
        private readonly long _tokenExpiry;

        public JwtManager(IConfiguration config)
        {
            _secretKey = config["SecretKey"];
            _tokenExpiry = Convert.ToInt64(config["TokenExpiry"]);
        }

        public string GenerateToken(int id)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_secretKey);
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256),
                
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddSeconds(_tokenExpiry),

                // NotBefore = DateTime.UtcNow.AddSeconds(10)
            };

            var securityToken = jwtHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtHandler.WriteToken(securityToken);

            return jwtToken;
        }

        public int ValidateToken(string token)
        {
            throw new System.NotImplementedException();
        }
    }
}