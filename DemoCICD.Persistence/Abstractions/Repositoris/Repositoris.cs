using NHibernate.Transform;
using NHibernate;
using QLTS.Domain.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using QLTS.Persistence.SessionManagement;

namespace QLTS.Persistence.Abstractions.Repositoris
{
    public class Repository<T, IdT> : IRepository<T, IdT>
    {
        public Repository(string sessionFactoryConfigPath, string connectionString = null)
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
                SessionFactoryConfigPath = SessionFactoryConfigPath + "#" + connectionString.Trim();
            if (!sessionFactoryConfigPath.Contains(AppDomain.CurrentDomain.BaseDirectory))
                SessionFactoryConfigPath = AppDomain.CurrentDomain.BaseDirectory + @"" + sessionFactoryConfigPath;
            else SessionFactoryConfigPath = sessionFactoryConfigPath;
        }

        /// <summary>
        /// CreateNew object 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T CreateNew(T entity)
        {
            NHibernateSession.Save(entity);
            return entity;
        }
        public virtual T Delete(T entity)
        {
            NHibernateSession.Delete(entity);
            return entity;
        }

        public virtual T Update(T entity)
        {
            NHibernateSession.Update(entity);
            return entity;
        }
        public virtual T Save(T entity)
        {
            NHibernateSession.SaveOrUpdate(entity);
            return entity;
        }

        public IList<R> GetbyQuery<R>(string query, bool hql, params SQLParam[] _params)
        {
            var iQuery = hql ? NHibernateSession.CreateQuery(query) : NHibernateSession.CreateSQLQuery(query);
            foreach (SQLParam _para in _params)
            {
                iQuery.SetParameter(_para.ParameName, _para.ParamValue);
            }
            return iQuery.SetResultTransformer(Transformers.AliasToBean<R>()).List<R>();
        }

        public IList<R> GetbyQuery<R>(string query, int pageIndex, int pageSize, params SQLParam[] _params)
        {
            var iQuery = NHibernateSession.CreateSQLQuery(query);
            foreach (SQLParam _para in _params)
            {
                iQuery.SetParameter(_para.ParameName, _para.ParamValue);
            }
            iQuery.SetFirstResult(pageIndex * pageSize);
            iQuery.SetMaxResults(pageSize);
            return iQuery.SetResultTransformer(Transformers.AliasToBean<R>()).List<R>();
        }

        /// <summary>
        /// Loads an instance of type T from the DB based on its ID.
        /// </summary>
        public virtual T Getbykey(IdT key)
        {
            object entity = NHibernateSession.Get(persitentType, key);
            return (entity == null) ? default(T) : (T)entity;
        }

        public virtual List<T> GetByCriteria(ICriteria _crit)
        {
            if (_fromIndex >= 0 && _MaxResult > 0)
            {
                _crit.SetFirstResult(_fromIndex);
                _crit.SetMaxResults(_MaxResult);
                List<T> ret = _crit.List<T>() as List<T>;
                ResetFetchPage();
                return ret;
            }
            else return _crit.List<T>() as List<T>;
        }

        public virtual ICriteria CreateCriteria()
        {
            return NHibernateSession.CreateCriteria(persitentType);
        }

        public List<T> GetbySQLQuery(string Query, params SQLParam[] _params)
        {
            IQuery q = NHibernateSession.CreateSQLQuery(Query).AddEntity(typeof(T));
            foreach (SQLParam _para in _params)
            {
                q.SetParameter(_para.ParameName, _para.ParamValue);
            }
            if (_fromIndex >= 0 && _MaxResult > 0)
            {
                q.SetFirstResult(_fromIndex);
                q.SetMaxResults(_MaxResult);
                List<T> ret = q.List<T>() as List<T>;
                ResetFetchPage();
                return ret;
            }
            return q.List<T>() as List<T>;
        }

        /// <summary>
        /// Fetch entity queryable
        /// </summary>
        /// <param name="predicate">Predicate expression</param>
        /// <returns>Queryable instance</returns>
        private IQueryable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            return Query.Where(predicate);
        }
        /// <summary>
        /// Get intity by expression
        /// </summary>
        /// <param name="predicate">Predicate expression</param>
        /// <returns>Entity</returns>
        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            var results = Fetch(predicate).ToList();
            return (results.Count > 0) ? results[0] : default(T);
        }

        public List<T> GetbyHQuery(string Query, params SQLParam[] _params)
        {
            IQuery q = NHibernateSession.CreateQuery(Query);
            foreach (SQLParam _para in _params)
            {
                q.SetParameter(_para.ParameName, _para.ParamValue);
            }
            if (_fromIndex >= 0 && _MaxResult > 0)
            {
                q.SetFirstResult(_fromIndex);
                q.SetMaxResults(_MaxResult);
                List<T> ret = q.List<T>() as List<T>;
                ResetFetchPage();
                return ret;
            }
            return q.List<T>() as List<T>;
        }

        public object ExcuteNonQuery(string query, bool isHQL, params SQLParam[] _params)
        {
            IQuery q = isHQL ? NHibernateSession.CreateQuery(query) : NHibernateSession.CreateSQLQuery(query);
            foreach (SQLParam _para in _params)
            {
                q.SetParameter(_para.ParameName, _para.ParamValue);
            }
            int rowCount = q.ExecuteUpdate();
            return rowCount;
        }

        /// <summary>
        /// Commits changes regardless of whether there's an open transaction or not
        /// </summary>
        public void CommitChanges()
        {
            NHibernateSessionManager.Instance.GetSessionFrom(SessionFactoryConfigPath).Flush();
        }

        /// <summary>
        /// Exposes the ISession used within the DAO.
        /// </summary>
        protected ISession NHibernateSession
        {
            get
            {
                ISession ret = NHibernateSessionManager.Instance.GetSessionFrom(SessionFactoryConfigPath);
                return ret;
            }
        }

        private Type persitentType = typeof(T);
        protected readonly string SessionFactoryConfigPath;
        public void BindSession(object _obj)
        {
            if (_obj is ICollection<T>)
            {
                foreach (T entity in (ICollection<T>)_obj) NHibernateSession.Persist(entity);
            }
            else
                NHibernateSession.Persist(_obj);
        }
        public void UnbindSession(object _obj)
        {
            if (_obj is ICollection<T>)
            {
                foreach (T entity in (ICollection<T>)_obj) NHibernateSession.Evict(entity);
            }
            else
                NHibernateSession.Evict(_obj);
        }

        public void UnbindSession()
        {
            NHibernateSession.Clear();
        }

        public void BeginTran()
        {
            NHibernateSessionManager.Instance.BeginTransactionOn(SessionFactoryConfigPath);
        }
        public void CommitTran()
        {
            NHibernateSessionManager.Instance.CommitTransactionOn(SessionFactoryConfigPath);
        }
        public void RolbackTran()
        {
            NHibernateSessionManager.Instance.RollbackTransactionOn(SessionFactoryConfigPath);
        }

        public IQueryable<T> Query
        {
            get
            {
                return NHibernateSession.Query<T>();
            }
        }

        //paging        
        private int _fromIndex = -1;
        private int _MaxResult = 0;
        private void ResetFetchPage()
        {
            _fromIndex = -1;
            _MaxResult = 0;
        }
    }

}
