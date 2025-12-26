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
        public Task<Notes> CreateNotes(int userId,NotesDTO modal);

        public List<Notes> FetchNotesByTitleAndDescrptiion(string title, string description);

        public Task<List<Notes>> GetAllNotes(int UserId);

        public Notes GetNotesById(int notesId);

        public Task<Notes> UpdateNotes(int userId,int NotesId,NotesDTO modal);

        public bool ToggleArchiveNotes(int userId, int NotesId);

        public bool ToggleTrashNotes(int userId, int NotesId);
    }
}
