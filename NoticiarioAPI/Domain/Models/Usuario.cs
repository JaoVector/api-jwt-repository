using System.ComponentModel.DataAnnotations;

namespace NoticiarioAPI.Domain.Models;

public class Usuario
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}
