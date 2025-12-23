using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModalLayer.DTOs.User;
using ModalLayer.Entities;

namespace DataAcessLayer.Interface
{
public interface IUserRepo
    {
        public User UserRegister(UserRegisterDTO request);

        public User LoginUser(LoginDTO request);
    }
}
