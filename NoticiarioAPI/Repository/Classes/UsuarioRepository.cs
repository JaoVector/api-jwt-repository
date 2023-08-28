using Microsoft.IdentityModel.Tokens;
using NoticiarioAPI.Context;
using NoticiarioAPI.Domain.Models;
using NoticiarioAPI.Exceptions;
using NoticiarioAPI.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoticiarioAPI.Repository.Classes;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    private readonly NContext _context;
    private readonly IConfiguration _config;

    public UsuarioRepository(NContext nContext, IConfiguration config) : base(nContext)
    {
        _context = nContext;
        _config = config;
    }

    private string GeraToken(Usuario user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var chave = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            }),

            Expires = DateTime.UtcNow.AddHours(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), algorithm: SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    public TokenModel AutenticaUser(string email, string password)
    {
        var user = _context.Usuarios.Where(x => x.Email == email && x.Password == password).FirstOrDefault();

        if (user != null)
        {
            var token = GeraToken(user);
            return new TokenModel { Token = token };
        }
        else
        {
            throw new NotFoundException("Email ou Senha incorretos");
        }

    }

}
