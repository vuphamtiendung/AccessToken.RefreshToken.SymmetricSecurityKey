using AccessRefreshToken.Domain.AccessToken;
using AccessRefreshToken.Domain.Common;
using AccessRefreshToken.Domain.RedisCache.Extension;
using AccessRefreshToken.Domain.RedisCache.Services;
using AccessRefreshToken.Domain.RefreshToken;
using AccessRefreshToken.LoggingServices;
using AccessRefreshToken.Model.ListUser;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using NLog;
using NLog.Web;

using StackExchange.Redis.Configuration;

using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.SessionConfiguration();
        builder.Services.AddApiVersioning(option =>
        {
            option.DefaultApiVersion = new ApiVersion(1, 0);
            option.AssumeDefaultVersionWhenUnspecified = true;
            option.ReportApiVersions = true;
        });
        builder.Services.RedisCacheConfiguration(builder.Configuration);
        builder.Services.AddSingleton<IValidateToken, ValidateToken>();
        builder.Services.AddSingleton<IValidateUser, ValidateUser>();
        builder.Services.AddSingleton<IAccessToken, AccessToken>();
        builder.Services.AddSingleton<IRefreshToken, RefreshToken>();
        builder.Services.AddSingleton<ListUser>();
        builder.Services.AddScoped<IRedisCacheServicecs, RedisCacheService>();
        builder.Services.LoggingConfiguration();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(option =>
                            {
                                option.TokenValidationParameters = new TokenValidationParameters()
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = builder.Configuration["Token:Issuer"],
                                    ValidAudience = builder.Configuration["Token:Audience"],
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecretKey"]))
                                };
                            });
        builder.Services.AddAuthorization();
        builder.Services.AddCors(option =>
        {
            option.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://127.0.0.1:5500", "http://127.0.0.1:5500/index.html")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
            });
        });
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        //
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseSession();
        app.UseAuthorization();
        app.UseRouting();
        app.UseCors("AllowFrontend");
        app.MapControllers();
        app.Run();

    }
}