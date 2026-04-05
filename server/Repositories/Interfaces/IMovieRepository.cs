using IMDB.Models.Db;
using System.Collections.Generic;

namespace IMDB.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        int Add(string actorIds, string genreIds, Movie movie);
        IEnumerable<Movie> GetAll(int? year);
        IEnumerable<Movie> GetHomeContent();
        Movie GetById(int id);
        string GetCoverById(int id);
        bool Update(int id, IEnumerable<int> updatedActorIds, IEnumerable<int> updatedGenreIds, Movie movie);
        bool UpdateCover(int id, string url);
        bool Delete(int id);
    }
}