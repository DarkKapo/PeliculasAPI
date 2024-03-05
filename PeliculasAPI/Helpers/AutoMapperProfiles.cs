using AutoMapper;
using PeliculasAPI.Controllers.Entidades;
using PeliculasAPI.DTOs;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap(); //reverse es para mappear de GeneroDTO a Genero
            CreateMap<GeneroCreacionDTO, Genero>();
        }
    }
}
