namespace QLTS.Contract.Services.Company;

public static class Response
{
    public record CompanyResponse(int ID, string Name, string Code, string Address, string Phone);
}
