using DemoCICD.Common.Abstractions.Shared;
using MediatR;
namespace QLTS.Contract.Abstractions.Message;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
