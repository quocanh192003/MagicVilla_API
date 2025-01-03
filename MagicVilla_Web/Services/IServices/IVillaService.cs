using MagicVilla_Web.Models.dto;


namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> UpdateAsync<T>(villaUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(villaCreateDTO dto, string token);
    }
}
