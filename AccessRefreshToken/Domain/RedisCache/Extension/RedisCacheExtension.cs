using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;

namespace AccessRefreshToken.Domain.RedisCache.Extension
{
    public static class RedisCacheExtension
    {
        public static void RedisCacheConfiguration(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = configuration["ConnectionString:RedisConnectionString"];
                option.InstanceName = "AuthenticateCatalog_";
            });
        }

        public static void SessionConfiguration(this IServiceCollection service)
        {
            service.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
                options.Cookie.HttpOnly = true; // Bảo mật cookie
                options.Cookie.IsEssential = true; // Yêu cầu cookie luôn được gửi
            });
        }
    }
}
