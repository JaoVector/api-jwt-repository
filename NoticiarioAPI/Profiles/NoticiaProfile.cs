using AutoMapper;
using NoticiarioAPI.Domain.DTOS;
using NoticiarioAPI.Domain.Models;

namespace NoticiarioAPI.Profiles;

public class NoticiaProfile : Profile
{
	public NoticiaProfile()
	{
		CreateMap<Noticia, ReadNoticiaDTO>().ForMember(x => x.DataPublicacao, opt => opt.MapFrom(src => ((DateTime)src.DataPublicacao).ToString("dd/MM/yyyy")));
		CreateMap<CreateNoticiaDTO, Noticia>();
        CreateMap<UpdateNoticiaDTO, Noticia>().ForMember(x => x.DataPublicacao, opt => opt.MapFrom(src => DateTime.Parse(src.DataPublicacao)));
	}
}
