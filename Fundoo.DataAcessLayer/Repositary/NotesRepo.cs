using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Data;
using DataAcessLayer.Interface;
using Microsoft.Extensions.Configuration;
using ModalLayer.DTOs.Notes;
using ModalLayer.Entities;

namespace DataAcessLayer.Repositary
{
    public class NotesRepo : INotesRepo
    {
        private readonly IConfiguration configuration;
        private readonly FundooDBContext context;

        public NotesRepo(IConfiguration configuration,FundooDBContext context)
        {
            this.configuration = configuration; 
            this.context = context; 
        }

        public async Task<Notes> CreateNotes(int UserId, NotesDTO modal)
        {
                if(modal == null)
                    throw new ArgumentNullException(nameof(modal));

                Notes notes = new Notes()
                {
                    Title = modal.Title,
                    Description= modal.Description,
                    UserId=UserId,
                    CreatedAt= DateTime.Now,
                    UpdatedAt= DateTime.Now,
                    Colour=modal.Colour,
                    Reminder=modal.Reminder,
                   // Image=modal.Image,
                    IsPin=modal.IsPinned,
                   IsArchive=modal.IsArchived

                };
                context.Notes.Add(notes);
              await  context.SaveChangesAsync();
                return notes;
        }


        //for searching purpose
        public List<Notes> FetchNotesByTitleAndDescrptiion(string Title,string Description)
        {
            IQueryable<Notes> query = context.Notes;

            if (!string.IsNullOrEmpty(Title))
            {
                query=query.Where(x=> x.Title.Contains( Title));
            }

            if (!string.IsNullOrEmpty(Description))
            {
                query=query.Where(x=> x.Description.Contains( Description));
            }

            return query.ToList();
        }

        public  Notes GetNotesById(int NotesId)
        {
            var notes= context.Notes.Where(n => n.NotesId == NotesId).FirstOrDefault();
            if(notes == null)
            {
                throw new Exception("Notes Not Found");
            }
            return notes;
        }


        public  List<Notes> GetAllNotes(int UserId)
        {
            return  context.Notes.Where(n=> n.UserId==UserId).ToList();
        }

        public async Task<Notes> UpdateNotes(int UserId,int NotesId, NotesDTO modal)
        {
            if (modal == null)
                throw new ArgumentNullException(nameof(modal));

            var existingNote = context.Notes.FirstOrDefault(n => n.NotesId == NotesId && n.UserId==UserId);

            if (existingNote != null)
            {
                existingNote.Description = modal.Description;
                existingNote.Title = modal.Title;
                existingNote.Colour = modal.Colour;
               // existingNote.Image = modal.Image;
                existingNote.Reminder= modal.Reminder;
                existingNote.IsPin = modal.IsPinned;
                existingNote.IsArchive = modal.IsArchived;
                existingNote.UpdatedAt= DateTime.Now;
                context.SaveChanges();
            }
            else
            {
                 throw new Exception("Note not Fount");
            }


            return existingNote;            
        }

        public bool ToggleArchiveNotes(int UserId, int NotesId)
        {

            var note = GetNotesById(NotesId);
            if (note != null)
            {
                if (note.IsPin==null ||(bool)note.IsPin)
                    note.IsPin = false;
                note.IsArchive = !note.IsArchive.GetValueOrDefault();
                note.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;

            }
            else
            {
                throw new Exception("Not Found ");
            }
        }

                public bool ToggleTrashNotes(int UserId, int NotesId)
                {
                   var note = GetNotesById(NotesId);
                        if (note != null)
                            {
                              note.IsTrash = !note.IsTrash.GetValueOrDefault();
                              note.UpdatedAt = DateTime.Now;
                              context.SaveChanges();
                              return true;
                         }
                          else
                          {
                throw new Exception("Note is not found for a requested id : " + NotesId);
                          }
        }
    }
}
