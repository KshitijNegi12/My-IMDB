using IMDB.Models.Request;
using IMDB.Models.Response;
using System.Collections.Generic;

namespace IMDB.Services.Interfaces
{
    public interface IReviewService
    {
        int Add(int movieId, ReviewRequest review);
        IEnumerable<ReviewResponse> GetAll(int movieId);
        ReviewResponse GetById(int id, int movieId);
        bool Update(int id, int movieId, ReviewRequest review);
        bool Delete(int id, int movieId);
    }
}
