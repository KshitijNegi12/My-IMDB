using IMDB.Models.Db;
using IMDB.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace IMDB.Repositories
{
    public class ProducerRepository : BaseRepository<Producer>, IProducerRepository
    {
        public ProducerRepository(IOptions<ConnectionString> connectionString)
            : base(connectionString) { }

        public int Add(Producer producer)
        {
            string query = @"
                            INSERT INTO 
                                Foundation.Producers (Name, Gender, DateOfBirth, Bio)
                            VALUES 
                                (@Name, @Gender, @DateOfBirth, @Bio);
                            SELECT 
                                SCOPE_IDENTITY();";

            return Add(query, producer);
        }

        public IEnumerable<Producer> GetAll()
        {
            string query = @"
                            SELECT
                                Id, Name, Gender, DateOfBirth, Bio
                            FROM 
                                Foundation.Producers;";

            return GetAll(query);
        }

        public Producer GetById(int id)
        {
            string query = @"
                            SELECT
                                Id, Name, Gender, DateOfBirth, Bio
                            FROM 
                                Foundation.Producers
                            WHERE
                                Id = @Id;";

            return GetById(query, new { Id = id });
        }

        public bool Update(int id, Producer producer)
        {
            string query = @"
                            UPDATE 
                                Foundation.Producers
                            SET
                                Name = @Name,
                                Gender = @Gender,
                                DateOfBirth = @DateOfBirth,
                                Bio = @Bio,
                                UpdatedAt = GETDATE()
                            WHERE
                                Id = @Id;";

            Update(query, new
            {
                producer.Name,
                producer.Gender,
                producer.DateOfBirth,
                producer.Bio,
                Id = id
            });
            return true;
        }

        public bool Delete(int id)
        {
            string procedure = "Foundation.usp_DeleteProducer";
            ExecuteProdcedure(procedure, new { ProducerId = id });
            return true;
        }
    }
}
