using System.Linq;
using Abarnathy.DemographicsAPI.Models;
using AutoMapper;

namespace Abarnathy.DemographicsAPI.Infrastructure
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // To input models
            CreateMap<Patient, PatientInputModel>()
                .ForMember(
                    dest => dest.Addresses,
                    cfg => cfg.MapFrom(src =>
                        src.PatientAddress.Select(pa =>
                            pa.Address).ToList()
                    )
                );

            CreateMap<Address, AddressInputModel>();

            // To entities
            CreateMap<PatientInputModel, Patient>();
            CreateMap<AddressInputModel, Address>();
        }
    }
}