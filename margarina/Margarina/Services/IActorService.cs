using Margarina.Utils;

namespace Margarina.Services
{
    public interface IActorService
    {
        void UpdateActors(long tick);
        void MoveTo(string username, Point destination);
    }
}
