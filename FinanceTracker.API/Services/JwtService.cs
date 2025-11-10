using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinanceTracker.API.Handlers;
using FinanceTracker.API.Models.API;
using FinanceTracker.Services;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;

namespace FinanceTracker.API.Services
{
    public class JwtService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public JwtService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<LoginResponseModel?> Authenticate(LoginRequestModel loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return null;
            }

            // Normalize email for case-insensitive lookup
            var normalizedEmail = loginRequest.Email.Trim().ToLowerInvariant();
            var user = await _userService.GetUserByEmailAsync(normalizedEmail);

            if (user == null || !PasswordHashHandler.VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                return null;
            }

            // Read from JwtConfig section to match Program.cs and appsettings
            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int?>("JwtConfig:TokenValidityMins") ?? 30;
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            {
                // Misconfigured JWT settings
                return null;
            }
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, normalizedEmail),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Add userId as "sub" claim
                    new Claim("userId", user.Id) // Also add as "userId" for clarity
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseModel
            {
                Email = user.Email,
                Token = accessToken,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }
    }
}
