using QLTS.Contract.Abstractions.Message;
using static QLTS.Contract.Services.Company.Response;

namespace QLTS.Contract.Services.Company;
public static class Query
{
    //public record GetProductsQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, IDictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<ProductResponse>>;
    public record GetCompanyByIdQuery(int id) : IQuery<CompanyResponse>;
}
