
using System.Linq.Expressions;
using GestaoDeTarefas.Infra.Entities;
using GestaoDeTarefas.Infra.Enums;
using GestaoDeTarefas.Infra.Repositories.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeTarefas.Infra.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
    protected readonly AppDbContext _dbContext;
    public RepositoryBase(AppDbContext context) => _dbContext = context;

    public virtual async Task DeletarFisicamenteAsync(T entity, bool commit = true)
    {
        _dbContext.Set<T>().Remove(entity);

        if (commit)
            await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<T?> FindAsync(int id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        return entity;
    }

    public virtual async Task<IEnumerable<T>> ObterComCondicaoAsync(
        Expression<Func<T, bool>> condicao,
        Expression<Func<T, object>>? orderBy = null,
        EnumSortDirection sortDirection = EnumSortDirection.Asc,
        int? take = null,
        bool asNoTracking = false,
        bool ignoreQueryFilters = false
    )
    {
        var query = _dbContext.Set<T>().AsQueryable();

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (asNoTracking)
            query = query.AsNoTracking();

        query = query.Where(condicao);

        if (orderBy == null)
            return await query.ToListAsync();

        query = sortDirection == EnumSortDirection.Asc
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);

        var result =
            take.HasValue
            ? await query.Take(take.Value).ToListAsync()
            : await query.ToListAsync();

        return result;
    }

    public virtual async Task<T> SalvarAsync(T entity, bool commit = true)
    {
        await ((entity.Id.Equals(Guid.Empty) || entity.Id.Equals(0))
                                            ? Inserir(entity)
                                            : Atualizar(entity));

        if (commit)
            await _dbContext.SaveChangesAsync();

        return entity;
    }

    private async Task<T> Inserir(T entity)
    {
        entity.CriadoEm = DateTime.Now;
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    private async Task<T> Atualizar(T entity)
    {
        entity.CriadoEm = DateTime.Now;
        _dbContext.Set<T>().Update(entity);
        return entity;
    }
}
