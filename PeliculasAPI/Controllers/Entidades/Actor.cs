using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Controllers.Entidades
{
    public class Actor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El máximo de letras es {1}")]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; }//Guarda la url de la foto que estará en azure
    }
}
