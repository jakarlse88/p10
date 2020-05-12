using System.Collections.Generic;
using Abarnathy.HistoryAPI.Data;
using Abarnathy.HistoryAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

namespace Abarnathy.HistoryAPI.Services
{
    public class NoteService
    {
        private readonly PatientHistoryDbContext _context;

        public NoteService(PatientHistoryDbContext context)
        {
            _context = context;
        }

        public List<Note> Get() =>
            _context.Notes.Find(note => true).ToList();

        // public Note Get(string id) =>
        //     _books.Find<Note>(note => note.Id == id).FirstOrDefault();

        // public Note Create(Note note)
        // {
        //     _books.InsertOne(note);
        //     return note;
        // }

        // public void Update(string id, Note bookIn) =>
        //     _books.ReplaceOne(note => note.Id == id, bookIn);

        // public void Remove(Note bookIn) =>
        //     _books.DeleteOne(note => note.Id == bookIn.Id);

        // public void Remove(string id) =>
        //     _books.DeleteOne(note => note.Id == id);
    }
}