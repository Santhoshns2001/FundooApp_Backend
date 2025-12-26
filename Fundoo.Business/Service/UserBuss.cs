using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using DataAcessLayer.Interface;
using EmailService.Interfaces;
using ModalLayer;
using ModalLayer.DTOs.User;
using ModalLayer.Entities;

namespace BusinessLogicLayer.Service
{
   public class UserBuss : IUserRegisterBuss
    {
        private  readonly IUserRepo _userRepo;
        private readonly IJWTService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IRabbitMqProducer _rabbitMqProducer;
        public UserBuss( IUserRepo userRepo,IJWTService jWTService,IEmailService emailService,IRabbitMqProducer rabbitMqProducer)
        {
            this._userRepo = userRepo;
            this._jwtService = jWTService;
            this._emailService = emailService;
            this._rabbitMqProducer = rabbitMqProducer;
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

                //calling smtp here to send the mail after registration.
              await  _emailService.SendEmailAsync(request.Email, "Registration Successful", $"<h2>Welcome {request.FirstName}</h2>" +
                 "<p>Your account has been created successfully.</p>");
            }

            return user;
        }

        public bool ForgotPassword(string Email)
        {
           var forgotPassword= _userRepo.ForgotPassword(Email);

            if (forgotPassword == null)
                 return false; 
            
                var token = _jwtService.GenerateToken(forgotPassword.Email, forgotPassword.UserId);

            var message = new ForgotPasswordEvent
            {
                Email = forgotPassword.Email,
                Token = token
            };

            _rabbitMqProducer.Publish(message, "forgot-password-queue");
            return true;

        }

        public bool ResetPassword(string email, string password)
        {
           return _userRepo.ResetPassword(email, password);
        }
    }
}
