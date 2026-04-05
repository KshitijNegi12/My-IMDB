using IMDB.Models.Request;
using System.Threading.Tasks;

namespace IMDB.Services.Interfaces
{
    public interface IAuthService
    {
        bool SignIn(SignInRequest userDetails);
        string LogIn(LogInRequest userDetails);
        string GenerateToken(string email);
    }
}