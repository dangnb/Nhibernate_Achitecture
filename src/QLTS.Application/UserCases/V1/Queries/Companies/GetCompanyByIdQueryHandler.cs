using AutoMapper;
using QLTS.Contract;
using QLTS.Contract.Abstractions.Message;
using QLTS.Contract.Abstractions.Shared;
using QLTS.Contract.Services.Company;
using QLTS.Domain.Abstractions;
using QLTS.Domain.Abstractions.Repositories;

namespace QLTS.Application.UserCases.V1.Queries.Company;
public class GetCompanyByIdQueryHandler : IQueryHandler<Query.GetCompanyByIdQuery, Response.CompanyResponse>
{
    private readonly IMapper _mapper;

    public GetCompanyByIdQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task<Result<Response.CompanyResponse>> Handle(Query.GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var companyRepository = IoC.Resolve<ICompanyRepository>();
        var company =  companyRepository.Getbykey(request.id);
        var result = _mapper.Map<Response.CompanyResponse>(company);
        return result;
    }
}
