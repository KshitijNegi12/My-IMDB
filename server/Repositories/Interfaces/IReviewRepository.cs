using IMDB.Models.Db;
using System.Collections.Generic;

namespace IMDB.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        int Add(Review review);
        IEnumerable<Review> GetAll(int movieId);
        Review GetById(int id, int movieId);
        bool Update(int id, int movieId, Review review);
        bool Delete(int id, int movieId);
    }
}