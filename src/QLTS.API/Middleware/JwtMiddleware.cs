using Microsoft.Extensions.Options;
using QLTS.Application.UserCases.jwt;
using QLTS.Contract;
using QLTS.Domain.Abstractions.Repositories;

namespace QLTS.API.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
    {
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        IUserRepository userRepository = IoC.Resolve<IUserRepository>();
        ClaimJwtResultModel user = jwtUtils.ValidateJwtToken(token);
        if (user != null)
        {
            // attach account to context on successful jwt validation
            context.Items["Account"] = userRepository.GetbyName(user.ComId.ToString(), user.UserName);
        }

        await _next(context);
    }
}
