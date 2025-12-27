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
        public Label CreateLabel(int userId, string labelName)
        {
            if (string.IsNullOrWhiteSpace(labelName))
                throw new Exception("Label name cannot be empty");

            bool exists = _dbContext.Labels
                .Any(l => l.UserId == userId && l.LabelName == labelName);

            if (exists)
                throw new Exception("Label already exists");

            var label = new Label
            {
                LabelName = labelName,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _dbContext.Labels.Add(label);
            _dbContext.SaveChanges();

            return label;
        }

        public List<Label> FetchLabel(int userId)
        {
            return _dbContext.Labels.Where(l => l.UserId == userId).OrderBy(l => l.LabelName).ToList();
        }

        public bool GetNotesByID(int notesId, int userId)
        {
            return _dbContext.Notes.Any(n => n.NotesId == notesId && n.UserId == userId);
        }

        public Label UpdateLabel(int userId, int labelId, string newLabelName)
        {
            if (string.IsNullOrWhiteSpace(newLabelName))
                throw new Exception("Label name cannot be empty");

            var label = _dbContext.Labels
                .FirstOrDefault(l => l.LabelId == labelId && l.UserId == userId);

            if (label == null)
                throw new Exception("Label not found");

            bool duplicate = _dbContext.Labels.Any(l =>
                l.UserId == userId &&
                l.LabelId != labelId &&
                l.LabelName == newLabelName);

            if (duplicate)
                throw new Exception("Label name already exists");

            label.LabelName = newLabelName;
            label.UpdatedAt = DateTime.Now;

            _dbContext.SaveChanges();
            return label;
        }

        public bool DeleteLabel(int userId, int labelId)
        {
            var label = _dbContext.Labels
                .FirstOrDefault(l => l.LabelId == labelId && l.UserId == userId);

            if (label == null)
                throw new Exception("Label not found");

            var mappings = _dbContext.NoteLabels
                .Where(nl => nl.LabelId == labelId)
                .ToList();

            _dbContext.NoteLabels.RemoveRange(mappings);
            _dbContext.Labels.Remove(label);

            _dbContext.SaveChanges();
            return true;
        }


        public bool AddLabelToNote(int userId, int labelId, int noteId)
        {
            var note = _dbContext.Notes
                .FirstOrDefault(n => n.NotesId == noteId && n.UserId == userId);

            if (note == null)
                throw new Exception("Note not found");

            var label = _dbContext.Labels
                .FirstOrDefault(l => l.LabelId == labelId && l.UserId == userId);

            if (label == null)
                throw new Exception("Label not found");

            bool exists = _dbContext.NoteLabels.Any(nl =>
                nl.NotesId == noteId && nl.LabelId == labelId);

            if (exists)
                throw new Exception("Label already added to this note");

            var noteLabel = new NoteLabel
            {
                NotesId = noteId,
                LabelId = labelId
            };

            _dbContext.NoteLabels.Add(noteLabel);
            _dbContext.SaveChanges();

            return true;
        }

        public bool RemoveLabelFromNote(int userId, int labelId, int noteId)
        {
            var note = _dbContext.Notes
                .FirstOrDefault(n => n.NotesId == noteId && n.UserId == userId);

            if (note == null)
                throw new Exception("Note not found");

            var noteLabel = _dbContext.NoteLabels
                .FirstOrDefault(nl => nl.NotesId == noteId && nl.LabelId == labelId);

            if (noteLabel == null)
                throw new Exception("Label not associated with this note");

            _dbContext.NoteLabels.Remove(noteLabel);
            _dbContext.SaveChanges();

            return true;
        }

        public List<Notes> GetNotesByLabel(int userId, int labelId)
        {
            var label = _dbContext.Labels.FirstOrDefault(l => l.LabelId == labelId && l.UserId == userId);

            if (label == null)
                throw new Exception("Label not found");

            return _dbContext.NoteLabels.Where(nl => nl.LabelId == labelId).Select(nl => nl.Notes).Where(n => n.UserId == userId).ToList();
        }


    }
}
