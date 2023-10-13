using NoticiarioAPI.Context;
using NoticiarioAPI.Domain.Models;
using NoticiarioAPI.Repository.Interfaces;

namespace NoticiarioAPI.Repository.Classes;

public class NoticiaRepository : Repository<Noticia>, INoticiaRepository
{ 
    public NoticiaRepository(NContext context) : base(context) {}

}
