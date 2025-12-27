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
        public Label CreateLabel(int userId, string labelName);
        public List<Label> FetchLabel(int userId);
        public bool DeleteLabel( int userId, int labelid);
        public Label UpdateLabel( int userId, int labelId,string newLabelname);
        public bool AddLabelToNote(int userId,int labelId,int Notesid);
        public bool RemoveLabelFromNote(int userId, int labelId, int notesId);
        public List<Notes> GetNotesByLabel(int userId, int labelId);
    }
}
