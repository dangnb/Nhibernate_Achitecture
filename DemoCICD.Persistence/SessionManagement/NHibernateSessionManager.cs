using NHibernate;
using NHibernate.Cfg;
using QLTS.Contract;
using System.Collections;

namespace QLTS.Persistence.SessionManagement
{
    /// <summary>
    /// Handles creation and management of sessions and transactions.  It is a singleton because 
    /// building the initial session factory is very expensive. Inspiration for this class came 
    /// from Chapter 8 of Hibernate in Action by Bauer and King.  Although it is a sealed singleton
    /// you can use TypeMock (http://www.typemock.com) for more flexible testing.
    /// </summary>
    public sealed class NHibernateSessionManager
    {
        #region Thread-safe, lazy Singleton
        public static NHibernateSessionManager Instance
        {
            get
            {
                return Nested.NHibernateSessionManager;
                //if (_Instance == null) _Instance = new NHibernateSessionManager();
                //return _Instance;
            }
        }

        /// <summary>
        /// Private constructor to enforce singleton
        /// </summary>
        private NHibernateSessionManager()
        {
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            static Nested() { }
            internal static readonly NHibernateSessionManager NHibernateSessionManager =
                new NHibernateSessionManager();
        }
        #endregion

        static object looker = new object();

        /// <summary>
        /// This method attempts to find a session factory stored in <see cref="sessionFactories" />
        /// via its name; if it can't be found it creates a new one and adds it the hashtable.
        /// </summary>
        /// <param name="sessionFactoryConfigPath">Path location of the factory config</param>
        internal ISessionFactory GetSessionFactoryFor(string sessionFactoryConfigPath)
        {
            //sessionFactoryConfigPath = AppDomain.CurrentDomain.BaseDirectory + @"Config\NHibernateConfig.xml";
            Check.Require(!string.IsNullOrEmpty(sessionFactoryConfigPath),
                "sessionFactoryConfigPath may not be null nor empty");
            lock (looker)
            {
                //  Attempt to retrieve a stored SessionFactory from the hashtable.
                ISessionFactory? sessionFactory = (ISessionFactory?)ContextSessionFactories[sessionFactoryConfigPath];

                //  Failed to find a matching SessionFactory so make a new one.
                if (sessionFactory == null)
                {
                    Check.Require(File.Exists(sessionFactoryConfigPath),
                        "The config file at '" + sessionFactoryConfigPath + "' could not be found");
                    string connectString = string.Empty;
                    string PathFileConfig = sessionFactoryConfigPath;
                    string[] configs = sessionFactoryConfigPath.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                    if (configs.Length > 1)
                    {
                        connectString = configs[1];
                        PathFileConfig = configs[0];
                    }
                    //end adding

                    Check.Require(File.Exists(PathFileConfig),
                        "The config file at '" + PathFileConfig + "' could not be found");
                    Configuration cfg = new Configuration();
                    cfg.Configure(PathFileConfig);
                    //sửa schema Name cho cfg
                    if (!string.IsNullOrWhiteSpace(connectString))
                    {
                        cfg.SetProperty(NHibernate.Cfg.Environment.ConnectionString, connectString);
                    }

                    sessionFactory = cfg.BuildSessionFactory();
                    //  Now that we have our Configuration object, create a new SessionFactory
                    if (sessionFactory == null)
                    {
                        throw new InvalidOperationException("cfg.BuildSessionFactory() returned null.");
                    }
                    ContextSessionFactories[sessionFactoryConfigPath] = sessionFactory;
                }

                return sessionFactory;
            }

        }

        internal ISession GetSessionFrom(string sessionFactoryConfigPath)
        {
            return GetSessionFrom(sessionFactoryConfigPath, null);
        }

        /// <summary>
        /// Gets a session with or without an interceptor.  This method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private ISession GetSessionFrom(string sessionFactoryConfigPath, IInterceptor interceptor)
        {
            ISession? session = (ISession?)ContextSessions[sessionFactoryConfigPath];

            if (session == null)
            {
                if (interceptor != null)
                {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession(interceptor);
                }
                else
                {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession();
                }

                // linhpn set flush mode for transaction commit
                //session.FlushMode = FlushMode.Manual;

                ContextSessions[sessionFactoryConfigPath] = session;
            }

            Check.Ensure(session != null, "session was null");

            return session;
        }

        /// <summary>
        /// not Flushes anything  in the session and closes the connection. Create By Duyet for rolback session
        /// </summary>
        internal void CloseSession(string sessionFactoryConfigPath)
        {
            ISession? session = (ISession?)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen)
            {
                session.Close();
            }
            ContextSessions.Remove(sessionFactoryConfigPath);
        }

        internal ITransaction BeginTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction? transaction = (ITransaction?)ContextTransactions[sessionFactoryConfigPath];
            if (transaction == null)
            {
                transaction = GetSessionFrom(sessionFactoryConfigPath).BeginTransaction();
                ContextTransactions.Add(sessionFactoryConfigPath, transaction);
            }
            return transaction;
        }

        internal void CommitTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction? transaction = (ITransaction?)ContextTransactions[sessionFactoryConfigPath];
            try
            {
                if (transaction != null)
                {
                    if (HasOpenTransactionOn(sessionFactoryConfigPath))
                    {
                        //GetSessionFrom(sessionFactoryConfigPath).Flush();
                        transaction.Commit();
                        ContextTransactions.Remove(sessionFactoryConfigPath);
                    }
                }
            }
            catch (HibernateException)
            {
                RollbackTransactionOn(sessionFactoryConfigPath);
                throw;
            }
        }

        internal bool HasOpenTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction? transaction = (ITransaction?)ContextTransactions[sessionFactoryConfigPath];
            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        internal void RollbackTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction? transaction = (ITransaction?)ContextTransactions[sessionFactoryConfigPath];
            try
            {
                if (transaction != null)
                {
                    if (HasOpenTransactionOn(sessionFactoryConfigPath))
                    {
                        transaction.Rollback();
                    }
                }
                ContextTransactions.Remove(sessionFactoryConfigPath);
            }
            finally
            {
                CloseSession(sessionFactoryConfigPath);
            }
        }

        #region SessionStateLess
        public IStatelessSession GetSessionStateLessFrom(string sessionFactoryConfigPath)
        {
            var session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenStatelessSession();
            session.SetBatchSize(100);
            return session;
        }
        #endregion

        /// <summary>
        /// Since multiple databases may be in use, there may be one transaction per database 
        /// persisted at any one time.  The easiest way to store them is via a hashtable
        /// with the key being tied to session factory.  If within a web context, this uses
        /// <see cref="HttpContext" /> instead of the WinForms specific <see cref="CallContext" />.  
        /// Discussion concerning this found at http://forum.springframework.net/showthread.php?t=572
        /// </summary>
        private Hashtable ContextTransactions
        {
            get
            {
                return SessionContext.Transactions;
            }
        }

        /// <summary>
        /// Since multiple databases may be in use, there may be one session per database 
        /// persisted at any one time.  The easiest way to store them is via a hashtable
        /// with the key being tied to session factory.  If within a web context, this uses
        /// <see cref="HttpContext" /> instead of the WinForms specific <see cref="CallContext" />.  
        /// Discussion concerning this found at http://forum.springframework.net/showthread.php?t=572
        /// </summary>
        private Hashtable ContextSessions
        {
            get
            {
                return SessionContext.Sessions;
            }
        }

        private Hashtable ContextSessionFactories
        {
            get
            {
                return SessionFactoryContext.SessionFactories;
            }
        }

        public NhibernateSessionFactoryStore SessionFactoryContext
        {
            get
            {
                return IoC.Resolve<NhibernateSessionFactoryStore>();
            }
        }

        public NhibernateSessionStore SessionContext
        {
            get
            {
                return IoC.Resolve<NhibernateSessionStore>();
            }
        }
    }
}