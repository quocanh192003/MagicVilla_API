using MagicVilla_Web.Models.dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO obtToCreate);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO obtToCreate);
    }
}
