using QLTS.Domain.Abstractions.Repositories;
using QLTS.Domain.Entities.Identity;
using QLTS.Persistence.Abstractions.Repositoris;

namespace QLTS.Persistence.Repositoris.Identity;

public class RoleRepository : Repository<Role, int>, IRoleRepository
{
    public RoleRepository(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
    {
    }

    public Role GetbyCode(int comid, string code)
    {
        return Get(p => p.ComID == comid && p.Code == code);
    }
}
