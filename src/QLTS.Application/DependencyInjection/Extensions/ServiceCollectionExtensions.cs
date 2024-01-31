using QLTS.Application.Mapper;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using QLTS.Application.Behaviors;
using FluentValidation;
using QLTS.Application.UserCases.jwt;

namespace QLTS.Application.DependencyInjection.Extensions;
public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddConfigureMediatR(this IServiceCollection services)
    {
        services.AddScoped<IJwtUtils, JwtUtils>();
        services.AddMediatR(options =>
       options.RegisterServicesFromAssembly(AssemblyReference.assembly))
       .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
       .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
       .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformancePipelineBehavior<,>))
       //.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>))
       .AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>))
      .AddValidatorsFromAssembly(Contract.AssemblyReference.assembly, includeInternalTypes: true);
        return services;
    }
   

    public static IServiceCollection AddConfigureAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(typeof(ServiceProfile));
}
