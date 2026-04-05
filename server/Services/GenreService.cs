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

    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public int Add(GenreRequest genre)
        {
            Validate(genre);
            return _genreRepository.Add(new Genre
            {
                Name = genre.Name,
            });
        }

        public IEnumerable<GenreResponse> GetAll()
        {
            return _genreRepository.GetAll().Select(g => new GenreResponse
            {
                Id = g.Id,
                Name = g.Name,
            });
        }

        public GenreResponse GetById(int id)
        {
            var genre = _genreRepository.GetById(id);
            CheckExistense(id, genre);
            return new GenreResponse
            {
                Id = genre.Id,
                Name = genre.Name,
            };
        }

        public IEnumerable<GenreResponse> GetByMovieId(int movieId)
        {
            return _genreRepository.GetByMovieId(movieId).Select(g => new GenreResponse
            {
                Id = g.Id,
                Name = g.Name,
            }); 
        }

        public bool Update(int id, GenreRequest genre)
        {
            CheckExistense(id);
            Validate(genre);
            return _genreRepository.Update(id, new Genre
            {
                Name = genre.Name,
            });
        }

        public bool Delete(int id)
        {
            CheckExistense(id);
            IsOnlyGenreInAMovie(id);
            return _genreRepository.Delete(id);
        }

        public void CheckExistense(int id)
        {
            var genre = _genreRepository.GetById(id);
            if (genre == null)
            {
                throw new ResourceNotFoundException($"Genre with id {id} not found");
            }
        }

        public void CheckExistense(int id, Genre genre)
        {
            if (genre == null)
            {
                throw new ResourceNotFoundException($"Genre with id {id} not found");

            }
        }

        public void Validate(GenreRequest genre)
        {
            if (genre == null)
            {
                throw new ArgumentNullException("Genre cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(genre.Name))
            {
                throw new InvalidFieldException("Name is required.");
            }
        }

        public void IsOnlyGenreInAMovie(int id)
        {
            var count = _genreRepository.GetSoloMovieCountByGenreId(id);
            if (count > 0)
            {
                throw new InvalidOperationException($"Cannot Delete, This Genre is the only one in a total of {count} movie.");
            }
        }
    }
}
