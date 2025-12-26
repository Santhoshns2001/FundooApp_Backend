using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModalLayer.Entities
{
   public class Collaborator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  CollaboratorId { get; set; }

        public string Email { get; set; }

        [ForeignKey("Notes")]
        public int NotesId { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual Notes Notes { get; set; }

        public virtual User User { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }
    }
}
