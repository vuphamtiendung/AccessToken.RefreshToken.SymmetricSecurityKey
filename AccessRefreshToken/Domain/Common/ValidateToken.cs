using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AccessRefreshToken.Domain.Common
{
    public class ValidateToken : IValidateToken
    {
        private readonly IConfiguration _configuration;
        public ValidateToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        string IValidateToken.ValidateToken(string token)
        {
            string jwtKey = _configuration["Token:SecretKey"];
            string jwtIssuer = _configuration["Token:Issuer"];
            string jwtAudience = _configuration["Token:Audience"];

            var validateToken = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var principal = jwtSecurityTokenHandler.ValidateToken(token, validateToken, out _);
            return principal?.Identity?.Name;
        }
    }
}
