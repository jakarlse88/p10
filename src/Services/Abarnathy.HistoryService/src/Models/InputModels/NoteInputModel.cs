using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Abarnathy.HistoryService.Models.InputModels
{
    /// <summary>
    /// DTO for <see cref="Note"/>.
    /// </summary>
    public class NoteInputModel
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public NoteInputModel()
        {
            TimeCreated = DateTime.Now;
            TimeLastUpdated = DateTime.Now;
        }
        
        [Required]
        public string Id { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastUpdated { get; set; }

        public IEnumerable<NoteLogItemInputModel> NoteLog { get; set; }
    }
}