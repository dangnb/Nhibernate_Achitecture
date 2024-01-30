using AutoMapper;
using QLTS.Contract.Services.Company;
using QLTS.Domain.Entities.Identity;

namespace QLTS.Application.Mapper;
internal class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Company, Response.CompanyResponse>().ReverseMap();
    }
}
