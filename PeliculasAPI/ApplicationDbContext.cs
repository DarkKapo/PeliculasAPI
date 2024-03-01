using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Controllers.Entidades;

namespace PeliculasAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=DESKTOP-4T1RAFL;Initial Catalog=PeliculasAPI;Integrated Security=True");
        //    base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<Genero> Generos { get; set; }
    }
}
