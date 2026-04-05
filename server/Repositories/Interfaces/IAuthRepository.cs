using IMDB.Models.Db;

namespace IMDB.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        bool Add(User user);
        User GetbyEmail(string email);
    }
}