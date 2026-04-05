using Dapper;
using IMDB.Models.Db;
using IMDB.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IMDB.Repositories
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        private readonly string _connectionString;

        public MovieRepository(IOptions<ConnectionString> connectionString)
            : base(connectionString)
        {
            _connectionString = connectionString.Value.IMDB;
        }

        public int Add(string actorIds, string genreIds, Movie movie)
        {
            using var connection = new SqlConnection(_connectionString);
            string procedure = "Foundation.usp_InsertMovie";
            return connection.ExecuteScalar<int>(procedure, new
            {
                movie.Name,
                movie.YearOfRelease,
                movie.Plot,
                movie.CoverImage,
                movie.ProducerId,
                ActorIds = actorIds,
                GenreIds = genreIds
            }, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Movie> GetAll(int? year)
        {
            string query = @"
                            SELECT
                                Id, Name, YearOfRelease, Plot, CoverImage, ProducerId
                            FROM 
                                Foundation.Movies;";
            if (year.HasValue)
            {
                query = @"
                            SELECT
                                Id, Name, YearOfRelease, Plot, CoverImage, ProducerId
                            FROM 
                                Foundation.Movies
                            WHERE 
                                YearOfRelease = @Year;";

                return GetAll(query, new { Year = year });
            }

            return GetAll(query);
        }

        public IEnumerable<Movie> GetHomeContent()
        {
            string query = @"
                            SELECT
                                Id, Name, Plot, CoverImage
                            FROM 
                                Foundation.Movies;";

            return GetAll(query);
        }

        public Movie GetById(int id)
        {
            string query = @"
                            SELECT
                                Id, Name, YearOfRelease, Plot, CoverImage, ProducerId
                            FROM 
                                Foundation.Movies
                            WHERE 
                                Id = @Id;";

            return GetById(query, new { Id = id });
        }

        public string GetCoverById(int id)
        {
            string query = @"
                            SELECT
                                CoverImage
                            FROM 
                                Foundation.Movies
                            WHERE 
                                Id = @Id;";

            return  ExecuteQuery<string>(query, new { Id = id });
        }

        public bool Update(int id, IEnumerable<int> updatedActorIds, IEnumerable<int> updatedGenreIds, Movie movie)
        {
            using var connection = new SqlConnection(_connectionString);

            string query1 = @"
                            SELECT
                                ActorId
                            FROM 
                                Foundation.Actors_Movies
                            WHERE 
                                MovieId = @MovieId;";

            List<int> currentActors = connection.Query<int>(query1, new { MovieId = id }).ToList();
            List<int> actorsToDelete = currentActors.Except(updatedActorIds).ToList();
            List<int> actorsToAdd = updatedActorIds.Except(currentActors).ToList();

            string query2 = @"
                            SELECT
                                GenreId
                            FROM 
                                Foundation.Genres_Movies
                            WHERE 
                                MovieId = @MovieId;";

            List<int> currentGenres = connection.Query<int>(query2, new { MovieId = id }).ToList();
            List<int> genresToDelete = currentGenres.Except(updatedGenreIds).ToList();
            List<int> genresToAdd = updatedGenreIds.Except(currentGenres).ToList();

            string procedure = "Foundation.usp_UpdateMovie";
            ExecuteProdcedure(procedure, new
            {
                Id = id,
                movie.Name,
                movie.YearOfRelease,
                movie.Plot,
                movie.CoverImage,
                movie.ProducerId,
                ActorsToAdd = string.Join(",", actorsToAdd),
                ActorsToDelete = string.Join(",", actorsToDelete),
                GenresToAdd = string.Join(",", genresToAdd),
                GenresToDelete = string.Join(",", genresToDelete),
            });
            return true;
        }

        public bool UpdateCover(int id, string url)
        {
            string query = @"UPDATE
                                Foundation.Movies
                            SET 
                                CoverImage = @Url
                            WHERE 
                                Id = @Id;";

            Update(query, new { Url = url, Id = id });
            return true;
        }

        public bool Delete(int id)
        {
            string procedure = "Foundation.usp_DeleteMovie";
            ExecuteProdcedure(procedure, new { MovieId = id });
            return true;
        }
    }
}
