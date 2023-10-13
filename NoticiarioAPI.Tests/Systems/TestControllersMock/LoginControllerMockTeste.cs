using Moq;
using NoticiarioAPI.Controllers;
using NoticiarioAPI.Domain.Models;
using NoticiarioAPI.Repository.Interfaces;
using NoticiarioAPI.Tests.DataTest.FakeData;

namespace NoticiarioAPI.Tests.Systems.TestControllersMock
{
    public class LoginControllerMockTeste
    {
        private Mock<IUnitOfWork> _uof;
        private List<Usuario> _usuarios;

        public LoginControllerMockTeste()
        {
            var noticiaRepo = new Mock<IUsuarioRepository>();
            _uof = new Mock<IUnitOfWork>();

            _uof.Setup(repo => repo.UsuarioRepository).Returns(noticiaRepo.Object);
            _usuarios = FakeUser.FakeUsers();
        }

        [Fact]
        public void Autentica_Usuario_Sucesso_Returns() 
        {
            _uof.Setup(repo => repo.UsuarioRepository.AutenticaUser(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => _usuarios.FirstOrDefault());

            var controller = new LoginController(_uof.Object);

            var result = controller.AutenticaAsync("teste@gmail.com", "12345");

            Assert.NotNull(result);
        }

    }
}
