namespace PeliculasAPI.DTOs
{
    public class FiltroPeliculasDTO
    {
        public int Pagina { get; set; } = 1;
        public int CantidadRegistrosPorPagina { get; set; } = 10;
        public PaginacionDTO Paginacion
        {
            get
            {
                return new PaginacionDTO() { Pagina = Pagina, CantidadRegistrosPorPagina = CantidadRegistrosPorPagina };
            }
        }
        public string Titulo { get; set; } = "";
        public int GeneroId { get; set; } = 0;
        public bool EnCines { get; set; } = false;
        public bool ProximosEstrenos { get; set; } = false;
        public string CampoOrdenar { get; set; }
        public bool OrdenarAsc { get; set; } = true;
    }
}
