using Microsoft.EntityFrameworkCore;

namespace PeliculasAPI.Helpers
{
    public static class HttpContextExtensions
    {   //Método de extensión para insertar los parámetros de paginación en el header de la respuesta
        //queryable: consulta que se va a paginar, se usa poara saber la cantidad total de registros en la tabla
        public async static Task InsertarParametrosPaginacion<T>(this HttpContext httpContext, IQueryable<T> queryable, int cantidadRegistrosPorPagina)
        {
            double conteo = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(conteo / cantidadRegistrosPorPagina);
            httpContext.Response.Headers.Add("totalPaginas", totalPaginas.ToString());
        }
    }
}
