using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserAuthentication _userAuth;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserAuthentication userAuth) : base(options, logger, encoder, clock)
        {
            _userAuth = userAuth;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            try
            {
                System.Console.WriteLine("BASIC AUTHENTICATION");

                // Authorization Header is Base64 String
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                // Getting Auth Header value in Bytes
                var authHeaderBytes = Convert.FromBase64String(authHeader.Parameter);

                // Converting Auth Header in Bytes to String
                var credentials = Encoding.UTF8.GetString(authHeaderBytes);

                var credentialsArray = credentials.Split(':');

                if (credentialsArray.Length != 2)
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));

                var userCode = credentialsArray[0];
                var password = credentialsArray[1];

                User user = _userAuth.AuthenticateUser(new UserLogin
                {
                    s_UserCode = userCode,
                    s_Password = password
                });

                if (user == null)
                    return Task.FromResult(AuthenticateResult.Fail("Invalid User Code or Password"));

                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.n_UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.s_UserCode)
                    // new Claim(ClaimTypes.Role, user.n_Role == "1" ? "Normal" : "Admin")),
                };
                var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(claimsIdentity);

                var authTicket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(authTicket));
            }
            catch (System.Exception)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }
    }
}