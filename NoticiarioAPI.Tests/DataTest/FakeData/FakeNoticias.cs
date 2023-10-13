using NoticiarioAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoticiarioAPI.Tests.DataTest.FakeData
{
    public class FakeNoticias
    {
        public static List<Noticia> FakeNews()
        {
            return new List<Noticia>()
            {
                new Noticia() { Id=1, Titulo="A Volta dos Que não foram", Autor="Desconhecido", Conteudo="Alou Alou", DataPublicacao=DateTime.UtcNow, Descricao="Aloha Aloha" },
                new Noticia() { Id=2, Titulo="Os Croods", Autor="Cru", Conteudo="E Huuuuuu", DataPublicacao=DateTime.UtcNow, Descricao="Elu Elu" },
                new Noticia() { Id=3, Titulo="Aguias no Ceu", Autor="Falcao", Conteudo="Voem Voem", DataPublicacao=DateTime.UtcNow, Descricao="Galinha não voa" },
                new Noticia() { Id=4, Titulo="Aumento de Lucro", Autor="Lucradores", Conteudo="Lucrados", DataPublicacao=DateTime.UtcNow, Descricao="Muito Lucro" },
                new Noticia() { Id=5, Titulo="O Velho novo", Autor="Hugo", Conteudo="Cataclisma Nacional", DataPublicacao=DateTime.UtcNow, Descricao="Pouco a se fazer" },
                new Noticia() { Id=6, Titulo="Saude em Primeiro Plano", Autor="Bem estar", Conteudo="Como se cuidar de forma correta", DataPublicacao=DateTime.UtcNow, Descricao="A Solução da Boa Alimentação" }
            };
        }
    }
}
