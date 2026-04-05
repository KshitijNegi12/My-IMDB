using System;

namespace IMDB.Models.Response
{
    public class ActorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Bio { get; set; }
    }
}
