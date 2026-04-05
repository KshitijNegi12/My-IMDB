using IMDB.Models.Request;
using IMDB.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDB.Services.Interfaces
{
    public interface IMovieService
    {
        int Add(MovieRequest movie);
        IEnumerable<object> GetAll(int? year, bool home);
        MovieResponse GetById(int id);
        bool Update(int id, MovieRequest movie);
        Task<string> UpdateCover(int id, IFormFile img);
        bool Delete(int id);
    }
}
