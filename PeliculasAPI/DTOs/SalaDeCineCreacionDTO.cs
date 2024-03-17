using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class SalaDeCineCreacionDTO
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El máximo de letras para {0} es {1}")]
        public string Nombre { get; set; }

        [Range(-90, 90)]
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }
    }
}
