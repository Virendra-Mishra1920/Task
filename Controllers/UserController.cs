using CRUD_API_Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

using CRUD_API_Assignment.Services.UserService;

namespace CRUD_API_Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
            
        }

        [HttpGet("GetAllUsers")]
        public async Task< ActionResult<ServiceResponse<List<GetUserResponseDto>>>> GetUsers()
        {
            return Ok( await _userService.GetAllUsers());
        }

        [HttpGet()]
        public async Task<ActionResult<ServiceResponse< GetUserResponseDto>>> GetUserById(string id=null)
        {
            if(string.IsNullOrEmpty(id))
            return BadRequest("Invalid userid");
            var response=await _userService.GetUserById(id);
            if(response.Data is null)
            return NotFound(response);
            return Ok(response);
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<ServiceResponse<string>>> AddUser(AddUserResquestDto user)
        {
            if(string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password)  || user.Age==0 || user.Hobbies.Count<0  )
            return BadRequest("Invalid user details");

            var response=await _userService.AddUser(user,user.Password);
            return Ok(response);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginRequestDto request)
        {
            if(string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password)  )
            return BadRequest("Invalid user details");

            var response=await _userService.Login(request.UserName,request.Password);
            return Ok(response);

        }

        //  [HttpGet("{id}")]
        // private async Task<ActionResult<ServiceResponse< GetUserResponseDto>>> Create(string? id)
        // {
        //     return await _userService.GetUserById(id);
        // }

    
       [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetUserResponseDto>>> UpdateUser(UpdateUserRequestDto updatedUser)
        {
            var response=await _userService.UpdateUser(updatedUser);
            if(response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete()]

        public async Task<ActionResult<ServiceResponse<List<GetUserResponseDto>>>> DeleteUser(string id=null)
        {
             if(string.IsNullOrEmpty(id))
            return BadRequest("Invalid userid");
            var response=await _userService.DeleteUser(id);
            if(response.Success==false)
            return NotFound(response);
        
            return NoContent();
        }

    }
}