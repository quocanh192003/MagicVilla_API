using MagicVilla_Web.Model.dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> UpdateAsync<T>(villaUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
        Task<T> CreateAsync<T>(villaCreateDTO dto);
    }
}
