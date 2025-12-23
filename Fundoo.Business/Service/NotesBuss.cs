using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using DataAcessLayer.Interface;
using ModalLayer.DTOs.Notes;
using ModalLayer.Entities;

namespace BusinessLogicLayer.Service
{
    public class NotesBuss : INotesBuss
    {
        private readonly INotesRepo notesRepo;

        public NotesBuss(INotesRepo notes)
        {
            this.notesRepo= notes;
        }
        public Notes CreateNotes(int UserId,NotesDTO modal)
        {
            return notesRepo.CreateNotes(UserId, modal);
        }
    }
}
