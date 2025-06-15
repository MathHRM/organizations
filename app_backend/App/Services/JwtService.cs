using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using app_backend.App.Models;
using Microsoft.IdentityModel.Tokens;
using app_backend.App.Config;
using Microsoft.Extensions.Options;
using app_backend.App.Enums;

namespace app_backend.App.Services
{
    public class TokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(User user, Role role, int? organizationId = null, string organizationName = "")
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, ((int) role).ToString()),
                    new Claim(JwtClaim.OrganizationId.ToString(), organizationId?.ToString() ?? string.Empty),
                    new Claim(JwtClaim.OrganizationName.ToString(), organizationName)
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.TokenExpirationHours),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
