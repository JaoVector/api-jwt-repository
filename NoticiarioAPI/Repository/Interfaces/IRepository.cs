using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System.Linq.Expressions;

namespace NoticiarioAPI.Repository.Interfaces;

public interface IRepository<T>
{
    IQueryable<T> Lista(int skip = 0, int take = 4);
    Task<T> PegaPorID(Expression<Func<T, bool>> expression);
    void Add(T entity);
    void Atualiza(T entity);
    void Deleta(T entity);
}
