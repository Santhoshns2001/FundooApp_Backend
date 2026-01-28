using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModalLayer.DTOs.Collaborator
{
    public class AddCollaboratorRequest
    {
        public string Email { get; set; }
        public int NotesId { get; set; }
    }
}
