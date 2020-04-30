using System;
using System.Linq;
using System.Text.RegularExpressions;
using Abarnathy.DemographicsAPI.Models;
using AutoMapper;

namespace Abarnathy.DemographicsAPI.Infrastructure
{
    /// <summary>
    /// Contains AutoMapper configuration profiles.
    /// </summary>
    public class MappingProfiles : Profile
    {
        /// <summary>
        /// Configures AutoMapper mapping profiles.
        /// </summary>
        public MappingProfiles()
        {
            // To DTO
            CreateMap<Patient, PatientDTO>()
                .ForMember(
                    dest => dest.Addresses,
                    cfg => cfg.MapFrom(src =>
                        src.PatientAddresses.Select(pa =>
                            pa.Address).ToList()
                    ))
                .ForMember(
                    dest => dest.PhoneNumbers,
                    cfg => cfg.MapFrom(src =>
                        src.PatientPhoneNumbers.Select(pn =>
                        pn.PhoneNumber).ToList())
                );

            CreateMap<Address, AddressDTO>();

            CreateMap<PhoneNumber, PhoneNumberDTO>();

            // To entity
            CreateMap<PatientDTO, Patient>()
                .ForMember(dest => dest.PatientAddresses, action => action.Ignore())
                .ForMember(dest => dest.PatientPhoneNumbers, action => action.Ignore());

            CreateMap<AddressDTO, Address>();

            CreateMap<PhoneNumberDTO, PhoneNumber>();
        }
    }
}