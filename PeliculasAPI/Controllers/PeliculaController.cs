using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PeliculasAPI.Controllers.Entidades;
using PeliculasAPI.DTOs;
using PeliculasAPI.Helpers;
using PeliculasAPI.Servicios;

namespace PeliculasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";

        public PeliculaController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<PeliculasIndexDTO>> Get()
        {
            var top = 5;
            var fechaActual = DateTime.Today;
            var proximosEstrenos = await context.Peliculas.Where(x => x.FechaEstreno > fechaActual).Take(top).OrderBy(x => x.FechaEstreno).ToListAsync();
            var enCines = await context.Peliculas.Where(x => x.EnCines).Take(top).ToListAsync();
            var resultado = new PeliculasIndexDTO();
            resultado.FuturosEstrenos = mapper.Map<List<PeliculaDTO>>(proximosEstrenos);
            resultado.EnCines = mapper.Map<List<PeliculaDTO>>(enCines);
            return resultado;
        }

        [HttpGet("filtro")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] FiltroPeliculasDTO filtroPeliculasDTO)
        {
            var peliculasQueryable = context.Peliculas.AsQueryable();
            if (!string.IsNullOrEmpty(filtroPeliculasDTO.Titulo))
                peliculasQueryable = peliculasQueryable.Where(x => x.Titulo.Contains(filtroPeliculasDTO.Titulo));
            
            if (filtroPeliculasDTO.EnCines)
                peliculasQueryable = peliculasQueryable.Where(x => x.EnCines);
            
            if (filtroPeliculasDTO.ProximosEstrenos)
            {
                var fechaHoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(x => x.FechaEstreno > fechaHoy);
            }
            if (filtroPeliculasDTO.GeneroId != 0)
                peliculasQueryable = peliculasQueryable.Where(x => x.PeliculasGeneros.Select(y => y.GeneroId).Contains(filtroPeliculasDTO.GeneroId));
            
            await HttpContext.InsertarParametrosPaginacion(peliculasQueryable, filtroPeliculasDTO.CantidadRegistrosPorPagina);
            var peliculas = await peliculasQueryable.OrderBy(x => x.Titulo).Paginar(filtroPeliculasDTO.Paginacion).ToListAsync();
            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpGet("{id}", Name = "obtenerPelicula")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            if (pelicula == null) return NotFound();

            return mapper.Map<PeliculaDTO>(pelicula);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var param = Request.Form["Actores"].ToList(); //var param = Request.Form["parametro"];
            foreach (var actor in param)
            {
                var valorDeserializado = JsonConvert.DeserializeObject<ActorPeliculasCreacionDTO>(actor);
                peliculaCreacionDTO.Actores.Add(valorDeserializado);
            }
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, peliculaCreacionDTO.Poster.ContentType);
                }
            }
            AsignarOrdenActores(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();
            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return new CreatedAtRouteResult("obtenerPelicula", new { id = pelicula.Id }, peliculaDTO);
        }

        private void AsignarOrdenActores(Pelicula pelicula)
        {
            if (pelicula.PeliculasActores != null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var peliculaDB = await context.Peliculas.Include(x => x.PeliculasActores).Include(x => x.PeliculasGeneros).FirstOrDefaultAsync(x => x.Id == id);
            if (peliculaDB == null) return NotFound();
            var param = Request.Form["Actores"].ToList(); //var param = Request.Form["parametro"];
            foreach (var actor in param)
            {
                var valorDeserializado = JsonConvert.DeserializeObject<ActorPeliculasCreacionDTO>(actor);
                peliculaCreacionDTO.Actores.Add(valorDeserializado);
            }
            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliculaDB.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, peliculaDB.Poster, peliculaCreacionDTO.Poster.ContentType);
                }
            }
            AsignarOrdenActores(peliculaDB);
            peliculaDB = mapper.Map(peliculaCreacionDTO, peliculaDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PeliculaPatchDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest();

            var peliculaDB = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            if (peliculaDB == null) return NotFound();

            var peliculaDTO = mapper.Map<PeliculaPatchDTO>(peliculaDB);
            patchDocument.ApplyTo(peliculaDTO, ModelState);

            var esValido = TryValidateModel(peliculaDTO);
            if (!esValido) return BadRequest(ModelState);

            mapper.Map(peliculaDTO, peliculaDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            if (pelicula == null) return NotFound();

            context.Remove(pelicula);
            await context.SaveChangesAsync();
            await almacenadorArchivos.BorrarArchivo(pelicula.Poster, contenedor);
            return NoContent();
        }
    }
}
