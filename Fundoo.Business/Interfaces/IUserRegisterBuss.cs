using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModalLayer.DTOs;
using ModalLayer.DTOs.User;
using ModalLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
   public interface IUserRegisterBuss
    {
        public Task<User> RegisterUser(UserRegisterDTO request); 

        public string LoginUser(LoginDTO loginDTO);


    }
}
