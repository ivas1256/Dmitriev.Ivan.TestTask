using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace Dmitriev.Ivan.TestTask.DAL
{
    /// <summary>
    /// Коннкет к базе и выполнение запросов. Если когда-то понадобится несколько  коннектов, можно играться с GetInstance
    /// </summary>
    public class DBConnectionSingleton : IDisposable
    {
        static DBConnectionSingleton instance;
        SqlConnection connect;

        private DBConnectionSingleton()
        {
            connect = new SqlConnection(ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString);
            connect.Open();
        }

        public static DBConnectionSingleton GetInstance()
        {
            if (instance == null)
                instance = new DBConnectionSingleton();

            return instance;
        }

        public SqlTransaction GetTransaction()
        {
            return connect.BeginTransaction();
        }

        public void ExecSql(string sql, params SqlParameter[] sqlParameters)
        {            
            using(var com = new SqlCommand(sql, connect))
            {
                com.Parameters.AddRange(sqlParameters);
                com.ExecuteNonQuery();
            }
        }

        public void ExecSql(string sql, List<SqlParameter> sqlParameters, SqlTransaction tran)
        {
            using (var com = new SqlCommand(sql, connect, tran))
            {
                com.Parameters.AddRange(sqlParameters.ToArray());
                com.ExecuteNonQuery();
                com.Parameters.Clear();                
            }
        }

        public SqlDataReader ExecSqlWithReader(string sql, params SqlParameter[] sqlParameters)
        {
            using (var com = new SqlCommand(sql, connect))            
            {
                com.Parameters.AddRange(sqlParameters);
                var reader = com.ExecuteReader();

                return reader;
            }
        }

        public void Dispose()
        {
            if (connect == null)
                return;

            connect.Close();
            connect.Dispose();
        }
    }
}
