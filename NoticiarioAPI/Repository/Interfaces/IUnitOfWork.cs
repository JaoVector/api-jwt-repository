namespace NoticiarioAPI.Repository.Interfaces;

public interface IUnitOfWork
{
    INoticiaRepository NoticiaRepository { get; }
    IUsuarioRepository UsuarioRepository { get; }
    Task Commit();
}
