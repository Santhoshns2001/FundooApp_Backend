using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModalLayer.Entities
{
    public class NoteLabel
    {
        [Key]
        public int NoteLabelId { get; set; }

        public int NotesId { get; set; }
        public Notes Notes { get; set; }

        public int LabelId { get; set; }
        public Label Label { get; set; }
    }
}
