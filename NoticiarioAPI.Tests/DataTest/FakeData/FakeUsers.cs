using NoticiarioAPI.Domain.Models;

namespace NoticiarioAPI.Tests.DataTest.FakeData
{
    public class FakeUser
    {
        public static List<Usuario> FakeUsers()
        {
            List<Usuario> fakeUsuarios = new List<Usuario>
            {
                new Usuario { Id=5, Email="teste@gmail.com", Name="Joao", Password="54321", Role="manager"},
                new Usuario { Id=2, Email="teste@gmail.com", Name="Victor", Password="12345", Role="manager"},
                new Usuario { Id=3, Email="teste@gmail.com", Name="Carneiro", Password="1235", Role="manager"},
            };

            return fakeUsuarios;
        }
    }
}
