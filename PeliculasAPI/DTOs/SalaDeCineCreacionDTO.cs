using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class SalaDeCineCreacionDTO
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El máximo de letras para {0} es {1}")]
        public string Nombre { get; set; }
    }
}
