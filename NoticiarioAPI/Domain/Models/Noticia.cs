using System.ComponentModel.DataAnnotations;

namespace NoticiarioAPI.Domain.Models;

public class Noticia
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    [StringLength(255)]
    public string? Titulo { get; set; }
    [Required]
    [StringLength(255)]
    public string? Descricao { get; set; }
    [Required]
    [StringLength(255)]
    public string? Conteudo { get; set; }
    [Required]
    public DateTime DataPublicacao { get; set; } = DateTime.Now;
    [Required]
    [StringLength(255)]
    public string? Autor { get; set; }
}