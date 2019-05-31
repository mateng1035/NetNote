﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetNode.Models
{
    public class Note
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
        public int TypeId { get; set; }
        public NoteType Type { get; set; }
        public string Password { get; set; }
        public string Attachment { get; set; }
    }
}
