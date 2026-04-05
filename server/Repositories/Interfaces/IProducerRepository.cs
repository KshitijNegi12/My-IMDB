using IMDB.Models.Db;
using System.Collections.Generic;

namespace IMDB.Repositories.Interfaces
{
    public interface IProducerRepository
    {
        int Add(Producer producer);
        IEnumerable<Producer> GetAll();
        Producer GetById(int id);
        bool Update(int id, Producer producer);
        bool Delete(int id);
    }
}