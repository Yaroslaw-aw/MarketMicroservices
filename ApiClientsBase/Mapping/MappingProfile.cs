using AutoMapper;
using ProductsMicroservice.Db;
using ProductsMicroservice.DTO;

namespace ProductsMicroservice.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap <ClientDto, Client>().ReverseMap();

            CreateMap<ClientDto, GetClientsDto>().ReverseMap();
        }
    }
}
