using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModalLayer.Entities;

namespace DataAcessLayer.Data
{
    public class FundooDBContext : DbContext
    {
        public FundooDBContext(DbContextOptions<FundooDBContext> context)
            : base(context)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<NoteLabel> NoteLabels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → Notes (1:N)
            modelBuilder.Entity<Notes>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Labels (1:N) - change to Restrict to avoid multiple cascade paths
            modelBuilder.Entity<Label>()
                .HasOne(l => l.User)
                .WithMany(u => u.Labels)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notes ↔ Labels (M:N via NoteLabel)
            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Notes)
                .WithMany(n => n.NoteLabels)
                .HasForeignKey(nl => nl.NotesId)
                .OnDelete(DeleteBehavior.Cascade); // delete NoteLabel when Note is deleted

            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Label)
                .WithMany(l => l.NoteLabels)
                .HasForeignKey(nl => nl.LabelId)
                .OnDelete(DeleteBehavior.Cascade); // delete NoteLabel when Label is deleted

            // Prevent duplicate label on same note
            modelBuilder.Entity<NoteLabel>()
                .HasIndex(nl => new { nl.NotesId, nl.LabelId })
                .IsUnique();

            // Notes → Collaborators (1:N)
            modelBuilder.Entity<Collaborator>()
                .HasOne(c => c.Notes)
                .WithMany()
                .HasForeignKey(c => c.NotesId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Collaborators (1:N)
            modelBuilder.Entity<Collaborator>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
