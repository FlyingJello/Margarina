using System.Threading.Tasks;

namespace Margarina.Services
{
    public interface IPlayerService
    {
        Task<string> Authenticate(string username, string password);
        void Login(string username);
        void Disconnect(string username);
    }
}
