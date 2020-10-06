using System.Threading.Tasks;

namespace Margarina.Services
{
    public interface IPlayerService
    {
        Task<string> Create(string username, string password);
        string Authenticate(string username, string password);
        void Login(string username);
        void Disconnect(string username);
    }
}
