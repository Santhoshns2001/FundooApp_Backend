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
    

    
    public class LabelBuss : ILabelBuss
    {
        private readonly ILabelRepo labelRepo;

        public LabelBuss(ILabelRepo repo)
        {
            this.labelRepo = repo;
        }

        public Label CreateLabel(int UserId, string labelName)
        {
            return labelRepo.CreateLabel( UserId, labelName);
        }

        public List<Label> FetchLabel(int userId)
        {
            return labelRepo.FetchLabel(userId);
        }

        public bool DeleteLabel( int userId, int labelId)
        {
            return labelRepo.DeleteLabel( userId, labelId);
        }

        public Label UpdateLabel( int userId, int labelId, string newLabelname)
        {
            return labelRepo.UpdateLabel( userId, labelId, newLabelname);
        }

        public bool AddLabelToNote(int userId, int labelId, int Notesid)
        {
            return labelRepo.AddLabelToNote(userId,labelId,Notesid);
        }

        public bool RemoveLabelFromNote(int userId, int labelId, int notesId)
        {
            return labelRepo.RemoveLabelFromNote( userId, labelId, notesId);
        }

        public List<Notes> GetNotesByLabel(int userId, int labelId)
        {
            return labelRepo.GetNotesByLabel(userId,labelId);
        }
    }
}
