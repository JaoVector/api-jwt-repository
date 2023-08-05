using Newtonsoft.Json;

namespace NoticiarioAPI.Domain.Models;

public class ErrorsMap
{
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
