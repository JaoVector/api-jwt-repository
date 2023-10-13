using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NoticiarioAPI.Context;
using NoticiarioAPI.Controllers;
using NoticiarioAPI.Domain.DTOS;
using NoticiarioAPI.Profiles;
using NoticiarioAPI.Repository.Classes;
using NoticiarioAPI.Repository.Interfaces;
using NoticiarioAPI.Tests.DataTest.FakeData;

namespace NoticiarioAPI.Tests.Systems.Controllers
{   
    public class NoticiaControllerTests
    {
        private IMapper _mapper;
        private IUnitOfWork _uof;
        private NoticiaController _controller;

        public static DbContextOptions<NContext> dbContextOptions { get; }

        static NoticiaControllerTests()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetValue<string>("ConnectionStrings:FarmConnection");
            dbContextOptions = new DbContextOptionsBuilder<NContext>().UseSqlServer(connectionString).Options;
        }

        public NoticiaControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new NoticiaProfile());
            });

            _mapper = config.CreateMapper();

            var context = new NContext(dbContextOptions);

            FakeDataDbMock dataNocitia = new FakeDataDbMock();

            dataNocitia.DataNoticias(context);

            _uof = new UnitOfWork(context);

            _controller = new NoticiaController(_uof, _mapper);
        }


        [Fact]
        public async Task Post_Add_Noticia_OkResult() 
        {
            
            var noticia = new CreateNoticiaDTO() { Titulo="Ordem desmedida", Autor="JohnJohn", Conteudo="Kkakamaskdabfksaksdnksafbasbjabsfjasfasjfjsfj", Descricao="E disso que falamos" };

            var result = await _controller.Add(noticia);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ListaNoticias_OkResult() 
        {
            var result = await _controller.ListaNoticias();

            Assert.IsType<ActionResult<IEnumerable<ReadNoticiaDTO>>>(result);
        }

        [Fact]
        public async Task Get_PegaPorID_Noticia_OkResult() 
        {
            var Id = 55;

            var noticia = await _controller.PegaPorID(Id);

            Assert.IsType<OkObjectResult>(noticia.Result);
        }

        [Fact]
        public async Task Put_Atualiza_Noticia_OkResult() 
        {
            var novaNoticia = new UpdateNoticiaDTO() { Titulo="A lagoa Azul", Autor="Desconhecido", Conteudo="Lua e Praia", DataPublicacao="05/05/2000", Descricao="A queda" };

            var id = 50;

            var atualizado = await _controller.Atualiza(id, novaNoticia);

            Assert.IsType<OkObjectResult>(atualizado);
        }

        [Fact]
        public async Task Delete_Deleta_Noticia_OkResult() 
        {
            var id = 60;

            var result = await _controller.Deleta(id);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
