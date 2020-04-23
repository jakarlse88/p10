using AutoMapper;

namespace Abarnathy.DemographicsAPI.Models
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Patient
            CreateMap<Patient, PatientInputModel>();
            CreateMap<PatientInputModel, Patient>();
            
            // Address
            CreateMap<Address, AddressInputModel>();
            CreateMap<AddressInputModel, Address>();
            
            // Sex
            CreateMap<Sex, SexInputModel>();
            CreateMap<SexInputModel, Sex>();
            
            // 
        }
    }
}