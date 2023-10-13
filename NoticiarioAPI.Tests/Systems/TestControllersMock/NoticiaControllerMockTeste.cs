using AutoMapper;
using FluentAssertions;
using Moq;
using NoticiarioAPI.Controllers;
using NoticiarioAPI.Domain.DTOS;
using NoticiarioAPI.Domain.Models;
using NoticiarioAPI.Profiles;
using NoticiarioAPI.Repository.Interfaces;
using NoticiarioAPI.Tests.DataTest.FakeData;
using System.Linq.Expressions;

namespace NoticiarioAPI.Tests.Systems.TestControllersMock
{
    public class NoticiaControllerMockTeste
    {
        private List<Noticia> fakeNews;
        private Mock<IUnitOfWork> _uof;
        private IMapper _mapper;

        public NoticiaControllerMockTeste()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new NoticiaProfile());
            });

            _mapper = config.CreateMapper();

            fakeNews = FakeNoticias.FakeNews();

            var noticiaRepo = new Mock<INoticiaRepository>();
            _uof= new Mock<IUnitOfWork>();

            _uof.Setup(repo => repo.NoticiaRepository).Returns(noticiaRepo.Object);

            _uof.Setup(u => u.NoticiaRepository.PegaPorID(It.IsAny<Expression<Func<Noticia, bool>>>()))
                    .ReturnsAsync((Expression<Func<Noticia, bool>> predicate) =>
                    {
                        return fakeNews.Single(predicate.Compile());
                    });
        }


        [Fact]
        public void Get_ListaNoticias_Sucesso_Returns() 
        {
            _uof.Setup(repo => repo.NoticiaRepository.Lista(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((skip, take) => fakeNews.Skip(skip).Take(take).AsQueryable());

            var result = _uof.Object.NoticiaRepository.Lista();

            Assert.IsAssignableFrom<IEnumerable<Noticia>>(result);
        }


        [Fact]
        public async Task Get_PegaPorId_Sucesso_Returns() 
        {
            var newsId = 3;

            var controller = new NoticiaController(_uof.Object, _mapper);

            var result = await controller.PegaPorID(newsId);

            result.Should().NotBeNull();
        }


        [Fact]
        public async Task Post_Add_Noticia_Sucesso_Returns() 
        {
            _uof.Setup(repo => repo.NoticiaRepository.Add(It.IsAny<Noticia>()))
                    .Callback((Noticia noticia) => 
                    {
                        if(!fakeNews.Any(n => n.Id == noticia.Id)) fakeNews.Add(noticia);
                    });

            var newNoticia = new CreateNoticiaDTO() { Autor = "Alex", Conteudo = "A Farmacia", Descricao = "Big Pharma", Titulo = "Luta por Remedios mais Baratos" };
            
            var controller = new NoticiaController(_uof.Object, _mapper);
            await controller.Add(newNoticia);

            var noticia = fakeNews.FirstOrDefault(n => n.Titulo == newNoticia.Titulo);

            Assert.Equal(newNoticia.Titulo, noticia.Titulo);
            
        }


        [Fact]
        public async Task Atualiza_Noticia_Sucesso_Returns() 
        {
            _uof.Setup(repo => repo.NoticiaRepository.Atualiza(It.IsAny<Noticia>()))
                .Callback((Noticia noticia) => 
                {
                    var consulta = fakeNews.FirstOrDefault(n => n.Id == noticia.Id);
                    if (consulta != null) 
                    {
                        consulta.Titulo = noticia.Titulo;
                        consulta.Autor = noticia.Autor;
                        consulta.DataPublicacao = noticia.DataPublicacao;
                        consulta.Descricao = noticia.Descricao;
                        consulta.Conteudo = noticia.Conteudo;
                    }
                });

            var newNoticia = new UpdateNoticiaDTO() { Autor = "Alex", Conteudo = "A Farmacia", Descricao = "Big Pharma", Titulo = "Luta por Remedios mais Baratos", DataPublicacao="06/10/2023" };
            var noticiaId = 4;

            var controller = new NoticiaController(_uof.Object, _mapper);

            await controller.Atualiza(noticiaId, newNoticia);

            var noticiaAtualizada = fakeNews.Single(n => n.Id == noticiaId);

            Assert.Equal(newNoticia.Titulo, noticiaAtualizada.Titulo);
        }


        [Fact]
        public async Task Deleta_Noticia_Sucesso_Returns() 
        {
            _uof.Setup(repo => repo.NoticiaRepository.Deleta(It.IsAny<Noticia>()))
                    .Callback((Noticia noticia) => 
                    {
                        var consulta = fakeNews.FirstOrDefault(n => n.Id == noticia.Id);
                        if(consulta != null) fakeNews.Remove(consulta);
                    });

            var noticiaId = 5;
            var dele = fakeNews.FirstOrDefault(n => n.Id == noticiaId);

            var controller = new NoticiaController(_uof.Object, _mapper);

            await controller.Deleta(noticiaId);

            Assert.DoesNotContain(dele, fakeNews);
        }
    }
}
