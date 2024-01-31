using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QLTS.Contract;

namespace QLTS.API.Middleware;

public static class CustomJwtAuthExtension
{
    public static void AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)

    {
        var appSetting = configuration.GetSection("AppSettings").Get<AppSettings>();
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(appSetting?.Secret)),
                RequireExpirationTime = true,
                ValidateLifetime = true,
            };
        });
    }
}
