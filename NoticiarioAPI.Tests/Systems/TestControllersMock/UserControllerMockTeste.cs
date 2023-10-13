using NoticiarioAPI.Controllers;
using AutoMapper;
using NoticiarioAPI.Profiles;
using Telerik.JustMock;
using Moq;
using NoticiarioAPI.Repository.Interfaces;
using NoticiarioAPI.Domain.Models;
using NoticiarioAPI.Domain.DTOS;
using System.Linq.Expressions;
using FluentAssertions;
using NoticiarioAPI.Tests.DataTest.FakeData;

namespace NoticiarioAPI.Tests.Systems.TestControllersMock
{
    public class UserControllerMockTeste
    {
        private List<Usuario> usersFake;
        private Mock<IUnitOfWork> _uof;
        private IMapper _mapper;

        public UserControllerMockTeste()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UsuarioProfile());
            });

            _mapper = config.CreateMapper();

            var userRepo = new Mock<IUsuarioRepository>();

            _uof = new Mock<IUnitOfWork>();
            usersFake = FakeUser.FakeUsers();

            _uof.Setup(u => u.UsuarioRepository).Returns(userRepo.Object);
            _uof.Setup(u => u.UsuarioRepository.PegaPorID(It.IsAny<Expression<Func<Usuario, bool>>>()))
                    .ReturnsAsync((Expression<Func<Usuario, bool>> predicate) =>
                    {
                        return usersFake.Single(predicate.Compile());
                    });

        }


        [Fact]
        public void GetMock_ListaUsers_Sucesso_ReturnsStatusCode200()
        {

            _uof.Setup(u => u.UsuarioRepository.Lista(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns<int, int>((skip, take) => usersFake.Skip(skip).Take(take).AsQueryable());

            var resultU = _uof.Object.UsuarioRepository.Lista();

            Assert.IsAssignableFrom<IQueryable>(resultU);

        }


        [Fact]
        public async Task PostMock_Add_Sucesso_Returns()
        {
            _uof.Setup(repo => repo.UsuarioRepository.Add(It.IsAny<Usuario>()))
                .Callback((Usuario user) =>
                {
                    if (!usersFake.Any(u => u.Id == user.Id)) usersFake.Add(user);
                });


            var controller = new UsuarioController(_mapper, _uof.Object);

            var user1 = new CreateUsuarioDTO() { Email = "joaquim@hotmail.com", Name = "Joaquim", Password = "12345", Role = "employee" };
            await controller.Add(user1);

            var userLis = usersFake.FirstOrDefault(u => u.Name == user1.Name);
            Assert.Equal(user1.Name, userLis.Name);

        }


        [Fact]
        public async Task Get_PegaPorId_Sucesso_Returns()
        {
            int userId = 2;

            var controller = new UsuarioController(_mapper, _uof.Object);

            var resp = await controller.PegaPorID(userId);

            Assert.NotNull(resp);
            resp.Should().NotBeNull();
        }


        [Fact]
        public async Task Put_Atualiza_Sucesso_Returns()
        {
            _uof.Setup(repo => repo.UsuarioRepository.Atualiza(It.IsAny<Usuario>()))
                .Callback((Usuario user) =>
                {
                    var consulta = usersFake.FirstOrDefault(x => x.Id == user.Id);
                    if (consulta != null)
                    {
                        consulta.Name = user.Name;
                        consulta.Email = user.Email;
                        consulta.Password = user.Password;
                        consulta.Role = user.Role;
                    }
                });

            var nuser = new UpdateUsuarioDTO() { Email = "hospital@outlook.com", Name = "Paulo", Password = "54321", Role = "manager" };
            var nuserId = 2;
            var controller = new UsuarioController(_mapper, _uof.Object);

            await controller.Atualiza(nuserId, nuser);
            var atualizadoLista = usersFake.Single(n => n.Id == nuserId);

            Assert.Equal(nuser.Name, atualizadoLista.Name);
        }


        [Fact]
        public async Task Delete_Deleta_User_Sucesso_Returns()
        {

            _uof.Setup(repo => repo.UsuarioRepository.Deleta(It.IsAny<Usuario>()))
                  .Callback((Usuario user) =>
                  {
                      var consulta = usersFake.FirstOrDefault(u => u.Id == user.Id);
                      if (consulta != null)
                      {
                          usersFake.Remove(consulta);
                      }
                  });


            var id = 3;
            var del = usersFake.FirstOrDefault(n => n.Id == id);

            var controller = new UsuarioController(_mapper, _uof.Object);

            await controller.Deleta(id);

            Assert.DoesNotContain(del, usersFake);
        }
    }
}