using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AccessRefreshToken.Domain.RefreshToken
{
    public class RefreshToken : IRefreshToken
    {
        private readonly IConfiguration _configuration; 
        public RefreshToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        string IRefreshToken.GenerateRefreshToken(string userId)
        {
            string jwtKey = _configuration["Token:SecretKey"];
            string jwtIssuer = _configuration["Token:Issuer"];
            string jwtAudience = _configuration["Token:Audience"];
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var SigninCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Iss, jwtIssuer),
                    new Claim(JwtRegisteredClaimNames.Aud, jwtAudience),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = SigninCredential
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}