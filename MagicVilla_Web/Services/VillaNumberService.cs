using MagicVilla_Utility;
using MagicVilla_Web.Models.dto;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;
        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory){
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
            // Debugging: Kiểm tra giá trị
            Console.WriteLine($"Client Factory: {_clientFactory != null}");
            Console.WriteLine($"Villa URL: {villaUrl}");
        }
        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest(){
                apiType = SD.ApiType.POST,
                Url = villaUrl + "/api/VillaNumberAPI",
                data = dto
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest(){
                apiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/VillaNumberAPI/" + id,
                
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest(){
                apiType = SD.ApiType.GET,
                Url = villaUrl + "/api/VillaNumberAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T> (new APIRequest(){
                apiType = SD.ApiType.GET,
                Url = villaUrl + "/api/VillaNumberAPI/" + id
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)
        {
            return SendAsync<T> (new APIRequest(){
                apiType = SD.ApiType.PUT,
                Url = villaUrl + "/api/VillaNumberAPI/" + dto.villaNo,
                data = dto
            });
        }
    }
}
