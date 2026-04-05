using IMDB.Models.Db;
using System.Collections.Generic;

namespace IMDB.Repositories.Interfaces
{
    public interface IActorRepository
    {
        int Add(Actor actor);
        IEnumerable<Actor> GetAll();
        Actor GetById(int id);
        IEnumerable<Actor> GetByMovieId(int movieId);
        bool Update(int id, Actor actor);
        bool Delete(int id);
        int GetSoloMovieCountByActorId(int id);
    }
}