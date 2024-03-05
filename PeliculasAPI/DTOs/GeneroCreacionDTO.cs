using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class GeneroCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 40, ErrorMessage = "El máximo de letras es {1}")]
        public string Nombre { get; set; }
    }
}
