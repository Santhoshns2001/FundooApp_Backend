using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModalLayer.Entities;

namespace DataAcessLayer.Interface
{
    public interface ILabelRepo
    {
        public Label CreateLabel( int userId, string labelName);
        public List<Label> FetchLabel(int userId);
        public bool DeleteLabel( int userId, int labelId);
        public Label UpdateLabel( int userId, int labelId, string newLabelname);
        public bool AddLabelToNote(int userId, int labelId, int notesid);

        public bool RemoveLabelFromNote(int userId, int labelId, int notesid);
        public List<Notes> GetNotesByLabel(int userId, int labelId);
    }
}
