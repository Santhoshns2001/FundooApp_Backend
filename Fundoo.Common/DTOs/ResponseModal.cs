using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModalLayer.DTOs
{
    public class ResponseMdl<T>
    {
        public bool IsSuccuss { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

}
