using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using DataAcessLayer.Data;
using DataAcessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModalLayer;
using ModalLayer.DTOs.User;
using ModalLayer.Entities;

namespace DataAcessLayer.Repositary
{
    public class UserRepo : IUserRepo
    {

        public readonly FundooDBContext context;
        public readonly IConfiguration configuration;
        public UserRepo( FundooDBContext context,IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public User UserRegister(UserRegisterDTO request)
        {
            if (Check(request.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            User userEntity = new User()
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Created = DateTime.Now,
                ChangedAt = DateTime.Now
            };

            context.Users.Add(userEntity);
            context.SaveChanges();

            return userEntity;
        }

        private bool Check(string Email)
        {

            return context.Users.Any(m => m.Email == Email);
        }

     

        public  User LoginUser(LoginDTO mdl)
        {
            if (mdl == null)
                throw new ArgumentNullException(nameof(mdl));

            if (string.IsNullOrWhiteSpace(mdl.Email))
                throw new ArgumentException("Email is required");

            if (string.IsNullOrWhiteSpace(mdl.Password))
                throw new ArgumentException("Password is required");

            var user = context.Users.FirstOrDefault(u => u.Email == mdl.Email);

            if (user == null)
                return null;

            bool isPasswordIsValid = BCrypt.Net.BCrypt.Verify(mdl.Password,user.Password);

            if (!isPasswordIsValid)
            {
                return null;
            }

            return user;
           
        }

        public ForgotPasswordEvent ForgotPassword(string email)
        {
            email = email.ToLower();

            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = context.Users.FirstOrDefault(u=> u.Email == email);

                if (user != null)
                {
                    ForgotPasswordEvent forgotPassword = new ForgotPasswordEvent();

                    forgotPassword.Email = user.Email;
                    forgotPassword.UserId = user.UserId;
                    return forgotPassword;
                }
                else
                {
                    throw new Exception("user does not exist ");
                }
            }
            else
            {
                throw new Exception("Invalid Email Provided ");
            }
        }

        public bool ResetPassword(string Email, string Password)
        { 
            Email = Email.ToLower();
            
            var user = context.Users.FirstOrDefault(u=> u.Email == Email);
            if (user == null)
             return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(Password);
            user.ChangedAt = DateTime.Now;
            context.SaveChanges();
            return true;

        }
    }
}
