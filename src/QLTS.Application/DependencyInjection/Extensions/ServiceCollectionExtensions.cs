using QLTS.Application.Mapper;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using QLTS.Application.Behaviors;
using FluentValidation;

namespace QLTS.Application.DependencyInjection.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigureMediatR(this IServiceCollection services) =>
        services.AddMediatR(options =>
        options.RegisterServicesFromAssembly(AssemblyReference.assembly))
        .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformancePipelineBehavior<,>))
        //.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>))
       .AddValidatorsFromAssembly(Contract.AssemblyReference.assembly, includeInternalTypes: true);

    public static IServiceCollection AddConfigureAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(typeof(ServiceProfile));
}
