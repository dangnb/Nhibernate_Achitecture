using MediatR;
using QLTS.Contract.Abstractions.Shared;

namespace QLTS.Contract.Abstractions.Message;
public interface ICommand : IRequest<Result>
{
}
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
