using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ModalLayer.DTOs.Notes
{
   public  class NotesDTO
    {

        public string? Title { get; set; }
        public string? Description { get; set; }

        public string? Colour { get; set; }

        public IFormFile? Image { get; set; }

        public string? Reminder { get; set; }

        public bool? IsPinned { get; set; }
        public bool? IsArchived { get; set; }

    }
}
