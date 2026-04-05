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
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieService _movieService;
        public ReviewService(IReviewRepository reviewRepository, IMovieService movieService)
        {
            _reviewRepository = reviewRepository;
            _movieService = movieService;
        }

        public int Add(int movieId, ReviewRequest review)
        {
            ValidateMovieExistence(movieId);
            Validate(movieId, review);
            return _reviewRepository.Add(new Review
            {
                Message = review.Message,
                MovieId = movieId,
            });
        }

        public IEnumerable<ReviewResponse> GetAll(int movieId)
        {
            ValidateMovieExistence(movieId);
            return _reviewRepository.GetAll(movieId).Select(p => new ReviewResponse
            {
                Id = p.Id,
                Message = p.Message,
                MovieId = p.MovieId,
            });
        }

        public ReviewResponse GetById(int id, int movieId)
        {
            ValidateMovieExistence(movieId);
            var review = _reviewRepository.GetById(id, movieId);
            CheckExistense(id, movieId, review);
            return new ReviewResponse
            {
                Id = review.Id,
                Message = review.Message,
                MovieId = review.MovieId,
            };
        }

        public bool Update(int id, int movieId, ReviewRequest review)
        {
            ValidateMovieExistence(movieId);
            CheckExistense(id, movieId);
            Validate(movieId, review);
            return _reviewRepository.Update(id, movieId, new Review
            {
                Message = review.Message,
                MovieId = movieId,
            });
        }
        public bool Delete(int id, int movieId)
        {
            ValidateMovieExistence(movieId);
            CheckExistense(id, movieId);
            return _reviewRepository.Delete(id, movieId);
        }

        public void CheckExistense(int id, int movieId)
        {
            var review = _reviewRepository.GetById(id, movieId);
            if (review == null)
            {
                throw new ResourceNotFoundException($"Review with id {id} for movie id {movieId} not found");
            }
        }

        public void CheckExistense(int id, int movieId, Review review)
        {
            if (review == null)
            {
                throw new ResourceNotFoundException($"Review with id {id} for movie id {movieId} not found");

            }
        }

        public void Validate(int movieId, ReviewRequest review)
        {
            if (review == null)
            {
                throw new ArgumentNullException("Review cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(review.Message))
            {
                throw new InvalidFieldException("Message is required.");
            }
        }

        public void ValidateMovieExistence(int movieId)
        {
            var movie = _movieService.GetById(movieId);
            if (movie == null)
            {
                throw new ResourceNotFoundException($"Movie with id {movieId} not found.");
            }
        }
    }
}
