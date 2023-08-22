using AutoMapper;
using NoticiarioAPI.Domain.DTOS;
using NoticiarioAPI.Domain.Models;

namespace NoticiarioAPI.Profiles;

public class UsuarioProfile : Profile
{
	public UsuarioProfile()
	{
		CreateMap<CreateUsuarioDTO, Usuario>();
		CreateMap<Usuario, ReadUsuarioDTO>();
		CreateMap<UpdateUsuarioDTO, Usuario>();
	}
}
