using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Data;
using DataAcessLayer.Interface;
using ModalLayer.Entities;

namespace DataAcessLayer.Repositary
{
    public class CollabRepo : ICollabRepo
    {
        private readonly FundooDBContext dBContext;
        public CollabRepo(FundooDBContext fundooDBContext)
        {
            this.dBContext = fundooDBContext;
        }
        public Collaborator AddCollaborator(string email, int notesId, int userId)
        {
            email = email.ToLower();

            var note = dBContext.Notes
                .FirstOrDefault(n => n.NotesId == notesId && n.UserId == userId);

            if (note == null)
                throw new Exception("Note not found");

            bool exists = dBContext.Collaborators.Any(c => c.Email == email &&c. NotesId == notesId && c.UserId == userId);

            if (exists)
                throw new Exception("Collaborator already exists");

            var collaborator = new Collaborator
            {
                Email = email,
                NotesId = notesId,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            dBContext.Collaborators.Add(collaborator);
            dBContext.SaveChanges();

            return collaborator;
        }


        public List<Collaborator> GetCollaborator(int notesId, int userId)
        {
            bool noteExists = dBContext.Notes.Any(n => n.NotesId == notesId && n.UserId == userId);

            if (!noteExists)
                throw new Exception("Note not found");

            return dBContext.Collaborators .Where(c => c.NotesId == notesId && c.UserId == userId) .ToList();
        }

        public bool RemoveCollaborator(string email, int notesId, int userId)
        {
            email = email.ToLower();

            var collaborator = dBContext.Collaborators
                .FirstOrDefault(c =>
                    c.Email == email &&
                    c.NotesId == notesId &&
                    c.UserId == userId);

            if (collaborator == null)
                throw new Exception("Collaborator not found");

            dBContext.Collaborators.Remove(collaborator);
            dBContext.SaveChanges();
            return true;
        }

        public bool RemoveCollaboratorByID(int collabId, int userId)
        {
            var collaborator = dBContext.Collaborators
                .FirstOrDefault(c => c.CollaboratorId == collabId && c.UserId == userId);

            if (collaborator == null)
                throw new Exception("Collaborator not found");

            dBContext.Collaborators.Remove(collaborator);
            dBContext.SaveChanges();
            return true;
        }
    }
}
