
using Microsoft.AspNetCore.Identity;

namespace PUI.Identity.Models
{
    
    public class Usuario
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

    }

}
