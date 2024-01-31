namespace QLTS.Application.UserCases.jwt;

public interface IJwtUtils
{
    public string GenerateJwtToken(string user, int comId);
    public ClaimJwtResultModel ValidateJwtToken(string? token);
}
