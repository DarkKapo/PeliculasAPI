using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Controllers.Entidades
{
    public class Genero: IId
    //{
    //    public int Id { get; set; }
    //    [Required(ErrorMessage = "El campo {0} es requerido")]
    //    [StringLength(maximumLength: 40, ErrorMessage = "El máximo de letras es {1}")]
    //    public string Nombre { get; set; }
    //}
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 40, ErrorMessage = "El máximo de letras es {1}")]
        public string Nombre { get; set; }
        public List<PeliculasGeneros> GenerosPeliculas { get; set; }
    }
}
