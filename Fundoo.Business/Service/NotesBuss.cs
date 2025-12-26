using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using DataAcessLayer.Interface;
using Microsoft.Extensions.Caching.Distributed;
using ModalLayer.DTOs.Notes;
using ModalLayer.Entities;
using Newtonsoft.Json;

namespace BusinessLogicLayer.Service
{
    public class NotesBuss : INotesBuss
    {
        private readonly INotesRepo notesRepo;
        private readonly IDistributedCache _cache;


        public NotesBuss(INotesRepo notes,IDistributedCache cache)
        {
            this.notesRepo= notes;
            this._cache= cache;
        }
        public async Task<Notes> CreateNotes(int UserId,NotesDTO modal)
        {
            string cacheKey = $"GET_ALL_NOTES_FUNDOO_{UserId}";
            await _cache.RemoveAsync(cacheKey);

            return await notesRepo.CreateNotes(UserId, modal);
        }

       public List<Notes> FetchNotesByTitleAndDescrptiion(string title, string description)
        {
            return notesRepo.FetchNotesByTitleAndDescrptiion(title,description);
        }

        public Notes GetNotesById(int NotesId)
        {
            return notesRepo.GetNotesById(NotesId);
        }

        public async Task<List<Notes>> GetAllNotes(int UserId)
        {
            String CacheKey = $"GET_ALL_NOTES_FUNDOO_{UserId}";

            var notes = await _cache.GetStringAsync(CacheKey);

            if (notes !=null)
            {
                return JsonConvert.DeserializeObject<List<Notes>>(notes);
            }
           
                List <Notes> noteslist=notesRepo.GetAllNotes(UserId);

                if (noteslist != null)
                {
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                    await _cache.SetStringAsync(CacheKey, JsonConvert.SerializeObject(noteslist), options);
                }


            return noteslist;
             
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
