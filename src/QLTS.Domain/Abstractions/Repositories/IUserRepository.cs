using QLTS.Domain.Entities.Identity;

namespace QLTS.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<Userdata, int>
{
    Userdata Authenticate(string groupcode, string username, string password);

    Userdata GetbyName(string groupcode, string username);

    bool ChangePassword(Userdata userdata, string password, string newpassword, out string message);

    IList<Userdata> GetbyFilter(string groupcode, string keyword, int pageindex, int pagesize, out int total);
}
