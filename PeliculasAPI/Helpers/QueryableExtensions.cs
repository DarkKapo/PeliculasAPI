using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;

namespace PeliculasAPI.Helpers
{
    public static class QueryableExtensions
    {   //Método de extensión para paginar una consulta
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.CantidadRegistrosPorPagina)
                .Take(paginacionDTO.CantidadRegistrosPorPagina);
        }
    }
}
