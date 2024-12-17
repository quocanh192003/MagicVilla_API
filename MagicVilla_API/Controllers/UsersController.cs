using MagicVilla_API.Model;
using MagicVilla_API.Model.dto;
using MagicVilla_API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/UsersAuth")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected APIReponse _apiResponse;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            this._apiResponse = new();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Username or password is incorrect!");
                return BadRequest(_apiResponse);
            } 
            
            _apiResponse.Status = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true; ;
            _apiResponse.Result = loginResponse;
            return Ok(_apiResponse);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            bool ifUserNameUnique = _userRepository.IUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess=false;
                _apiResponse.ErrorMessages.Add("Usernam already exists!");
                return BadRequest(_apiResponse);
            }

            var user = _userRepository.Register(model);
            if(user == null)
            {
                _apiResponse.Status = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Error while registering!");
                return BadRequest(_apiResponse);
            }
            _apiResponse.Status= HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
