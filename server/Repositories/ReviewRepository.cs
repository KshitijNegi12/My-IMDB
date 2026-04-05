using IMDB.Models.Db;
using IMDB.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace IMDB.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(IOptions<ConnectionString> connectionString)
            : base(connectionString) { }
        public int Add(Review review)
        {
            string query = @"
                            INSERT INTO 
                                Foundation.Reviews (Message, MovieId)
                            VALUES 
                                (@Message, @MovieId);
                            SELECT 
                                SCOPE_IDENTITY();";

            return Add(query, review);
        }

        public IEnumerable<Review> GetAll(int movieId)
        {
            string query = @"
                            SELECT
                                Id, Message, MovieId
                            FROM 
                                Foundation.Reviews
                            WHERE
                                MovieId = @MovieId";

            return GetAll(query, new { MovieId = movieId });
        }

        public Review GetById(int id, int movieId)
        {
            string query = @"
                            SELECT
                                Id, Message, MovieId
                            FROM 
                                Foundation.Reviews
                            WHERE
                                MovieId = @MovieId AND
                                Id = @Id;";

            return GetById(query, new { Id = id, MovieId = movieId });
        }

        public bool Update(int id, int movieId, Review review)
        {
            string query = @"
                            UPDATE 
                                Foundation.Reviews
                            SET
                                Message = @Message,
                                UpdatedAt = GETDATE()
                            WHERE
                                MovieId = @MovieId AND
                                Id = @Id;";

            Update(query, new
            {
                Id = id,
                review.Message,
                MovieId = movieId
            });
            return true;
        }

        public bool Delete(int id, int movieId)
        {
            string query = @"
                            DELETE FROM 
                                Foundation.Reviews
                            WHERE 
                                MovieId = @MovieId AND
                                Id = @Id;";

            Delete(query, new { Id = id, MovieId = movieId });
            return true;
        }
    }
}
