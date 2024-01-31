namespace QLTS.Contract.Services.Company;

public static class Response
{
    public record CompanyResponse(Guid Id, string Name, decimal Price, string Address, string Phone);
}
