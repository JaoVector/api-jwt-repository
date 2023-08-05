using System.ComponentModel.DataAnnotations;

namespace NoticiarioAPI.Domain.DTOS;

public class ReadNoticiaDTO
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public string? Conteudo { get; set; }
    public string? DataPublicacao { get; set; }
    public string? Autor { get; set; }
}
