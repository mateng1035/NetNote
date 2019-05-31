using Microsoft.EntityFrameworkCore;
using NetNode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetNode.Tools
{
    public class NoteContext : DbContext
    {
        public NoteContext(DbContextOptions<NoteContext> options)
             : base(options)
        {

        }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteType> NoteTypes { get; set; }
    }
}
