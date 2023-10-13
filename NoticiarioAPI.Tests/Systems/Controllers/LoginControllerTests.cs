using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NoticiarioAPI.Context;
using NoticiarioAPI.Controllers;
using NoticiarioAPI.Repository.Classes;
using NoticiarioAPI.Repository.Interfaces;

namespace NoticiarioAPI.Tests.Systems.Controllers
{
    public class LoginControllerTests
    {
        private IUnitOfWork _uof;
        private LoginController _controller;

        public static DbContextOptions<NContext> dbContextOptions { get; } 
       
        static LoginControllerTests()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetValue<string>("ConnectionStrings:FarmConnection");
            dbContextOptions = new DbContextOptionsBuilder<NContext>().UseSqlServer(connectionString).Options;
        }

        public LoginControllerTests()
        {
            var context = new NContext(dbContextOptions);

            _uof = new UnitOfWork(context);

            _controller = new LoginController(_uof);
        }


        [Fact]
        public void Teste_Para_Verificar_Login()
        { 
            //Act
            var logar = _controller.AutenticaAsync("test@gmail.com", "1234");

            //Assert
            Assert.IsType<OkObjectResult>(logar.Result);
        }
    }
  
}
