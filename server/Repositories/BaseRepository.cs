using System;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Data;

namespace IMDB.Repositories
{
    public class BaseRepository<T> where T : class
    {
        private readonly string _connectionString;

        public BaseRepository(IOptions<ConnectionString> connectionString)
        {
            _connectionString = connectionString.Value.IMDB;
        }   

        public int Add(string query, T args)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.ExecuteScalar<int>(query, args);
        }

        public List<T> GetAll(string query, Object args = null)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<T>(query, args).ToList();
        }

        public T GetById(string query, Object args)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.QuerySingleOrDefault<T>(query, args);
        }

        public void Update(string query, Object args)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(query, args);
        }

        public void Delete(string query, Object args)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(query, args);
        }

        public void ExecuteProdcedure(string procedure, Object args)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(procedure, args, commandType: CommandType.StoredProcedure);
        }

        public U ExecuteQuery<U>(string query, Object args)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.ExecuteScalar<U>(query, args);
        }
    }
}