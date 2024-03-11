using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Controllers.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 300, ErrorMessage = "El máximo de letras es {1}")]
        public string Titulo { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaEstreno{ get; set; }
        [StringLength(maximumLength: 2100, ErrorMessage = "El máximo de letras es {1}")]
        public string Poster { get; set; }
        public List<PeliculasActores> PeliculasActores { get; set; }
        public List<PeliculasGeneros> PeliculasGeneros { get; set; }
    }
}
