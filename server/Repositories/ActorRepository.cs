using IMDB.Models.Db;
using IMDB.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace IMDB.Repositories
{
    public class ActorRepository : BaseRepository<Actor>, IActorRepository
    {
        public ActorRepository(IOptions<ConnectionString> connectionString)
            : base(connectionString) { }

        public int Add(Actor actor)
        {
            string query = @"
                            INSERT INTO 
                                Foundation.Actors (Name, Gender, DateOfBirth, Bio)
                            VALUES 
                                (@Name, @Gender, @DateOfBirth, @Bio);
                            SELECT 
                                SCOPE_IDENTITY();";

            return Add(query, actor);
        }

        public IEnumerable<Actor> GetAll()
        {
            string query = @"
                            SELECT
                                Id, Name, Gender, DateOfBirth, Bio
                            FROM 
                                Foundation.Actors;";

            return GetAll(query);
        }

        public Actor GetById(int id)
        {
            string query = @"
                            SELECT
                                Id, Name, Gender, DateOfBirth, Bio
                            FROM 
                                Foundation.Actors
                            WHERE
                                Id = @Id;";

            return GetById(query, new { Id = id});
        }

        public IEnumerable<Actor> GetByMovieId(int movieId)
        {
            string query = @"
                            SELECT
                                a.Id, a.Name, a.Gender, a.DateOfBirth, a.Bio
                            FROM
                                Foundation.Actors a
                                INNER JOIN Foundation.Actors_Movies am ON a.Id = am.ActorId
                            WHERE
                                am.MovieId = @MovieId;";

            return GetAll(query, new { MovieId = movieId });
        }


        public bool Update(int id, Actor actor)
        {
            string query = @"
                            UPDATE 
                                Foundation.Actors
                            SET
                                Name = @Name,
                                Gender = @Gender,
                                DateOfBirth = @DateOfBirth,
                                Bio = @Bio,
                                UpdatedAt = GETDATE()
                            WHERE
                                Id = @Id;";

            Update(query, new {
                actor.Name,
                actor.Gender,
                actor.DateOfBirth,
                actor.Bio,
                Id = id
            });
            return true;
        }

        public bool Delete(int id)
        {
            string procedure = "Foundation.usp_DeleteActor";
            ExecuteProdcedure(procedure, new { ActorId = id });
            return true;
        }

        public int GetSoloMovieCountByActorId(int id)
        {
            string query = @"
                            SELECT
	                            COALESCE(COUNT(MovieId), 0)
                            FROM (
	                            SELECT
		                            am.MovieId
	                            FROM	
		                            Foundation.Actors_Movies am
		                            INNER JOIN (
			                            SELECT
				                            MovieId
			                            FROM
				                            Foundation.Actors_Movies
			                            WHERE
				                            ActorId = @ActorId
		                            ) AS t ON am.MovieId = t.MovieId
	                            GROUP BY
		                            am.MovieId
	                            HAVING
		                            COUNT(am.MovieId) = 1
	                            ) AS Tmp;";

            return ExecuteQuery<int>(query, new { ActorId = id});
        }
    }
}
