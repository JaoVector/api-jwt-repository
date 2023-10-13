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
    
    public class UserControllerTests
    {
        private IMapper _mapper;
        private IUnitOfWork _uof;
        private UsuarioController _controller;

        public static DbContextOptions<NContext> dbContextOptions { get; }

        static UserControllerTests() 
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetValue<string>("ConnectionStrings:FarmConnection");
            dbContextOptions = new DbContextOptionsBuilder<NContext>().UseSqlServer(connectionString).Options;
        }

        public UserControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UsuarioProfile());
            });

            _mapper = config.CreateMapper();

            var context = new NContext(dbContextOptions);
            
            FakeDataDbMock dataUser = new FakeDataDbMock();
            dataUser.DataUsuarios(context);

            _uof = new UnitOfWork(context);

            _controller = new UsuarioController(_mapper, _uof);
        }

        [Fact]
        public async Task Post_Sucesso_ReturnsStatusCode200()
        {
            // Arrange
            var userDto = new CreateUsuarioDTO() { Email= "test@gmail.com", Name="Victor Joao", Password="1234", Role="manager" };
            
            // Act
            var result = await _controller.Add(userDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ListaUsers_Usuarios_OkResult() 
        {
            var users = await _controller.ListaUsers();

            Assert.IsType<ActionResult<IEnumerable<ReadUsuarioDTO>>>(users);
        }

        [Fact]
        public async Task Get_PegaPorID_Usuario_OkResult() 
        {
            var id = 3;

            var user = await _controller.PegaPorID(id);

            Assert.IsType<OkObjectResult>(user.Result);
            
        }

        [Fact]
        public async Task Delete_Deleta_Usuario_OkResult() 
        {
            var id = 4;

            var dados = await _controller.Deleta(id);

            Assert.IsType<NoContentResult>(dados);
        }

        [Fact]

        public async Task Put_Atualiza_Usuario_OkReuslt() 
        {
            var id = 6;

            var novoModel = new UpdateUsuarioDTO { Email="test10@hotmail.com", Name="Ferreira Joaquim", Password="54321", Role="manager" };

            var atualizado = await _controller.Atualiza(id, novoModel);

            Assert.IsType<NoContentResult>(atualizado);
        }
    }
    
}