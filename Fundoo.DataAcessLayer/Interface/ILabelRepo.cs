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
        public Label AddLabel(int notesId, int userId, string labelName);
        public Label FetchLabel(int notesId, int userId, string labelName);
        public bool RemoveLabel(int notesId, int userId, string labelName);
        public Label RenameLabel(int notesId, int userId, string newLabelname, int labelId);
    }
}
