using NoticiarioAPI.Context;
using NoticiarioAPI.Domain.Models;

namespace NoticiarioAPI.Tests.DataTest.FakeData
{
    public class FakeDataDbMock
    {
        public FakeDataDbMock(){}

        public void DataUsuarios(NContext context) 
        {
            context.Usuarios.Add(
                new Usuario { Email = "teste@gmail.com", Name = "Joao", Password = "54321", Role = "manager" });
            context.Usuarios.Add(
                new Usuario { Email = "teste@hotmail.com", Name = "Victor", Password = "54321", Role = "employee" });
            context.Usuarios.Add(
                new Usuario { Email = "teste@outlook.com", Name = "Joaquim", Password = "54321", Role = "manager" });
            
            context.SaveChanges();
        }

        public void DataNoticias(NContext context) 
        {
            
            context.Noticias.Add(
                new Noticia() { Titulo = "A Volta dos Que não foram", Autor = "Desconhecido", Conteudo = "Alou Alou", DataPublicacao = DateTime.UtcNow, Descricao = "Aloha Aloha" });
            context.Noticias.Add(
                new Noticia() { Titulo = "Os Croods", Autor = "Cru", Conteudo = "E Huuuuuu", DataPublicacao = DateTime.UtcNow, Descricao = "Elu Elu" });
            context.Noticias.Add(
                new Noticia() { Titulo = "Aguias no Ceu", Autor = "Falcao", Conteudo = "Voem Voem", DataPublicacao = DateTime.UtcNow, Descricao = "Galinha não voa" });

            context.SaveChanges();
           
        }
    }
}
