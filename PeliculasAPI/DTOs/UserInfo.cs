using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class UserInfo
    {
        [Required(ErrorMessage = "Credenciales incorrectas")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Credenciales incorrectas")]
        public string Password { get; set; }
    }
}
