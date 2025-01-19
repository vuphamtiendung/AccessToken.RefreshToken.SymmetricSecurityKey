using AccessRefreshToken.Model.Access;
using AccessRefreshToken.Model.Token;

namespace AccessRefreshToken.Domain.RedisCache.Services
{
    public interface IRedisCacheServicecs
    {
        public T GetCacheData<T>(string refreshToken);
        public void SetCacheData(string tokenModel, RefreshTokenModel refreshTokenModel, TimeSpan cacheDuration);
        public void RemoveCache(TokenModel tokenModel);
    }
}
