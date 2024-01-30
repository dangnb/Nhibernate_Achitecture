using DemoCICD.Common.Abstractions.Shared;
using MediatR;

namespace QLTS.Contract.Abstractions.Message;
public interface ICommand : IRequest<Result>
{
}
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
