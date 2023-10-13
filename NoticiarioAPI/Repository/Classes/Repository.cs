using Microsoft.EntityFrameworkCore;
using NoticiarioAPI.Context;
using NoticiarioAPI.Exceptions;
using NoticiarioAPI.Repository.Interfaces;
using System.Linq.Expressions;

namespace NoticiarioAPI.Repository.Classes;

public class Repository<T> : IRepository<T> where T : class
{
    protected NContext _context;

	public Repository(NContext nContext)
	{
		_context = nContext;
	}

    public void Add(T entity)
    {
        try
        {
            _context.Set<T>().Add(entity);
        }
        catch (Exception ex)
        {

            throw new ErroNoBanco($"Erro ao Salvar valores no Banco {ex}");
        }
        
    }

    public void Atualiza(T entity)
    {
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
        }
        catch (Exception ex)
        {

            throw new ErroNoBanco($"Erro ao Atualizar valores no Banco {ex}");
        }
        
    }

    public void Deleta(T entity)
    {
        try
        {
            _context.Set<T>().Remove(entity);
        }
        catch (Exception ex)
        {
            throw new ErroNoBanco($"Erro ao Remover valores do Banco {ex}");
        }
        
    }

    public IQueryable<T> Lista(int skip, int take)
    {
        try
        {
            return _context.Set<T>().AsNoTracking().Skip(skip).Take(take);
        }
        catch (Exception ex)
        {
            throw new ErroNoBanco($"Erro ao Retornar valores do Banco {ex}");
        }
            
    }

    public async Task<T> PegaPorID(Expression<Func<T, bool>> expression)
    {
        try
        {
            return await _context.Set<T>().SingleOrDefaultAsync(expression);
        }
        catch (Exception ex)
        {
            throw new ErroNoBanco($"Erro ao Retornar valores do Banco {ex}");
        }
       
    }
}
