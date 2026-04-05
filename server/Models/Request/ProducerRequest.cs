using System;

namespace IMDB.Models.Request
{
    public class ProducerRequest
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Bio { get; set; }
    }
}
