using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PeliculasAPI.Helpers
{
    public class CustomFromBodyOperationFilter: IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiBodyParameter =
                context.ApiDescription.ParameterDescriptions.FirstOrDefault(p =>
                    p.Source.CanAcceptDataFrom(BindingSource.Body));

            if (apiBodyParameter == null) return;

            var swaggerQueryParameter = operation.Parameters
                .FirstOrDefault(p => p.Name == apiBodyParameter.Name && p.In == ParameterLocation.Query);

            if (swaggerQueryParameter == null) return;

            operation.Parameters.Remove(swaggerQueryParameter);
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = { ["multipart/form-data"] = new OpenApiMediaType { Schema = swaggerQueryParameter.Schema } }
            };
        }
    }
}
