﻿using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PeliculasAPI.Helpers
{
    public class TypeBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nombrePropiedad = bindingContext.ModelName;
            var proveedorDeValores = bindingContext.ValueProvider.GetValue(nombrePropiedad);

            if (proveedorDeValores == ValueProviderResult.None) return Task.CompletedTask;

            try
            {
                var valorDeserializado = JsonConvert.DeserializeObject<List<int>>(proveedorDeValores.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valorDeserializado);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(nombrePropiedad, "Valor inválido para tipo de dato");
            }

            return Task.CompletedTask;
        }
    }
}
