using BusinessLogicLayer.Interfaces;
using DataAcessLayer.Data;
using Microsoft.AspNetCore.Mvc;
using ModalLayer.DTOs;
using ModalLayer.DTOs.User;
using ModalLayer.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FundooApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private FundooDBContext dbContext;
        private IUserRegisterBuss userBuss;

        public UserController( FundooDBContext context,IUserRegisterBuss userBuss)
        {
            this.dbContext = context;
            this.userBuss = userBuss;
        }



        [HttpPost]
        [Route("register")]
        public async Task<IActionResult>  Register( UserRegisterDTO request)
        {
            var response=  await userBuss.RegisterUser(request);

            if (response != null)
            {
                return Ok(new ResponseMdl<User> { Message="Registered Successfully",IsSuccuss=true,Data=response});
            }
            else
            {
                return BadRequest(new ResponseMdl<User> { Message = "User Failed to register", Data = response });
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginMdl)
        {
            var token = userBuss.LoginUser(loginMdl);

            if (token != null)
            {
                return Ok(new ResponseMdl<string> { IsSuccuss = true, Message = "Login Success", Data = token });
            }
            else
            {
                return BadRequest(new ResponseMdl<string> { IsSuccuss = true, Message = "Unable to login", Data = token });
            }
        }
    }
}
