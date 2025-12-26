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

        public Label AddLabel(int notesId,int UserId, string labelName)
        {
            return labelRepo.AddLabel(notesId, UserId, labelName);
        }

        public Label FetchLabel(int notesId, int userId, string labelName)
        {
            return labelRepo.FetchLabel(notesId, userId, labelName);
        }

        public bool RemoveLabel(int notesId, int userId, string labelName)
        {
            return labelRepo.RemoveLabel(notesId, userId, labelName);
        }

        public Label RenameLabel(int notesId, int userId, string newLabelname, int labelId)
        {
            return labelRepo.RenameLabel(notesId, userId, newLabelname,labelId);
        }
    }
}
