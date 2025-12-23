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
    }
}
