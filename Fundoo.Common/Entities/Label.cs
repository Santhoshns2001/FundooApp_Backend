using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModalLayer.Entities
{
   public class Label
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }

        public string LabelName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int NotesId { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        public ICollection<Notes> NoteLabels { get; set; }
    }
}
