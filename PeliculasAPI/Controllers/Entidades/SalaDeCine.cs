using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Controllers.Entidades
{
    public class SalaDeCine : IId
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El máximo de letras para {0} es {1}")]
        public string Nombre { get; set; }
        public List<PeliculasSalasDeCine> PeliculasSalasDeCine { get; set; }
    }
}
