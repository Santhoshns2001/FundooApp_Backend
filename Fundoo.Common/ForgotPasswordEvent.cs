using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModalLayer
{
    public class ForgotPasswordEvent
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string  Token { get; set; }

    }
}
