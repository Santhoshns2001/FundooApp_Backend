using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using DataAcessLayer.Interface;
using ModalLayer.DTOs.User;
using ModalLayer.Entities;

namespace BusinessLogicLayer.Service
{
   public class UserBuss : IUserRegisterBuss
    {
        private  readonly IUserRepo _userRepo;
        private readonly IJWTService _jwtService;
        private readonly IEmailService _emailService;

        public UserBuss( IUserRepo userRepo,IJWTService jWTService,IEmailService emailService)
        {
            this._userRepo = userRepo;
            this._jwtService = jWTService;
            this._emailService = emailService;
        }

        public string LoginUser(LoginDTO loginDTO)
        {
           var user= _userRepo.LoginUser(loginDTO);

            if (user == null) { 
                return null;
            }

          return  _jwtService.GenerateToken(user.Email,user.UserId);
        }

        public async Task<User> RegisterUser(UserRegisterDTO request)
        {
            var user= _userRepo.UserRegister(request);

            if (user != null)
            {
              await  _emailService.SendEmailAsync(request.Email, "Registration Successful", $"<h2>Welcome {request.FirstName}</h2>" +
                 "<p>Your account has been created successfully.</p>");
            }

            return user;
        }




    }
}
