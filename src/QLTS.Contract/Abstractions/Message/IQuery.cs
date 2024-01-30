using QLTS.Contract.Abstractions.Shared;
using MediatR;
namespace QLTS.Contract.Abstractions.Message;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
