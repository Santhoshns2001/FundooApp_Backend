using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModalLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICollabBuss
    {
       public  Collaborator AddCollaborator(string email, int notesId, int userId);
       public List<Collaborator> GetCollaborator(int notesId, int userId);
        public bool RemoveCollaborator(string email, int notesId, int userId);
        public bool RemoveCollaboratorByID(int collabId, int userId);
    }
}
