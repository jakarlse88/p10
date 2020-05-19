using System;
using System.ComponentModel.DataAnnotations;

namespace Abarnathy.HistoryAPI.Models.InputModels
{
    public class NoteCreateModel
    {
        // <summary>
        /// Class constructor.
        /// </summary>
        public NoteCreateModel()
        {
            TimeCreated = DateTime.Now;
            TimeLastUpdated = DateTime.Now;
        }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastUpdated { get; set; }
    }
}