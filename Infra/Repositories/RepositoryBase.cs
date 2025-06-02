
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeTarefas.Infra;

public class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : EntityBase
{
    protected readonly AppDbContext _dbContext;
    public RepositoryBase(AppDbContext context) => _dbContext = context;

    public async void Dispose()
    {
        if (_dbContext != null)
            await _dbContext.DisposeAsync();
    }

    public virtual async Task DeletarListaAsync(IEnumerable<T> entities, bool commit = true)
    {
        _dbContext.RemoveRange(entities);

        if (commit)
            await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<long> ContarAsync(Expression<Func<T, bool>> condicao = null)
    {
        if (condicao == null)
            return (await _dbContext.Set<T>().AsNoTracking().Select(x => x.Id).ToListAsync()).Count;

        return (await _dbContext.Set<T>().AsNoTracking().Where(condicao).Select(x => x.Id).ToListAsync()).Count;
    }

    public virtual async Task DeletarFisicamenteAsync(T entity, bool commit = true)
    {
        _dbContext.Set<T>().Remove(entity);

        if (commit)
            await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<T> FindAsync(int id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        return entity;
    }

    public virtual async Task<IList<T>> InserirListaAsync(IList<T> lstEntity, bool commit = true, bool criarSomenteRegistrosQueNaoExistem = false)
    {
        if (lstEntity == null || lstEntity.Count.Equals(0))
            return null;

        var novaLista = new List<T>();
        if (criarSomenteRegistrosQueNaoExistem)
        {
            foreach (var e in lstEntity)
            {
                await InserirSeNaoExiste(e, false);
                novaLista.Add(e);
            }
        }
        else
        {
            foreach (var e in lstEntity)
            {
                e.CriadoEm = DateTime.Now;
                _dbContext.Set<T>().Add(e);
                novaLista.Add(e);
            }
        }

        if (commit)
            await _dbContext.SaveChangesAsync();

        return novaLista;
    }

    public virtual async Task<IEnumerable<T>> ObterComCondicaoAsync(
        Expression<Func<T, bool>> condicao,
        Expression<Func<T, object>> orderBy = null,
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

    public virtual async Task<IEnumerable<T>> ObterTodosAsync(Expression<Func<T, object>> orderBy = null, EnumSortDirection sortDirection = EnumSortDirection.Asc)
    {
        IQueryable<T> query = _dbContext.Set<T>().AsQueryable();

        if (orderBy != null)
        {
            if (sortDirection == EnumSortDirection.Asc)
                query = query.OrderBy(orderBy);
            else
                query = query.OrderByDescending(orderBy);
        }

        var entities = await query.ToListAsync();
        return entities;
    }

    public virtual async Task<T> SalvarAsync(T entity, bool commit = true)
    {
        T retorno = await ((entity.Id.Equals(Guid.Empty) || entity.Id.Equals(0))
                                            ? Inserir(entity)
                                            : Atualizar(entity));

        if (commit)
            await _dbContext.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<T> InserirSeNaoExiste(T entity, bool commit = true)
    {
        var registro = await _dbContext.Set<T>().FindAsync(entity.Id);
        if (registro != null)
            return entity;

        await Inserir(entity);

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

    public async Task<bool> ExisteAsync(int id)
    {
        var count = await this.ContarAsync(x => x.Id.Equals(id));
        return count > 0;
    }

    public async Task<bool> PossuiRegistrosAsync(Expression<Func<T, bool>> condicao = null)
    {
        var count = await this.ContarAsync(condicao);
        return count > 0;
    }

}
