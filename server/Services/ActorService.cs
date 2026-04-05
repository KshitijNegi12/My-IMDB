using IMDB.Exceptions;
using IMDB.Models.Db;
using IMDB.Models.Request;
using IMDB.Models.Response;
using IMDB.Repositories.Interfaces;
using IMDB.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IMDB.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;

        public ActorService(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public int Add(ActorRequest actor)
        {
            Validate(actor);
            return _actorRepository.Add(new Actor
            {
                Name = actor.Name,
                Bio = actor.Bio,
                DateOfBirth = actor.DateOfBirth,
                Gender = actor.Gender,
            });
        }

        public IEnumerable<ActorResponse> GetAll()
        {
            return _actorRepository.GetAll().Select(a => new ActorResponse
            {
                Id = a.Id,
                Name = a.Name,
                Gender = a.Gender,
                DateOfBirth = a.DateOfBirth,
                Bio = a.Bio,
            });
        }

        public ActorResponse GetById(int id)
        {
            var actor = _actorRepository.GetById(id);
            CheckExistense(id, actor);
            return new ActorResponse
            {
                Id = actor.Id,
                Name = actor.Name,
                Gender = actor.Gender,
                DateOfBirth = actor.DateOfBirth,
                Bio = actor.Bio,
            };
        }

        public IEnumerable<ActorResponse> GetByMovieId(int movieId)
        {
            return _actorRepository.GetByMovieId(movieId).Select(a => new ActorResponse
            {
                Id = a.Id,
                Name = a.Name,
                Gender = a.Gender,
                DateOfBirth = a.DateOfBirth,
                Bio = a.Bio,
            });
        }

        public bool Update(int id, ActorRequest actor)
        {
            CheckExistense(id);
            Validate(actor);
            return _actorRepository.Update(id, new Actor
            {
                Name = actor.Name,
                Gender = actor.Gender,
                DateOfBirth = actor.DateOfBirth,
                Bio = actor.Bio,
            });
        }

        public bool Delete(int id)
        {
            CheckExistense(id);
            IsActingAloneInAMovie(id);
            return _actorRepository.Delete(id);
        }

        public void  CheckExistense(int id)
        {
            var actor = _actorRepository.GetById(id);
            if (actor == null)
            {
                throw new ResourceNotFoundException($"Actor with id {id} not found");
            }
        }

        public void CheckExistense(int id, Actor actor)
        {
            if (actor == null)
            {
                throw new ResourceNotFoundException($"Actor with id {id} not found");

            }
        }

        public void Validate(ActorRequest actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException("Actor cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(actor.Name))
            {
                throw new InvalidFieldException("Name is required.");
            }
            if (string.IsNullOrWhiteSpace(actor.Bio))
            {
                throw new InvalidFieldException("Bio is required.");
            }
            if (string.IsNullOrWhiteSpace(actor.Gender))
            {
                throw new InvalidFieldException("Gender is required.");
            }
            if (actor.DateOfBirth > DateTime.Now)
            {
                throw new InvalidFieldException("DateOfBirth cannot be greater than current date.");
            }
        }

        public void IsActingAloneInAMovie(int id)
        {
            var count = _actorRepository.GetSoloMovieCountByActorId(id);
            if (count > 0)
            {
                throw new InvalidOperationException($"Cannot Delete, Actor is acting alone in a total of {count} movie.");
            }
        }
    }
}
