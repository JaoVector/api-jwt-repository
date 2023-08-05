using NoticiarioAPI.Domain.Models;
using NoticiarioAPI.Exceptions;

namespace NoticiarioAPI.Middlewares;

public class ExceptionMiddleware : IMiddleware
{

    private readonly ILogger _logger;

	public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
	{
        _logger = logger;
	}

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{

			_logger.LogError($"Ocorreu Algum Erro {ex}");
			await HandlerException(context, ex);

		}
    }

	private static Task HandlerException(HttpContext context, Exception ex) 
	{
		int statusCode = StatusCodes.Status500InternalServerError;

		switch (ex)
		{
			case NotFoundException _:
				statusCode = StatusCodes.Status404NotFound;
				break;
			case BadRequestException _:
				statusCode = StatusCodes.Status400BadRequest;
				break;
			case ErroNoBanco _:
				statusCode = StatusCodes.Status503ServiceUnavailable;
				break;
			default:
				break;
		}

		var errorResponse = new ErrorsMap
		{
			StatusCode = statusCode,
			ErrorMessage = ex.Message
		};

		context.Response.ContentType = "apllication/json";
		context.Response.StatusCode = statusCode;

		return context.Response.WriteAsync(errorResponse.ToString());
	}
}

//Metodo de extensão para o middleware

public static class ExceptionMiddlewareExtension
{
	public static void ConfigureExceptionMiddleware(this IApplicationBuilder app) 
	{
		app.UseMiddleware<ExceptionMiddleware>();
	}
}
