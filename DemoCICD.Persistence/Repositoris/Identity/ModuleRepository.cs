using QLTS.Domain.Abstractions.Repositories;
using QLTS.Domain.Entities.Identity;
using QLTS.Persistence.Abstractions.Repositoris;

namespace QLTS.Persistence.Repositoris.Identity;

public class ModuleRepository : Repository<Module, string>, IModuleRepository
{
    public ModuleRepository(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
    {
    }

    public IList<Module> GetModules(bool? active)
    {
        var query = Query;
        if (active.HasValue) query = query.Where(p => p.IsActive == active.Value);
        return query.OrderBy(p => p.Code).ToList();
    }
}