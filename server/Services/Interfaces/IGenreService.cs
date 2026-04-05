using IMDB.Models.Request;
using IMDB.Models.Response;
using System.Collections.Generic;

namespace IMDB.Services.Interfaces
{
    public interface IGenreService
    {
        int Add(GenreRequest genre);
        IEnumerable<GenreResponse> GetAll();
        GenreResponse GetById(int id);
        IEnumerable<GenreResponse> GetByMovieId(int movieId);
        bool Update(int id, GenreRequest genre);
        bool Delete(int id);
    }
}
