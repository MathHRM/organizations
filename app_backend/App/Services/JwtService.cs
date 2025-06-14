using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using app_backend.App.Models;
using Microsoft.IdentityModel.Tokens;

namespace app_backend.App.Services
{
    public class TokenService
    {
        private const string Issuer = "your_issuer";
        private const string Audience = "your_audience";
        private string _key;

        public TokenService(IConfiguration environment)
        {
            _key = environment["JwtSettings:Secret"];
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = Issuer,
                Audience = Audience,
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
