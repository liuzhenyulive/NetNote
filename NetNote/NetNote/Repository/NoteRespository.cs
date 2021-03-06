﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NetNote.Models;

namespace NetNote.Repository
{
    public class NoteRespository:INoteRespository
    {
        private NoteContext context;

        public NoteRespository(NoteContext _context)
        {
            this.context = _context;
        }

        public Task AddAsync(Note note)
        {
            context.Notes.Add(note);
            return context.SaveChangesAsync();
        }

        public Task<Note> GetByIdAsync(int id)
        {
            return context.Notes.FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<List<Note>> ListAsync()
        {
            return context.Notes.Include(type=>type.Type).ToListAsync();
        }

        public Task UpdateAsync(Note note)
        {
            context.Entry(note).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }

        public Tuple<List<Note>, int> PageList(int pageindex, int pagesize)
        {
            //var query = context.Notes.Include(type => type.Type).AsQueryable();
            //var cout = query.Count();
            //var pagecout = cout % pagesize == 0 ? cout / pagesize : cout / pagesize + 1;
            //var notes = query.OrderByDescending(r => r.Create).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            //return new Tuple<List<Note>, int>(notes,cout);


            var query = context.Notes.Include(type => type.Type).AsQueryable();
            var count = query.Count();
            var pagecount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            var notes = query.OrderByDescending(r => r.Create).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return new Tuple<List<Note>, int>(notes, pagecount);
        }
    }
}