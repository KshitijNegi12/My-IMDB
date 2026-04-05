using IMDB.Models.Db;
using IMDB.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace IMDB.Repositories
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(IOptions<ConnectionString> connectionString)
            : base(connectionString) { }

        public int Add(Genre genre)
        {
            string query = @"
                            INSERT INTO 
                                Foundation.Genres (Name)
                            VALUES 
                                (@Name);
                            SELECT 
                                SCOPE_IDENTITY();";

            return Add(query, genre);
        }

        public IEnumerable<Genre> GetAll()
        {
            string query = @"
                            SELECT
                                Id, Name
                            FROM 
                                Foundation.Genres;";

            return GetAll(query);
        }

        public Genre GetById(int id)
        {
            string query = @"
                            SELECT
                                Id, Name
                            FROM 
                                Foundation.Genres
                            WHERE
                                Id = @Id;";

            return GetById(query, new { Id = id });
        }

        public IEnumerable<Genre> GetByMovieId(int movieId)
        {
            string query = @"
                            SELECT
                                g.Id, g.Name
                            FROM
                                Foundation.Genres g
                                INNER JOIN Foundation.Genres_Movies gm ON g.Id = gm.GenreId
                            WHERE
                                gm.MovieId = @MovieId;";

            return GetAll(query, new { MovieId = movieId });
        }

        public bool Update(int id, Genre genre)
        {
            string query = @"
                            UPDATE 
                                Foundation.Genres
                            SET
                                Name = @Name,
                                UpdatedAt = GETDATE()
                            WHERE
                                Id = @Id;";

            Update(query, new
            {
                Id = id,
                genre.Name
            });
            return true;
        }

        public bool Delete(int id)
        {
            string query = @"
                            DELETE FROM 
                                Foundation.Genres_Movies
                            WHERE 
                                GenreId = @Id;
                            
                            DELETE FROM 
                                Foundation.Genres
                            WHERE 
                                Id = @Id;";

            Delete(query, new { Id = id });
            return true;
        }

        public int GetSoloMovieCountByGenreId(int id)
        {
            string query = @"
                            SELECT
	                            COALESCE(COUNT(MovieId), 0)
                            FROM (
	                            SELECT
		                            gm.MovieId
	                            FROM	
		                            Foundation.Genres_Movies gm
		                            INNER JOIN (
			                            SELECT
				                            MovieId
			                            FROM
				                            Foundation.Genres_Movies
			                            WHERE
				                            GenreId = @GenreId
		                            ) AS t ON gm.MovieId = t.MovieId
	                            GROUP BY
		                            gm.MovieId
	                            HAVING
		                            COUNT(gm.MovieId) = 1
	                            ) AS Tmp;";

            return ExecuteQuery<int>(query, new { GenreId = id });
        }
    }
}
