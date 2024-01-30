using QLTS.Domain.Abstractions.Repositories;
using QLTS.Domain.Entities.Identity;
using QLTS.Persistence.Abstractions.Repositoris;

namespace QLTS.Persistence.Repositoris.Identity;

public class CompanyConfigRepository : Repository<CompanyConfig, int>, ICompanyConfigRepository
{
    public CompanyConfigRepository(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
    {
    }
}
