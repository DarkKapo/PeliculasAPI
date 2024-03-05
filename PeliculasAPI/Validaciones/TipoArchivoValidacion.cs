using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validaciones
{
    public class TipoArchivoValidacion: ValidationAttribute
    {
        private readonly string[] TipoValidos;
        public TipoArchivoValidacion(string[] tipoValidos)
        {
            TipoValidos = tipoValidos;
        }

        public TipoArchivoValidacion(GrupoTipoArchivo grupoTipoArchivo)
        {
            if (grupoTipoArchivo == GrupoTipoArchivo.Imagen) TipoValidos = new string[] { "image/jpeg", "image/png", "image/gif" };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var archivo = (IFormFile)value;

            if (TipoValidos == null) return ValidationResult.Success;

            if (TipoValidos.Length == 0) return ValidationResult.Success;

            if (!TipoValidos.Contains(archivo.ContentType)) return new ValidationResult($"El tipo del archivo debe ser uno de los siguientes: {string.Join(", ", TipoValidos)}");

            return ValidationResult.Success;
        }
    }
}
