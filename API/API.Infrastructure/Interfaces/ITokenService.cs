using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        string CreatedToken(AppUser user);
    }
}