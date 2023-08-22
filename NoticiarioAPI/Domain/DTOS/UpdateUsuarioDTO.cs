namespace NoticiarioAPI.Domain.DTOS;

public class UpdateUsuarioDTO
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}
