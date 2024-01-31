namespace QLTS.Contract.Jwt;

public interface IJwtUtils
{
    public string GenerateJwtToken(string user);
    public int? ValidateJwtToken(string? token);
}
