using System.Threading.Tasks;
using Entities.DTO_s;
using Microsoft.AspNetCore.Identity;

namespace Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto);
        Task<string> CreateToken();
    }
}
