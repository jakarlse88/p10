using Abarnathy.HistoryService.Models;
using Abarnathy.HistoryService.Models.InputModels;
using AutoMapper;

namespace Abarnathy.HistoryService.Infrastructure
{
    /// <summary>
    /// AutoMapper profiles.
    /// </summary>
    public class MappingProfiles : Profile
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public MappingProfiles()
        {
            CreateMap<Note, NoteInputModel>();
            CreateMap<NoteInputModel, Note>();
            CreateMap<NoteCreateModel, Note>();

            CreateMap<NoteLogItem, NoteLogItemInputModel>();
            CreateMap<NoteLogItemInputModel, NoteLogItem>();
        }
    }
}