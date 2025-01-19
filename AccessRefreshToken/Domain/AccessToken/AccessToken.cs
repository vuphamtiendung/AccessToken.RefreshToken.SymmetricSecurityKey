using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace AccessRefreshToken.Domain.AccessToken
{
    public class AccessToken : IAccessToken
    {
        private readonly IConfiguration _configuration;
        public AccessToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        string IAccessToken.GenerateAccessToken(string userId)
        {
            string jwtKey = _configuration["Token:SecretKey"];
            string jwtIssuer = _configuration["Token:Issuer"];
            string jwtAudience = _configuration["Token:Audience"];
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var SigninCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var claim = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, jwtIssuer),
                new Claim(JwtRegisteredClaimNames.Aud, jwtAudience),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = new JwtSecurityToken(
                    jwtKey,
                    jwtIssuer,
                    claim,
                    expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["Token:AccessTokenExpirationMinutes"])),
                    signingCredentials: SigninCredential
                );
            return jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
        }
    }
}
