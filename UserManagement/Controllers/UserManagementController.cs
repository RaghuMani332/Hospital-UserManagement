using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.CustomException;
using UserManagement.Dto;
using UserManagement.Entity;
using UserManagement.Enums;
using UserManagement.ExceptionHandler;
using UserManagement.Service.Interface;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserManagementExceptionHandler]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        public UserManagementController(IUserManagementService userManagementService) 
        {
            this._userManagementService = userManagementService;
        }
      
        [HttpPost("create patient")]
        public IActionResult CreatePatient(UserRequestDto user)
        {
            _userManagementService.CreateUser(user,UserRole.PATIENT);
            var v = new ResponceBody<UserRequestDto> 
            {
                Data = user,
                IsSuccess = true,
                Message = "Patient created successfully"
            };

            return Ok(v);

        }



        [HttpPost("create Doctor")]
        public IActionResult CreateDoctor(UserRequestDto user)
        {
            _userManagementService.CreateUser(user, UserRole.DOCTOR);
            var v = new ResponceBody<UserRequestDto>
            {
                Data = user,
                IsSuccess = true,
                Message = "Doctor created successfully"
            };

            return Ok(v);

        }


        [HttpPost("create Admin")]
        public IActionResult CreateAdmin(UserRequestDto user)
        {
            _userManagementService.CreateUser(user, UserRole.ADMIN);
            var v = new ResponceBody<UserRequestDto>
            {
                Data = user,
                IsSuccess = true,
                Message = "Admin created successfully"
            };

            return Ok(v);

        }

        [HttpGet("Login")]
        public String Login(String UserEmail,String Password)
        {
           return _userManagementService.Login(UserEmail, Password);
        }

        [HttpGet("get all")]
       public ResponceBody<UserEntity[]> GetAllUser()
        {
            return new ResponceBody<UserEntity[]>
            {
                Data = _userManagementService.GetAllUser(),
                Message = "Fetched All User",
                IsSuccess=true

            };
        }
        [HttpGet("getbyemail")]
        public UserEntity GetUserByEmail(String email)
        {
            UserEntity e= _userManagementService.GetUserByEmail(email);
            return e == null ? throw new UserNotFoundException("User not found in given mail id" + email) : e;
        }

        [HttpGet("getbyid")]
        public UserEntity GetUserById(int id)
        {
            UserEntity e= _userManagementService.GetUserById(id);
            return e == null ? throw new UserNotFoundException("User not found in given  id" + id) : e;
        }



    }
}
