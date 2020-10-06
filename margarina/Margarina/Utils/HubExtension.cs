using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace Margarina.Utils
{
    public static class HubExtension
    {
        public static string GetUsername(this Hub hub)
        {
            return hub.Context.User.Identity.Name;
        }
    }
}
