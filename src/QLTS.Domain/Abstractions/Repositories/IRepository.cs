using NHibernate;
using System.Linq.Expressions;

namespace QLTS.Domain.Abstractions.Repositories;

public interface IRepository<T, IdT>
{
    ICriteria CreateCriteria();

    List<T> GetByCriteria(ICriteria _crit);

    T CreateNew(T entity);
    T Delete(T entity);
    T Update(T entity);
    T Save(T entity);

    IList<R> GetbyQuery<R>(string query, bool hql, params SQLParam[] _params);

    IList<R> GetbyQuery<R>(string query, int pageIndex, int pageSize, params SQLParam[] _params);

    T Getbykey(IdT key);

    List<T> GetbySQLQuery(string Query, params SQLParam[] _params);
    List<T> GetbyHQuery(string Query, params SQLParam[] _params);
    T Get(Expression<Func<T, bool>> predicate);

    void CommitChanges();
    void BindSession(object entity);
    void UnbindSession(object entity);
    void UnbindSession();
    void BeginTran();
    void CommitTran();
    void RolbackTran();
    IQueryable<T> Query { get; }
}

public class SQLParam
{
    public string ParameName { get; set; }

    public object ParamValue { get; set; }

    public SQLParam(string mParamName, object mParamValue)
    {
        ParameName = mParamName;
        ParamValue = mParamValue;
    }
}
