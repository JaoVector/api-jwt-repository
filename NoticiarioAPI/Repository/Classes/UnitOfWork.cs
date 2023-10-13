using NoticiarioAPI.Context;
using NoticiarioAPI.Exceptions;
using NoticiarioAPI.Repository.Interfaces;

namespace NoticiarioAPI.Repository.Classes;

public class UnitOfWork : IUnitOfWork
{

    private NoticiaRepository _noticiaRepository;
    private UsuarioRepository _usuariosRepository;

    public NContext _nContext;
   
    public UnitOfWork(NContext nContext)
    {
        _nContext = nContext;   
    }
  
    public INoticiaRepository NoticiaRepository 
    {
        get 
        { 
            return _noticiaRepository = _noticiaRepository ?? new NoticiaRepository(_nContext); 
        }
    }

    public IUsuarioRepository UsuarioRepository 
    {
        get 
        {
            return _usuariosRepository = _usuariosRepository ?? new UsuarioRepository(_nContext);
        }
    }

   
    public async Task Commit()
    {
        try
        {
            await _nContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw new ErroNoBanco($"Erro ao Gravar no Banco {ex}");
        }
        
    }
}
