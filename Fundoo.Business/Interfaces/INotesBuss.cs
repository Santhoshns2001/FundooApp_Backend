using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModalLayer.DTOs.Notes;
using ModalLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
   public interface INotesBuss
    {
        public Notes CreateNotes(int userId,NotesDTO modal);

    }
}
