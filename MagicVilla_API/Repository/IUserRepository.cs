using MagicVilla_API.Model;
using MagicVilla_API.Model.dto;

namespace MagicVilla_API.Repository
{
    public interface IUserRepository
    {
        bool IUniqueUser(string username);
        Task<LoginResponseDTO> Login (LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register (RegisterationRequestDTO registerationRequestDTO);
    }
}
