using Microsoft.EntityFrameworkCore;
using NetNode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetNode.Tools
{
    public class NoteTypeRepository : INoteTypeRepository
    {
        private NoteContext _context;
        public NoteTypeRepository(NoteContext context)
        {
            _context = context;
        }

        public Task<List<NoteType>> ListAsync()
        {
            return _context.NoteTypes.ToListAsync();
        }
    }
}
