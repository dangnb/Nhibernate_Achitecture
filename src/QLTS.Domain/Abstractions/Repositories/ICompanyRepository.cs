using QLTS.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Domain.Abstractions.Repositories
{
    public interface ICompanyRepository : IRepository<Company, int>
    {
        Company GetbyCode(string code);
        Company GetbyToken(string token);

        Company GetByDomain(string sitedomain);
    }
}
