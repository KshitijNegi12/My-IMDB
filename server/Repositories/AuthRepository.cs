using Dapper;
using IMDB.Models.Db;
using IMDB.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace IMDB.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IOptions<ConnectionString> connectionString)
        {
            _connectionString = connectionString.Value.IMDB;
        }

        public bool Add(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                        INSERT INTO
                            Foundation.Users (Email, PasswordHash)
                        VALUES
                            (@Email, @PasswordHash);";

            connection.Execute(query, user);
            return true;
        }

        public User GetbyEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                        SELECT
                            Email, PasswordHash
                        FROM
                            Foundation.Users
                        WHERE
                            Email = @Email;";

            return connection.QuerySingleOrDefault<User>(query, new { Email = email });
        }
    }
}
