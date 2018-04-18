using System;
using System.Data;
using System.Data.Common;


namespace EncodedComparer.Infra.DataContexts
{
    public class EncodedComparerContext : IDisposable
    {
        public DbConnection Connection { get; private set; }

        public EncodedComparerContext(DbConnection connection)
        {
            Connection = connection;
            Connection.Open();
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }
}
