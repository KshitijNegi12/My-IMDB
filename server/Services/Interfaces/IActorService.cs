using IMDB.Models.Request;
using IMDB.Models.Response;
using System.Collections.Generic;

namespace IMDB.Services.Interfaces
{
    public interface IActorService
    {
        int Add(ActorRequest actor);
        IEnumerable<ActorResponse> GetAll();
        ActorResponse GetById(int id);
        IEnumerable<ActorResponse> GetByMovieId(int movieId);
        bool Update(int id, ActorRequest actor);
        bool Delete(int id);     
    }
}
