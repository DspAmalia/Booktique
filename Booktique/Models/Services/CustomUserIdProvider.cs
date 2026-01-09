using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Booktique.Models.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var user = connection.User;

            // 1. Încearcă varianta standard (folosită de admin)
            var id = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // 2. Dacă e null, încearcă varianta ta personalizată (folosită de AmaliaD)
            if (string.IsNullOrEmpty(id))
            {
                id = user?.FindFirst("UserId")?.Value;
            }

            return id;
        }
    }
}
