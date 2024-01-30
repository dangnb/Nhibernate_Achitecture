using System.Collections;

namespace QLTS.Persistence.SessionManagement
{
    public class NhibernateSessionFactoryStore
    {
        public Hashtable SessionFactories = new Hashtable();
    }

    public class NhibernateSessionStore : IDisposable
    {
        public Hashtable Sessions = new Hashtable();
        public Hashtable Transactions = new Hashtable();
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
