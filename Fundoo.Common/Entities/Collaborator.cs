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
        public int CollaboratorId { get; set; }

        [Required]
        public string Email { get; set; }

        public int NotesId { get; set; }

        [JsonIgnore]
        public Notes Notes { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
