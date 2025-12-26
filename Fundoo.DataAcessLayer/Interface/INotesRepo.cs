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
        public Task<Notes> CreateNotes(int UserId,NotesDTO modal);

        public List<Notes> FetchNotesByTitleAndDescrptiion(string title,string descr);

        public Notes GetNotesById(int NotesId);

        public List<Notes> GetAllNotes(int Userid);

        public Task<Notes> UpdateNotes(int  UserId,int NotesId,NotesDTO modal);

        public bool ToggleArchiveNotes(int NotesId, int Userid);

        public bool ToggleTrashNotes(int UserId, int NotesId);
    }
}
