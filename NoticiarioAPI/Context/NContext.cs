using Microsoft.EntityFrameworkCore;
using NoticiarioAPI.Domain.Models;

namespace NoticiarioAPI.Context;

public class NContext : DbContext
{
   
    public NContext(DbContextOptions<NContext> options) : base(options) { }

    public DbSet<Noticia> Noticias { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}
