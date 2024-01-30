using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLTS.Contract.Services.Company;
using QLTS.Persentation.Abtractions;

namespace QLTS.Persentation.Controllers.V1
{
    [ApiVersion(1)]
    public class CompaniesContoller : ApiController
    {
        private readonly ILogger<CompaniesContoller> _logger;
        public CompaniesContoller(ISender sender) : base(sender)
        {
        }

        [HttpGet("{companyId}", Name = "GetByIdCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdCompany(int id)
        {
            var result = await Sender.Send(new Query.GetCompanyByIdQuery(id));
            return Ok(result);
        }
    }
}
