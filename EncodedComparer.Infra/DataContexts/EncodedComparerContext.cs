﻿using System;
using System.Data.SqlClient;
using System.Data;

namespace EncodedComparer.Infra.DataContexts
{
    public class EncodedComparerContext : IDisposable
    {
        public SqlConnection Connection { get; private set; }

        public EncodedComparerContext(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }
}
