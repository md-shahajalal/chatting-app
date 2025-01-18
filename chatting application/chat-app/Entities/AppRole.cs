using Microsoft.AspNetCore.Identity;

namespace chat_app.Entities
{
    public class AppRole: IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
