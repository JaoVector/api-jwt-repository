using Microsoft.AspNetCore.Mvc;
using NoticiarioAPI.Context;
using NoticiarioAPI.Domain.Models;
using NoticiarioAPI.Repository.Interfaces;

namespace NoticiarioAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    private readonly IUnitOfWork _uof;

    private readonly NContext _nContext;

    public LoginController(IUnitOfWork unitOf, NContext context)
    {
        _uof = unitOf;
        _nContext = context;
    }

    [HttpPost]
    [Route("GeraToken")]
    public ActionResult<TokenModel> AutenticaAsync(string email, string password)
    {
        var user = _uof.UsuarioRepository.AutenticaUser(email, password);

        if (user != null)
        {
            return Ok(user);

        }
        else
        {
            return Unauthorized();
        }

    }
}
