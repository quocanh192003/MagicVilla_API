using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {

        //private readonly IAuthService _authService;
        private readonly IHttpClientFactory _clientFactory;
        public string VillaUrl;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            VillaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public Task<T> LoginAsync<T>(LoginRequestDTO obtToCreate)
        {
            return SendAsync<T>(new APIRequest()
            {
                apiType = SD.ApiType.POST,
                data = obtToCreate,
                Url = VillaUrl + "/api/UsersAuth/Login",

            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDTO obtToCreate)
        {
            return SendAsync<T>(new APIRequest()
            {
                apiType = SD.ApiType.POST,
                data = obtToCreate,
                Url = VillaUrl + "/api/UsersAuth/Register",
            });
        }

        
    }
}
