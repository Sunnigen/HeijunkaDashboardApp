using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HeijunkaAppLibrary.Databases
{
    public class HerokuDataAccess : IHerokuDataAccess
    {
        private readonly IConfiguration _config;

        public HerokuDataAccess(IConfiguration config)
        {
            _config = config;
        }
        public List<T> LoadData<T, U>(string sqlStatement,
                                      U parameters,
                                      string connectionStringName)
        {
            string connectionString = TranslateDatabaseUrl(_config.GetConnectionString(connectionStringName));
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement,
                                T parameters,
                                string connectionStringName)
        {
            string connectionString = TranslateDatabaseUrl(_config.GetConnectionString(connectionStringName));
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
            }
        }

        private string TranslateDatabaseUrl(string connectionString)
        {
            var databaseUri = new Uri(connectionString);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/')
            };

            return builder.ToString();
        }
    }
}
