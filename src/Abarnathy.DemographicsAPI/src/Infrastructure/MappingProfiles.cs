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
            CreateMap<Patient, PatientInputModel>()
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

            CreateMap<Address, AddressInputModel>();

            CreateMap<PhoneNumber, PhoneNumberInputModel>();

            // To entity
            CreateMap<PatientInputModel, Patient>()
                .ForMember(dest => dest.Id, action => action.Ignore())
                .ForMember(dest => dest.PatientAddresses, action => action.Ignore())
                .ForMember(dest => dest.PatientPhoneNumbers, action => action.Ignore());

            CreateMap<AddressInputModel, Address>()
                .ForMember(dest => dest.Id, action => action.Ignore());

            CreateMap<PhoneNumberInputModel, PhoneNumber>()
                .ForMember(dest => dest.Id, action => action.Ignore());
        }
    }
}