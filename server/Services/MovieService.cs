using IMDB.Exceptions;
using IMDB.Models.Db;
using IMDB.Models.Request;
using IMDB.Models.Response;
using IMDB.Repositories.Interfaces;
using IMDB.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IMDB.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorService _actorService;
        private readonly Supabase.Client _supabase;
        private readonly IGenreService _genreService;
        private readonly IProducerService _producerService;

        public MovieService(IMovieRepository movieRepository, IGenreService genreService, IProducerService producerService, IActorService actorService, Supabase.Client supabase)
        {
            _movieRepository = movieRepository;
            _actorService = actorService;
            _supabase = supabase;
            _genreService = genreService;
            _producerService = producerService;
        }

        public int Add(MovieRequest movie)
        {
            Validate(movie);
            var actorIds = string.Join(",", movie.ActorIds);
            var genreIds = string.Join(",", movie.GenreIds);

            return _movieRepository.Add(actorIds, genreIds, new Movie
            {
                Name = movie.Name,
                YearOfRelease = movie.YearOfRelease,
                Plot = movie.Plot,
                ProducerId = movie.Producer,
                CoverImage = movie.CoverImage,
            });
        }

        public IEnumerable<object> GetAll(int? year, bool home)
        {
            if (!home)
            {
                return _movieRepository.GetAll(year).Select(m => new MovieResponse
                {
                    Id = m.Id,
                    Name = m.Name,
                    YearOfRelease = m.YearOfRelease,
                    Plot = m.Plot,
                    Actors = _actorService.GetByMovieId(m.Id),
                    Genres = _genreService.GetByMovieId(m.Id),
                    Producer = _producerService.GetById(m.ProducerId),
                    CoverImage = m.CoverImage,
                });
            }
            return _movieRepository.GetHomeContent().Select(m => new MovieHomeResponse
            {
                Id = m.Id,
                Name = m.Name,
                Plot = m.Plot,
                CoverImage = m.CoverImage,
            });
        }

        public MovieResponse GetById(int id)
        {
            var movie = _movieRepository.GetById(id);
            CheckExistense(id, movie);
            return new MovieResponse
            {
                Id = movie.Id,
                Name = movie.Name,
                YearOfRelease = movie.YearOfRelease,
                Plot = movie.Plot,
                Actors = _actorService.GetByMovieId(movie.Id),
                Genres = _genreService.GetByMovieId(movie.Id),
                Producer = _producerService.GetById(movie.ProducerId),
                CoverImage = movie.CoverImage,
            };
        }

        public bool Update(int id, MovieRequest movie)
        {
            CheckExistense(id);
            Validate(movie);
            return _movieRepository.Update(id, movie.ActorIds, movie.GenreIds, new Movie
            {
                Name = movie.Name,
                YearOfRelease = movie.YearOfRelease,
                Plot = movie.Plot,
                ProducerId = movie.Producer,
                CoverImage = movie.CoverImage,
            });
        }

        public async Task<string> UpdateCover(int id, IFormFile img)
        {
            if (img == null || img.Length == 0)
            {
                throw new InvalidFieldException("No Image uploaded.");
            }

            var allowedMimeTypes = new List<string> { "image/jpeg", "image/png", "image/webp" };
            if (!allowedMimeTypes.Contains(img.ContentType))
            {
                throw new InvalidFieldException("Invalid file type. Only JPG, JPEG, PNG, and WEBP are allowed.");
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(img.FileName).ToLower()}";

            using var memoryStream = new MemoryStream();
            await img.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var existingCoverUrl = _movieRepository.GetCoverById(id);
            if (!string.IsNullOrEmpty(existingCoverUrl))
            {
                var fileNameToDelete = Path.GetFileName(existingCoverUrl);
                await _supabase.Storage.From("movie-covers").Remove(new List<string> { fileNameToDelete });
            }

            await _supabase.Storage
                .From("movie-covers")
                .Upload(memoryStream.ToArray(), fileName, new Supabase.Storage.FileOptions { Upsert = true });

            var fileUrl = _supabase.Storage.From("movie-covers").GetPublicUrl(fileName);
            _movieRepository.UpdateCover(id, fileUrl);
            return fileUrl;
        }

        public bool Delete(int id)
        {
            CheckExistense(id);
            return _movieRepository.Delete(id);
        }

        public void CheckExistense(int id)
        {
            var movie = _movieRepository.GetById(id);
            if (movie == null)
            {
                throw new ResourceNotFoundException($"Movie with id {id} not found");
            }
        }

        public void CheckExistense(int id, Movie movie)
        {
            if (movie == null)
            {
                throw new ResourceNotFoundException($"Movie with id {id} not found");

            }
        }

        public void Validate(MovieRequest movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException("Movie cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(movie.Name))
            {
                throw new InvalidFieldException("Name is required.");
            }
            if (movie.YearOfRelease > DateTime.Now.Year)
            {
                throw new InvalidFieldException("YearOfRelease cannot be greater than current year.");
            }
            if (string.IsNullOrWhiteSpace(movie.Plot))
            {
                throw new InvalidFieldException("Plot is required.");
            }
            if (string.IsNullOrWhiteSpace(movie.CoverImage))
            {
                throw new InvalidFieldException("CoverImage is required.");
            }
            ValidateActorList(movie.YearOfRelease, movie.ActorIds);
            ValidateGenreList(movie.GenreIds);
            ValidateProducer(movie.YearOfRelease, movie.Producer);
        }

        public void ValidateActorList(int YearOfRelease, IEnumerable<int> actorIds)
        {
            if (actorIds == null || !actorIds.Any())
            {
                throw new InvalidFieldException("At least one actor is required.");
            }

            var actorsPresent = _actorService.GetAll();
            var IsAnyInvalidId = actorIds.Any(id => !actorsPresent.Any(a => a.Id == id));
            if (IsAnyInvalidId)
            {
                throw new InvalidFieldException("One or more actor IDs are invalid.");
            }

            var dobInvalids = actorsPresent.
                    Where(a => actorIds.Contains(a.Id)).
                    Any(a => a.DateOfBirth.Year > YearOfRelease);
            if (dobInvalids)
            {
                throw new InvalidFieldException("One or more actors dateOfBirth is greater than movie release.");
            }
        }

        public void ValidateGenreList(IEnumerable<int> genreIds)
        {
            if (genreIds == null || !genreIds.Any())
            {
                throw new InvalidFieldException("At least one genre is required.");
            }

            var genresPresent = _genreService.GetAll();
            var IsAnyInvalidId = genreIds.Any(id => !genresPresent.Any(g => g.Id == id));
            if (IsAnyInvalidId)
            {
                throw new InvalidFieldException("One or more genre IDs are invalid.");
            }
        }

        public void ValidateProducer(int yearOfRelease, int producerId)
        {
            if (producerId == 0)
            {
                throw new InvalidFieldException("Producer is required.");
            }
            var producersPresent = _producerService.GetAll();
            var IsProducerPresent = producersPresent.SingleOrDefault(id => id.Id == producerId);
            if (IsProducerPresent == null)
            {
                throw new InvalidFieldException("Producer ID is invalid.");
            }
            if (IsProducerPresent.DateOfBirth.Year > yearOfRelease)
            {
                throw new InvalidFieldException("Producer dateOfBirth is greater than movie release.");
            }

        }
    }
}