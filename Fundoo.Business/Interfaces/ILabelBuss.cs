using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModalLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface ILabelBuss
    {
        public Label AddLabel(int notesId,int UserId, string labelName);
        public Label FetchLabel(int notesId, int userId, string labelName);
        public bool RemoveLabel(int notesId, int userId, string labelName);
        public Label RenameLabel(int notesId, int userId, string newLabelname,int labelId);
    }
}
