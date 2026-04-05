using IMDB.Models.Db;
using System.Collections.Generic;

namespace IMDB.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        int Add(Genre genre);
        IEnumerable<Genre> GetAll();
        Genre GetById(int id);
        IEnumerable<Genre> GetByMovieId(int movieId);
        bool Update(int id, Genre genre);
        bool Delete(int id);
        int GetSoloMovieCountByGenreId(int id);
    }
}