using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Abarnathy.BlazorClient.Client.Models
{
    public class NoteInputModel
    {
        public NoteInputModel()
        {
            TimeCreated = DateTime.Now;
            TimeLastUpdated = DateTime.Now;

            NoteLog = new List<NoteLogItemInputModel>();
            
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public int PatientId { get; set; }
        
        [Required] 
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastUpdated { get; set; }

        public List<NoteLogItemInputModel> NoteLog { get; set; }
    }
}