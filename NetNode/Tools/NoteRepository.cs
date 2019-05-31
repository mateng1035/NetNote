﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetNode.Models;

namespace NetNode.Tools
{
    public class NoteRepository : INoteRepository
    {
        private NoteContext _context;

        public NoteRepository(NoteContext context)
        {
            _context = context;
        }

        public Task AddAsync(Note note)
        {
            _context.Notes.Add(note);
            return _context.SaveChangesAsync();
        }

        public Task<Note> GetByIdAsync(int id)
        {
            return _context.Notes.Include(t=>t.Type).FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<List<Note>> ListAsync()
        {
            return _context.Notes.Include(t=>t.Type).ToListAsync();
            //return _context.Notes.ToListAsync();
        }


        public Task UpdateAsync(Note note)
        {
            _context.Entry(note).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }


        public Tuple<List<Note>, int> PageList(int pageindex, int pagesize)
        {
            var query = _context.Notes.Include(type => type.Type).AsQueryable();
            var count = query.Count();
            var pagecount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            var notes = query.OrderByDescending(r => r.Create)
                .Skip((pageindex - 1) * pagesize)
                .Take(pagesize)
                .ToList();
            return new Tuple<List<Note>, int>(notes, pagecount);
        }
    }
}
