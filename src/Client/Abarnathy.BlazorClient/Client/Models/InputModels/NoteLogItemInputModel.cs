using System;

namespace Abarnathy.BlazorClient.Client.Models
{
    public class NoteLogItemInputModel
    {
        public DateTime TimeOriginallyCreated { get; set; }
        public DateTime TimeArchived { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}