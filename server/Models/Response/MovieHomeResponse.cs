using System.Collections.Generic;

namespace IMDB.Models.Response
{
    public class MovieHomeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Plot { get; set; }
        public string CoverImage { get; set; }
    }
}
