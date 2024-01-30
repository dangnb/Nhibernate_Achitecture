using QLTS.Contract.Abstractions.Shared;
using MediatR;

namespace QLTS.Contract.Abstractions.Message;
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
