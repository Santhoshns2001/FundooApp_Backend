using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using DataAcessLayer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModalLayer;
using ModalLayer.DTOs;
using ModalLayer.DTOs.User;
using ModalLayer.Entities;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FundooApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private FundooDBContext dbContext;
        private IUserRegisterBuss userBuss;
        private readonly ILogger<UserController> _logger;


        public UserController( FundooDBContext context,IUserRegisterBuss userBuss,ILogger<UserController> logger)
        {
            this.dbContext = context;
            this.userBuss = userBuss;
            this._logger = logger;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO request)
        {
            var response = await userBuss.RegisterUser(request);

            _logger.LogInformation("User registered successfully with Email {Email}",request.Email);    

            return Ok(new ResponseMdl<User>{Message = "Registered Successfully",IsSuccuss = true,Data = response});
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginMdl)
        {
            var token = userBuss.LoginUser(loginMdl);

            if (token != null)
            {
                _logger.LogInformation("User logged in successfully with Email {Email}", loginMdl.Email);

                return Ok(new ResponseMdl<string> { IsSuccuss = true, Message = "Login Success", Data = token });
            }
            else
            {
                _logger.LogInformation("User logged in Failed with Email {Email}", loginMdl.Email);
                return BadRequest(new ResponseMdl<string> { IsSuccuss = true, Message = "Unable to login" });
            }
        }

       
        [HttpGet]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(string Email)
        {

         bool res=  userBuss.ForgotPassword(Email);

            if (res)
            {
              
                return Ok(new ResponseMdl<string> { IsSuccuss = true, Message = "Login Success", Data = "Reset link has sent to the mail" });
            }
            else
            {

                return BadRequest(new ResponseMdl<string> { IsSuccuss = true, Message = "Unable to login", Data = "something went wrong" });
            }

        }

        [Authorize]
        [HttpPut]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(string newPassword,string confirmPassword)
        {

            var userEmailClaim = User.FindFirst(ClaimTypes.Email);

            if (userEmailClaim == null)
                return Unauthorized("Invalid token");

            string Email = userEmailClaim.Value;

            if (newPassword != newPassword)
                return BadRequest(new ResponseMdl<string>() { IsSuccuss = false, Message = "password and confirm password does not match", Data = "please check the password and confirm password " });

           
            if (userBuss.ResetPassword(Email,newPassword))
            {
                return Ok(new ResponseMdl<string> { IsSuccuss = true, Message = "PasswordUpdated  Successfully", Data = "Password Reset is Success" });
            }
            else
            {
                return BadRequest(new ResponseMdl<string> { IsSuccuss = true, Message = "Unable to login",Data="Password reset is Failed" });
            }

        }



    }
}
