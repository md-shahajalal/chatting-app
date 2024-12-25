using chat_app.Entities;

namespace chat_app.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
