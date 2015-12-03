using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paymetric.Proc;

namespace DataBus.Data
{
    public interface IDataContext
    {
        void WithinTransaction(Action action);
        void BindToLocalThread();
        object ExecuteProcedure<P, E>(Procedure<P, E> proc, P parms) where E : new();
        IEnumerable<E> GetList<E>(string sql);
        IDbCommand CreateCommand();
    }

    public class DataContext : IDataContext
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private static IThreadContextWrapper _threadContext;
        public DataContext(IDbConnection connection, IThreadContextWrapper tcw)
        {
            _connection = connection;
            _threadContext = tcw;
        }

        public static IDataContext Current
        {get { return _threadContext.CurrentDataContext; }}
        public void WithinTransaction(Action action)
        {
            if (!inTransaction())
            {
                using(_transaction =  _connection.BeginTransaction())
                {
                    action();
                    _transaction.Commit(); 
                }
            }
            else
            {
                action();
            }
        }

        private bool inTransaction()
        {
            return _transaction != null;
        }


        public void BindToLocalThread()
        {
            _threadContext.CurrentDataContext = this;
        }
        public object ExecuteProcedure<P, E>(Procedure<P, E> proc, P parms) where E : new()
        {
            if (proc.ReturnsRecords)
                return proc.ExecuteQuery(parms, this);
            return proc.Execute(parms, this);
        }

        public IEnumerable<E> GetList<E>(string sql)
        { 
            throw new NotImplementedException();
        }

        public IDbCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }
    }
}
