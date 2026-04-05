using IMDB.Models.Request;
using IMDB.Models.Response;
using System.Collections.Generic;

namespace IMDB.Services.Interfaces
{
    public interface IProducerService
    {
        int Add(ProducerRequest producer);
        IEnumerable<ProducerResponse> GetAll();
        ProducerResponse GetById(int id);
        bool Update(int id, ProducerRequest producer);
        bool Delete(int id);
    }
}
