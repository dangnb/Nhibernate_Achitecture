using QLTS.Domain.Entities.Identity;

namespace QLTS.Domain.Abstractions.Repositories;

public interface IRoleRepository : IRepository<Role, int>
{
    Role GetbyCode(int comid, string code);
}
