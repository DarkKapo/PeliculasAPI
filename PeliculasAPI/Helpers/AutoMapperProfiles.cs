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
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(x => x.Foto, options => options.Ignore());//Para ignorar el mapeo de la foto
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();
            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.PeliculasGeneros, options => options.MapFrom(MapPeliculasGeneros)) //Agrega los datos a la tabla intermedia
                .ForMember(x => x.PeliculasActores, options => options.MapFrom(MapPeliculasActores));//Agrega los datos a la tabla intermedia
            CreateMap<Pelicula, PeliculaDetallesDTO>()
                .ForMember(x => x.Generos, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.Actores, options => options.MapFrom(MapPeliculasActores));
            CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();
        }

        private List<GeneroDTO> MapPeliculasGeneros(Pelicula pelicula, PeliculaDetallesDTO peliculaDetallesDTO)
        {
            var resultado = new List<GeneroDTO>();
            if(pelicula.PeliculasGeneros == null) return resultado;
            foreach(var genero in pelicula.PeliculasGeneros) resultado.Add(new GeneroDTO() { Id = genero.GeneroId, Nombre = genero.Genero.Nombre });
            return resultado;
        }

        private List<ActorPeliculaDetalleDTO> MapPeliculasActores(Pelicula pelicula, PeliculaDetallesDTO peliculaDetallesDTO)
        {
            var resultado = new List<ActorPeliculaDetalleDTO>();
            if(pelicula.PeliculasActores == null) return resultado;
            foreach(var actor in pelicula.PeliculasActores) resultado.Add(new ActorPeliculaDetalleDTO() { ActorId = actor.ActorId, Personaje = actor.Personaje, NombrePersona = actor.Actor.Nombre });
            return resultado;
        }
        //La idea es asignar los ids de generos y actores a pelicula (parecid a asignar un id a una FK)
        private List<PeliculasGeneros> MapPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();
            if(peliculaCreacionDTO.GenerosIDs == null) return resultado;
            foreach(var id in peliculaCreacionDTO.GenerosIDs) resultado.Add(new PeliculasGeneros() { GeneroId = id });
            return resultado;
        }

        private List<PeliculasActores> MapPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();
            if(peliculaCreacionDTO.Actores == null) return resultado;
            foreach(var actor in peliculaCreacionDTO.Actores) resultado.Add(new PeliculasActores() { ActorId = actor.ActorId, Personaje = actor.Personaje });
            return resultado;
        }
    }
}
