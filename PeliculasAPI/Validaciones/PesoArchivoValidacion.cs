using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validaciones
{
    public class PesoArchivoValidacion: ValidationAttribute
    {
        private readonly int pesoMaximoEnMegaBytes;

        public PesoArchivoValidacion(int pesoMaximoEnMegaBytes)
        {
            this.pesoMaximoEnMegaBytes = pesoMaximoEnMegaBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var foto = (IFormFile)value;

            if (foto.Length > pesoMaximoEnMegaBytes * 1024 * 1024) return new ValidationResult($"El peso de la imagen no debe ser mayor a {pesoMaximoEnMegaBytes}MB");

            return ValidationResult.Success;
        }
    }
}
