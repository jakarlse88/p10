using System;
using System.Collections.Generic;
using Abarnathy.HistoryService.Models;
using Abarnathy.HistoryService.Models.InputModels;
using AutoMapper;

namespace Abarnathy.HistoryService.Infrastructure
{
    internal static class AutoMapperExtensionMethods
    {
        /// <summary>
        /// Maps a  <see cref="Note"/> entity to
        /// a <seealso cref="NoteInputModel"/> DTO.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static NoteInputModel ToInputModel(this Note source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            var config = new MapperConfiguration(opt => { opt.AddProfile(new MappingProfiles()); });

            var mapper = config.CreateMapper();
            
            return mapper.Map<NoteInputModel>(source);
        }

        /// <summary>
        /// Maps a collection of <see cref="Note"/> entities to
        /// a collection of <seealso cref="NoteInputModel"/> DTOs.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static IEnumerable<NoteInputModel> ToInputModel(this IEnumerable<Note> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            var config = new MapperConfiguration(opt => { opt.AddProfile(new MappingProfiles()); });

            var mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<NoteInputModel>>(source);
        }
    }
}