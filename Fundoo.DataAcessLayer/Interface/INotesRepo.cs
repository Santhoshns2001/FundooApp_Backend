using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModalLayer.DTOs.Notes;
using ModalLayer.Entities;

namespace DataAcessLayer.Interface
{
   public interface INotesRepo
    {
        public Notes CreateNotes(int UserId,NotesDTO modal);
    }
}
