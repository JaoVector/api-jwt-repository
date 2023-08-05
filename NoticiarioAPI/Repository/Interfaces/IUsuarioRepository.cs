using NoticiarioAPI.Domain.Models;

namespace NoticiarioAPI.Repository.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    TokenModel AutenticaUser(string email, string password);
}
