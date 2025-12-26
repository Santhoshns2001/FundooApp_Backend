using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Data;
using DataAcessLayer.Interface;
using Microsoft.Extensions.Configuration;
using ModalLayer.Entities;

namespace DataAcessLayer.Repositary
{
    public class LabelRepo : ILabelRepo
    {

        private readonly FundooDBContext _dbContext;
        private readonly IConfiguration configuration;
        private readonly INotesRepo notesRepo;

        public LabelRepo(FundooDBContext context, IConfiguration config, INotesRepo notesRepo)
        {
            this._dbContext = context;
            this.configuration = config;
            this.notesRepo = notesRepo;
        }
        public Label AddLabel(int notesId, int userId, string labelName)
        {
            bool IsNotesPresent = GetNotesByID(notesId, userId);

            if (IsNotesPresent)
            {

                bool IsLabelIsPresent = _dbContext.Labels.Any(l => l.LabelName == labelName && l.UserId == userId && l.NotesId == notesId);

                if (!IsLabelIsPresent)
                {

                    if (!string.IsNullOrEmpty(labelName))
                    {
                        Label label = new Label()
                        {
                            LabelName = labelName,
                            NotesId = notesId,
                            UserId = userId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                        };
                        _dbContext.Labels.Add(label);
                        _dbContext.SaveChanges();
                        return label;
                    }
                    else
                    {
                        throw new Exception("Label is Empty");
                    }
                }
                else
                {
                    throw new Exception("Label is already present for the note ");
                }
            }
            else
            {
                throw new Exception("No notes is present to add the label");
            }
        }

        public Label FetchLabel(int notesId, int userId, string labelName)
        {
            bool IsNotesPresent = GetNotesByID(notesId, userId);

            if (IsNotesPresent)
            {
                var label = _dbContext.Labels.Where(l => l.LabelName == labelName && l.UserId == userId && l.NotesId == notesId ).FirstOrDefault();
                if (label != null)
                {
                    return label;
                }
                else
                {
                    throw new Exception("Label is not present");
                }

            }
            else
            {
                throw new Exception("Notes is not present ");
            }
        }

        public bool GetNotesByID(int notesId, int userId)
        {
            return _dbContext.Notes.Any(n => n.NotesId == notesId && n.UserId == userId);
        }

        public bool RemoveLabel(int notesId, int userId, string labelName)
        {
            bool notes = GetNotesByID(notesId, userId);

            if (notes)
            {
                var label=_dbContext.Labels.Where(l=>l.NotesId==notesId &&  l.LabelName==labelName && l.UserId==userId).FirstOrDefault();

                if(label != null)
                {
                    _dbContext.Labels.Remove(label);
                    _dbContext.SaveChanges();
                    return true;
                }
                else
                {
                   
                    throw new Exception("Label not found");
                }
            }
            else
            {
                throw new Exception("Notes Not Found");
            }
        }

        public Label RenameLabel(int notesId, int userId, string newLabelname, int labelId)
        {
            var label = _dbContext.Labels.FirstOrDefault(l => l.LabelId == labelId && l.NotesId == notesId && l.UserId == userId);

            if (label == null)
                throw new Exception("Label not found");

            bool duplicateExists = _dbContext.Labels.Any(l =>l.UserId == userId && l.NotesId == notesId && l.LabelId != labelId && l.LabelName == newLabelname);

            if (duplicateExists)
                throw new Exception("Label name already exists");

            label.LabelName = newLabelname;
            label.UpdatedAt = DateTime.Now;

            _dbContext.SaveChanges();
            return label;
        }
    }
}
