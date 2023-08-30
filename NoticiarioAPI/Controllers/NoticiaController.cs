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

    [Authorize(Roles = "employee, manager")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadNoticiaDTO>>> ListaNoticias([FromQuery] int skip = 0, [FromQuery] int take = 4)
    {
        var consulta = await _uof.NoticiaRepository.Lista(skip, take).ToListAsync();

        if (consulta == null) throw new NotFoundException("Noticias não Econtradas");

        var noticias = _mapper.Map<List<ReadNoticiaDTO>>(consulta);

        return Ok(noticias);
    }

    [Authorize(Roles = "employee, manager")]
    [HttpGet("{id}", Name = "Obter Noticia por ID")]
    public async Task<ActionResult<ReadNoticiaDTO>> PegaPorID(int id)
    {
        var consulta = await _uof.NoticiaRepository.PegaPorID(i => i.Id == id);

        if (consulta == null) throw new NotFoundException($"Noticia com Id {id} não Econtrada");

        ReadNoticiaDTO readNoticia = _mapper.Map<ReadNoticiaDTO>(consulta);

        return Ok(readNoticia);
    }

    [Authorize(Roles = "employee, manager")]
    [HttpPost]
    public async Task<ActionResult> Add([FromBody] CreateNoticiaDTO createNoticia)
    {
        Noticia noticia = _mapper.Map<Noticia>(createNoticia);

        _uof.NoticiaRepository.Add(noticia);

        await _uof.Commit();

        ReadNoticiaDTO readNoticia = _mapper.Map<ReadNoticiaDTO>(noticia);

        return Ok(readNoticia);
    }

    [Authorize(Roles = "manager")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Deleta(int id)
    {
        var consulta = await _uof.NoticiaRepository.PegaPorID(n => n.Id == id);

        if (consulta == null) throw new BadRequestException($"Noticia com Id {id} não Econtrada");

        _uof.NoticiaRepository.Deleta(consulta);

        await _uof.Commit();

        return NoContent();
    }

    [Authorize(Roles = "manager")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Atualiza(int id, [FromBody] UpdateNoticiaDTO upNoticia)
    {
        var consulta = await _uof.NoticiaRepository.PegaPorID(n => n.Id == id);

        if (consulta == null) throw new NotFoundException($"Noticia com Id {id} não Econtrada");

        _mapper.Map(upNoticia, consulta);

        await _uof.Commit();

        ReadNoticiaDTO readNoticia = _mapper.Map<ReadNoticiaDTO>(consulta);

        return Ok(readNoticia);

    }
}
