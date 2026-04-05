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
    public class ProducerService : IProducerService
    {
        private readonly IProducerRepository _producerRepository;
        public ProducerService(IProducerRepository producerRepository)
        {
            _producerRepository = producerRepository;
        }

        public int Add(ProducerRequest producer)
        {
            Validate(producer);
            return _producerRepository.Add(new Producer
            {
                Name = producer.Name,
                Gender = producer.Gender,
                DateOfBirth = producer.DateOfBirth,
                Bio = producer.Bio,
            });
        }

        public IEnumerable<ProducerResponse> GetAll()
        {
            return _producerRepository.GetAll().Select(p => new ProducerResponse
            {
                Id = p.Id,
                Name = p.Name,
                Gender = p.Gender,
                DateOfBirth = p.DateOfBirth,
                Bio = p.Bio,
            });
        }

        public ProducerResponse GetById(int id)
        {
            var producer = _producerRepository.GetById(id);
            CheckExistense(id, producer);
            return new ProducerResponse
            {
                Id = producer.Id,
                Name = producer.Name,
                Gender = producer.Gender,
                DateOfBirth = producer.DateOfBirth,
                Bio = producer.Bio,
            };
        }

        public bool Update(int id, ProducerRequest producer)
        {
            CheckExistense(id);
            Validate(producer);
            return _producerRepository.Update(id, new Producer
            {
                Name = producer.Name,
                Gender = producer.Gender,
                DateOfBirth = producer.DateOfBirth,
                Bio = producer.Bio,
            });
        }
        public bool Delete(int id)
        {
            CheckExistense(id);
            return _producerRepository.Delete(id);
        }

        public void CheckExistense(int id)
        {
            var producer = _producerRepository.GetById(id);
            if (producer == null)
            {
                throw new ResourceNotFoundException($"Producer with id {id} not found");
            }
        }

        public void CheckExistense(int id, Producer producer)
        {
            if (producer == null)
            {
                throw new ResourceNotFoundException($"Producer with id {id} not found");

            }
        }

        public void Validate(ProducerRequest producer)
        {
            if (producer == null)
            {
                throw new ArgumentNullException("Producer cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(producer.Name))
            {
                throw new InvalidFieldException("Name is required.");
            }
            if (string.IsNullOrWhiteSpace(producer.Bio))
            {
                throw new InvalidFieldException("Bio is required.");
            }
            if (string.IsNullOrWhiteSpace(producer.Gender))
            {
                throw new InvalidFieldException("Gender is required.");
            }
            if (producer.DateOfBirth > DateTime.Now)
            {
                throw new InvalidFieldException("DateOfBirth cannot be greater than current date.");
            }
        }
    }
}
