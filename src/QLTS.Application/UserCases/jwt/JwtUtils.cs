using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QLTS.Contract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QLTS.Application.UserCases.jwt;

public class JwtUtils : IJwtUtils
{
    private readonly AppSettings _appSettings;

    private const string UserName = nameof(UserName);
    private const string ComId = nameof(ComId);

    public JwtUtils(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;

        if (string.IsNullOrEmpty(_appSettings.Secret))
            throw new Exception("JWT secret not configured");
    }
    public string GenerateJwtToken(string user, int comId)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(UserName, user),
                new Claim(ComId, comId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(_appSettings.TimeOutJwt),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimJwtResultModel ValidateJwtToken(string? token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == UserName).Value;
            var comId = int.Parse(jwtToken.Claims.First(x => x.Type == ComId).Value);

            // return user id from JWT token if validation successful
            return new ClaimJwtResultModel(comId, UserName);
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }
}
