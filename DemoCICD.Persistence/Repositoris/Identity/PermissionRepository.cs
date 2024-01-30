using QLTS.Domain.Abstractions.Repositories;
using QLTS.Domain.Entities.Identity;
using QLTS.Persistence.Abstractions.Repositoris;

namespace QLTS.Persistence.Repositoris.Identity;

public class PermissionRepository : Repository<Permission, int>, IPermissionRepository
{
    public PermissionRepository(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
    {
    }
}
