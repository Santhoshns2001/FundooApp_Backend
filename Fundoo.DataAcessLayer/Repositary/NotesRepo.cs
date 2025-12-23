using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Data;
using DataAcessLayer.Interface;
using Microsoft.Extensions.Configuration;
using ModalLayer.DTOs.Notes;
using ModalLayer.Entities;

namespace DataAcessLayer.Repositary
{
    public class NotesRepo : INotesRepo
    {
        private readonly IConfiguration configuration;
        private readonly FundooDBContext context;

        public NotesRepo(IConfiguration configuration,FundooDBContext context)
        {
            this.configuration = configuration; 
            this.context = context; 
        }

        public Notes CreateNotes(int UserId, NotesDTO modal)
        {
                if(modal == null)
                    throw new ArgumentNullException(nameof(modal));

                Notes notes = new Notes()
                {
                    Title = modal.Title,
                    Description= modal.Description,
                    UserId=UserId,
                    CreatedAt= DateTime.Now,
                    UpdatedAt= DateTime.Now
                };
                context.Notes.Add(notes);
                context.SaveChanges();
                return notes;
        }
    }
}
