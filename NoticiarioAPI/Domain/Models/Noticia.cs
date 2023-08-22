using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [MaxLength]
    public string? Conteudo { get; set; }
    [Required]
    public DateTime DataPublicacao { get; set; } = DateTime.Now;
    [Required]
    [StringLength(255)]
    public string? Autor { get; set; }
}