using Abarnathy.HistoryAPI.Models;
using Abarnathy.HistoryAPI.Models.InputModels;
using AutoMapper;

namespace Abarnathy.HistoryAPI.Infrastructure
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

            CreateMap<NoteInputModel, Note>()
                .ForMember(dest => dest.Id, action => action.Ignore());
        }
    }
}