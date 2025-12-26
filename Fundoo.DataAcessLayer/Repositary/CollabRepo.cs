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

            bool IsUserExists = dBContext.Collaborators.Any(c => c.Email == email);
            if (!IsUserExists)
                throw new Exception("Email does not exist");

            bool IsNotesPresent=dBContext.Notes.Any(n=>n.NotesId == notesId);
            if (!IsNotesPresent)
                throw new Exception("Notes is not present");

            bool IsCollabExists= dBContext.Collaborators.Any(c=>c.Email == email && c.NotesId==notesId && c.UserId ==userId);
            if (IsCollabExists)
                throw new Exception("Email already exists");

            var collaborator = new Collaborator()
            {
                Email = email,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                NotesId = notesId,
                UserId = userId
            };
            dBContext.Collaborators.Add(collaborator);
            dBContext.SaveChanges();
            return collaborator;    

        }

        public bool RemoveCollaborator(string email, int notesId, int userId)
        {
            email = email.ToLower();

            var collaborator = dBContext.Collaborators.FirstOrDefault(c => c.Email == email && c.NotesId == notesId && c.UserId == userId);

            if (collaborator == null)
                throw new Exception("Collaborator Not found");

            dBContext.Remove(collaborator);
            dBContext.SaveChanges();
            return true;

        }
    }
}
