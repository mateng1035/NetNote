using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetNode.Models
{
    public class ApiNoteModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Attachment { get; set; }
    }
}
