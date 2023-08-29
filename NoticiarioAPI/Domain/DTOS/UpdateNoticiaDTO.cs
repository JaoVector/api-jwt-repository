using System.ComponentModel.DataAnnotations;

namespace NoticiarioAPI.Domain.DTOS;

public class UpdateNoticiaDTO
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public string? Conteudo { get; set; }
    public string? DataPublicacao { get; set; }
    public string? Autor { get; set; }
}
