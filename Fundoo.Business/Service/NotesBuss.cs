using BusinessLogicLayer.Interfaces;
using DataAcessLayer.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ModalLayer.DTOs.Notes;
using ModalLayer.Entities;
using Newtonsoft.Json;

namespace BusinessLogicLayer.Service
{
    public class NotesBuss : INotesBuss
    {
        private readonly INotesRepo notesRepo;
        private readonly IDistributedCache _cache;
        private readonly ILogger<NotesBuss> _logger;


        public NotesBuss(INotesRepo notes,IDistributedCache cache,ILogger<NotesBuss> logger)
        {
            this.notesRepo= notes;
            this._cache= cache;
            this._logger= logger;
        }
        public async Task<Notes> CreateNotes(int userId, NotesDTO model)
        {
            string cacheKey = $"GET_ALL_NOTES_FUNDOO_{userId}";

            try
            {
                await _cache.RemoveAsync(cacheKey);
            }
            catch (Exception ex)
            {
                // Log only — DO NOT fail note creation
                 _logger.LogWarning(ex, "Cache remove failed");
            }

            return await notesRepo.CreateNotes(userId, model);
        }


        public List<Notes> FetchNotesByTitleAndDescrptiion(string title, string description)
        {
            return notesRepo.FetchNotesByTitleAndDescrptiion(title,description);
        }

        public Notes GetNotesById(int NotesId)
        {
            return notesRepo.GetNotesById(NotesId);
        }

        public async Task<List<Notes>> GetAllNotes(int userId)
        {
            string cacheKey = $"GET_ALL_NOTES_FUNDOO_{userId}";
            List<Notes>? notesList = null;

            try
            {
                var cachedNotes = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedNotes))
                {
                    return JsonConvert.DeserializeObject<List<Notes>>(cachedNotes)!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis GET failed. Falling back to DB.");
            }

            notesList = notesRepo.GetAllNotes(userId);

            if (notesList == null || notesList.Count == 0)
                return notesList;

            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                };

                await _cache.SetStringAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(notesList),
                    options
                );
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis SET failed.");
            }

            return notesList;
        }


        public async Task<Notes> UpdateNotes(int userId,int NotesId, NotesDTO modal)
        {
            string cache= $"GET_ALL_NOTES_FUNDOO_{userId}";

             await _cache.RemoveAsync(cache);

            return await notesRepo.UpdateNotes(userId,NotesId, modal);
        }

        public bool ToggleArchiveNotes(int UserId, int NotesId)
        {
            return notesRepo.ToggleArchiveNotes(UserId, NotesId);
        }

        public bool ToggleTrashNotes(int userId, int NotesId)
        {
            return notesRepo.ToggleTrashNotes(userId, NotesId);
        }
    }
}
