namespace NoticiarioAPI.Domain.DTOS;

public class CreateNoticiaDTO
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public string? Conteudo { get; set; }
    public string? Autor { get; set; }
}
