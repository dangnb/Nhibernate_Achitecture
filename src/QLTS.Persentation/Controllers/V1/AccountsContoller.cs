using Asp.Versioning;
using MediatR;
using Microsoft.Extensions.Logging;
using QLTS.Persentation.Abtractions;

namespace QLTS.Persentation.Controllers.V1
{
    [ApiVersion(1)]
    public class AccountsContoller : ApiController
    {
        private readonly ILogger<CompaniesContoller> _logger;

        public AccountsContoller(ISender sender) : base(sender)
        {
        }
    }
}
