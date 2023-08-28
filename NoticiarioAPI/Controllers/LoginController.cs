using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoticiarioAPI.Context;
using NoticiarioAPI.Domain.Models;
using NoticiarioAPI.Exceptions;
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
    [AllowAnonymous]
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

    public ActionResult RunMigration()
    {
        try
        {
            _nContext.Database.Migrate();
            return Ok();
        }
        catch (Exception ex)
        {

            throw new ErroNoBanco($"Houve um erro ao executar o Migrations {ex}");
        }

    }
}
