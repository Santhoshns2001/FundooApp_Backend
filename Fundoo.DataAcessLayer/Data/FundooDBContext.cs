using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModalLayer.Entities;

namespace DataAcessLayer.Data
{
   public class FundooDBContext :DbContext
    {
        public FundooDBContext( DbContextOptions<FundooDBContext> context) :base(context) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Label> Labels { get; set; }

        public DbSet<Collaborator> Collaborators { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Label>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Label>()
                .HasOne<Notes>()
                .WithMany()
                .HasForeignKey(l => l.NotesId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
