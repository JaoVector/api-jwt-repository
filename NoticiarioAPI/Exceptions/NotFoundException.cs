using NoticiarioAPI.Middlewares;

namespace NoticiarioAPI.Exceptions;

public class NotFoundException : Exception
{
	public NotFoundException(string message) : base(message)
	{

	}
}

public class BadRequestException : Exception
{
	public BadRequestException(string message) : base(message)
	{

	}
}

public class ErroNoBanco : Exception 
{
	public ErroNoBanco(string message) : base(message) 
	{

	}
}
