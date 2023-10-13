using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoticiarioAPI.Domain.DTOS;
using NoticiarioAPI.Domain.Models;
using NoticiarioAPI.Exceptions;
using NoticiarioAPI.Repository.Interfaces;

namespace NoticiarioAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uof;
    
    public UsuarioController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _uof = unitOfWork;
    }

    [HttpPost]
    public async Task<ActionResult> Add([FromBody] CreateUsuarioDTO user) 
    {
        Usuario usuario = _mapper.Map<Usuario>(user);

        _uof.UsuarioRepository.Add(usuario);
        await _uof.Commit();

        return Ok(usuario);
    }

    [Authorize(Roles = "employee, manager")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadUsuarioDTO>>> ListaUsers([FromQuery] int skip = 0, [FromQuery] int take = 4)
    {
        var consulta = await _uof.UsuarioRepository.Lista(skip, take).ToListAsync();

        if (consulta == null) throw new NotFoundException("Usuarios não Econtrados");

        var noticias = _mapper.Map<List<ReadUsuarioDTO>>(consulta);

        return Ok(noticias);
    }

    [Authorize(Roles = "employee, manager")]
    [HttpGet("{id}", Name = "Obter Usuario por ID")]
    public async Task<ActionResult<ReadUsuarioDTO>> PegaPorID(int id)
    {
        var consulta = await _uof.UsuarioRepository.PegaPorID(i => i.Id == id);

        if (consulta == null) throw new NotFoundException($"Usuario com Id {id} não Econtrado");

        ReadUsuarioDTO readNoticia = _mapper.Map<ReadUsuarioDTO>(consulta);

        return Ok(readNoticia);
    }

    [Authorize(Roles = "manager")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Deleta(int id)
    {
        var consulta = await _uof.UsuarioRepository.PegaPorID(n => n.Id == id);

        if (consulta == null) throw new BadRequestException($"Usuario com Id {id} não Econtrado");

        _uof.UsuarioRepository.Deleta(consulta);

        await _uof.Commit();

        return NoContent();
    }

    [Authorize(Roles = "manager")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Atualiza(int id, [FromBody] UpdateUsuarioDTO upUser)
    {
        var consulta = await _uof.UsuarioRepository.PegaPorID(n => n.Id == id);

        if (consulta == null) throw new BadRequestException($"Usuario com Id {id} não Econtrado");

        _mapper.Map(upUser, consulta);

        await _uof.Commit();

        return NoContent();

    }
}
