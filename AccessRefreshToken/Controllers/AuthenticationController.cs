using AccessRefreshToken.Domain.AccessToken;
using AccessRefreshToken.Domain.Common;
using AccessRefreshToken.Domain.RefreshToken;
using AccessRefreshToken.Model.Access;
using AccessRefreshToken.Model.Token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Collections.Generic;
using AccessRefreshToken.Domain.RedisCache.Services;
using AccessRefreshToken.Model.ListUser;
using AccessRefreshToken.LoggingServices;

namespace AccessRefreshToken.Controllers
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IValidateToken _validateToken;
        private readonly IValidateUser _validateUser;
        private readonly IAccessToken _accessToken;
        private readonly IRefreshToken _refreshToken;
        private readonly IConfiguration _configuration;
        private IRedisCacheServicecs _redisCacheServices;
        private readonly ILoggingServices _loggingServices;

        public AuthenticationController(IValidateToken validateToken, IValidateUser validateUser, IAccessToken accessToken, IRefreshToken refreshToken, IConfiguration configuration, IRedisCacheServicecs redisCacheServices, ILoggingServices loggingServices)
        {
            _validateToken = validateToken;
            _validateUser = validateUser;
            _accessToken = accessToken;
            _refreshToken = refreshToken;
            _configuration = configuration;
            _redisCacheServices = redisCacheServices;
            _loggingServices = loggingServices;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
        {
            if (string.IsNullOrEmpty(tokenModel.RefreshToken))
                        return BadRequest(new { message = "Refresh token is invalid!" });

            var principal = _validateToken.ValidateToken(tokenModel?.RefreshToken);
            if (principal != null) 
                        return BadRequest(new { message = "Refresh Token is still valid, no need to refresh" });
            
            RefreshTokenModel ? storeRefreshToken = _redisCacheServices.GetCacheData<RefreshTokenModel>(tokenModel.RefreshToken);

            if (storeRefreshToken == null) return Unauthorized("Ivalid Refresh Token");
            if (storeRefreshToken.Expiration < DateTime.UtcNow) return Unauthorized("RefreshToken had expired");
            if (storeRefreshToken.isRevoke) return Unauthorized("Refresh token had revoked");

            string? id = storeRefreshToken.UserId;
            string? newAccessToken = _accessToken.GenerateAccessToken(id);
            string? newRefreshToken = _refreshToken.GenerateRefreshToken(id);

            // Revoke old refresh token
            storeRefreshToken.isRevoke = true;
            _redisCacheServices.SetCacheData(tokenModel.RefreshToken, storeRefreshToken, TimeSpan.FromMinutes(30));

            // Create new refresh token
            var refreshTokenModel = new RefreshTokenModel
            {
                Token = newRefreshToken,
                UserId = id,
                Expiration = DateTime.UtcNow.AddDays(5),
                isRevoke = false
            };
            _redisCacheServices.SetCacheData(newRefreshToken, refreshTokenModel, TimeSpan.FromMinutes(30));
            _loggingServices.LogInfor("Refresh token had been create success");
            return base.Ok(new TokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = _validateUser.ValidateUser(loginModel.UserName, loginModel.Password);
            if (user == null) return Unauthorized("Invalid username or password");
            if(loginModel.UserName == user.UserName && loginModel.Password == user.Password)
            {
                string accessToken = _accessToken.GenerateAccessToken(loginModel?.UserId);
                string refreshToken = _refreshToken.GenerateRefreshToken(loginModel?.UserId);

                var refreshTokenModel = new RefreshTokenModel()
                {
                    Token = refreshToken,
                    UserId = user?.Id?.ToString(),
                    Expiration = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["Token:RefreshTokenExpirationDays"])),
                    isRevoke = false
                };

                _redisCacheServices.SetCacheData(refreshToken, refreshTokenModel, TimeSpan.FromMinutes(30));

                _loggingServices.LogInfor("User had been login success");

                return base.Ok(new Model.Access.TokenModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                });
            }
            else
            {
                return Unauthorized();
                _loggingServices.LogError("User had been loging failed");
            }
        }
    }
}
