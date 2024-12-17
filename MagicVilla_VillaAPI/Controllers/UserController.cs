using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/UsersAuth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        protected APIResponse _response;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _response = new APIResponse();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginReqDto)
        {
            var loginRes = await _userRepo.Login(loginReqDto);
            
            if (loginRes.User == null || string.IsNullOrEmpty(loginRes.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginRes;

            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto registerationReqDto)
        {
            bool isUnique = _userRepo.IsUniqueUser(registerationReqDto.UserName);

            if (!isUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            var user = await _userRepo.Register(registerationReqDto);

            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering");
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
