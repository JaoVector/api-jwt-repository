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
public class NoticiaController : ControllerBase
{

    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public NoticiaController(IUnitOfWork unitOf, IMapper mapper)
    {
        _uof = unitOf;
        _mapper = mapper;
    }

    [Authorize(Roles = "employee")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadNoticiaDTO>>> ListaNoticias([FromQuery] int skip = 0, [FromQuery] int take = 4)
    {
        var consulta = await _uof.NoticiaRepository.Lista(skip, take).ToListAsync();

        if (consulta == null) throw new NotFoundException("Noticia não Econtrada");

        var noticias = _mapper.Map<List<ReadNoticiaDTO>>(consulta);

        return Ok(noticias);
    }

    [Authorize(Roles = "employee")]
    [HttpGet("{id}", Name = "Obter Noticia por ID")]
    public async Task<ActionResult<ReadNoticiaDTO>> PegaPorID(int id) 
    {
        var consulta = await _uof.NoticiaRepository.PegaPorID(i => i.Id == id);

        if (consulta == null) throw new NotFoundException("Noticia não Econtrada");

        ReadNoticiaDTO readNoticia = _mapper.Map<ReadNoticiaDTO>(consulta);

        return Ok(readNoticia);
    }

    [Authorize(Roles = "employee")]
    [HttpPost]
    public async Task<ActionResult> Add([FromBody] CreateNoticiaDTO createNoticia) 
    {
        Noticia noticia = _mapper.Map<Noticia>(createNoticia);

        _uof.NoticiaRepository.Add(noticia);

        await _uof.Commit();

        return Ok(noticia);
    }

    [Authorize(Roles = "manager")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Deleta(int id) 
    {
        var consulta = await _uof.NoticiaRepository.PegaPorID(n => n.Id == id);

        if (consulta == null) throw new BadRequestException("Noticia não Econtrada");

        _uof.NoticiaRepository.Deleta(consulta);

        await _uof.Commit();

        return NoContent();
    }

    [Authorize(Roles = "manager")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Atualiza(int id, [FromBody] ReadNoticiaDTO readNoticia) 
    {
        var consulta = await _uof.NoticiaRepository.PegaPorID(n => n.Id == id);

        if (consulta == null) return NotFound("Noticia Não Encontrada");

        _mapper.Map(readNoticia, consulta);

        await _uof.Commit();

        return Ok(consulta);

    }
}
