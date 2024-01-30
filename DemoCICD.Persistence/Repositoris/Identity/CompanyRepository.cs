using QLTS.Domain.Abstractions.Repositories;
using QLTS.Domain.Entities.Identity;
using QLTS.Persistence.Abstractions.Repositoris;

namespace QLTS.Persistence.Repositoris.Identity;

public class CompanyRepository : Repository<Company, int>, ICompanyRepository
{
    public CompanyRepository(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
    {
    }

    public Company GetbyCode(string code)
    {
        return Get(p => p.Code == code);
    }

    public Company GetbyToken(string token)
    {
        return Get(p => p.TokenKey == token);
    }

    public Company GetByDomain(string sitedomain)
    {
        string sdomain = string.Format("({0})", sitedomain.Replace("http://", "").Replace("https://", ""));
        return Get(p => p.Domain.Contains(sdomain));
    }
}
