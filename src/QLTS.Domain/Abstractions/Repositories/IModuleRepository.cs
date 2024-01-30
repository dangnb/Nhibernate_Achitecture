using QLTS.Domain.Entities.Identity;

namespace QLTS.Domain.Abstractions.Repositories;

public interface IModuleRepository : IRepository<Module, string>
{
    IList<Module> GetModules(bool? active);
}
