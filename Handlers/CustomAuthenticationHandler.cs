using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Handlers
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserAuthentication _userAuth;
        private readonly ITokenManager _tokenManager;

        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserAuthentication userAuth,
            ITokenManager tokenManager) : base(options, logger, encoder, clock)
        {
            _userAuth = userAuth;
            _tokenManager = tokenManager;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            try
            {
                System.Console.WriteLine("CUSTOM AUTHENTICATION");

                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (authHeader.Scheme.ToUpper() != "BEARER")
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));

                // User user = _userAuth.ValidateToken(authHeader.Parameter);
                // if (user == null)
                //     return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));

                // var claims = new[] {
                //     new Claim(ClaimTypes.NameIdentifier, user.n_UserID.ToString()),
                //     new Claim(ClaimTypes.Name, user.s_UserCode)
                // };

                int nUserId = _tokenManager.ValidateToken(authHeader.Parameter);
                if (nUserId <= 0)
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));

                var claims = new [] {
                    new Claim(ClaimTypes.NameIdentifier, nUserId.ToString())
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