using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ModalLayer.Entities
{
    public class Notes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotesId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Reminder { get; set; }
        public string? Colour { get; set; }
        public string? Image { get; set; }

        public bool? IsArchive { get; set; }
        public bool? IsPin { get; set; }
        public bool? IsTrash { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public ICollection<NoteLabel> NoteLabels { get; set; }

        public Notes()
        {
            NoteLabels = new HashSet<NoteLabel>();
        }
    }
}
