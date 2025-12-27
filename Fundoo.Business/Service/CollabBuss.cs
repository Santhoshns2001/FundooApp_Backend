using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using DataAcessLayer.Interface;
using ModalLayer.Entities;

namespace BusinessLogicLayer.Service
{
    public class CollabBuss : ICollabBuss
    {
        private ICollabRepo collabRepo;

        public CollabBuss(ICollabRepo collabRepo)
        {
            this.collabRepo = collabRepo;
        }

        public Collaborator AddCollaborator(string email, int notesId, int userId)
        {
            return collabRepo.AddCollaborator(email, notesId, userId);
        }

        public List<Collaborator> GetCollaborator(int notesId, int userId)
        {
            return collabRepo.GetCollaborator(notesId, userId);
        }

        public bool RemoveCollaborator(string email, int notesId, int userId)
        {
            return collabRepo.RemoveCollaborator(email, notesId, userId);
        }

        public bool RemoveCollaboratorByID(int collabId, int userId)
        {
            return collabRepo.RemoveCollaboratorByID(collabId, userId);
        }
    }
}
