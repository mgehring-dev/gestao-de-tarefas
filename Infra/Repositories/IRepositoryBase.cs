
using System.Linq.Expressions;
using GestaoDeTarefas.Infra.Enums;

namespace GestaoDeTarefas.Infra.Repositories;

public interface IRepositoryBase<T> where T : class
{ 
    Task<T> SalvarAsync(T entity, bool commit = true); 
    Task DeletarFisicamenteAsync(T entity, bool commit = true);
    Task<IEnumerable<T>> ObterComCondicaoAsync(
        Expression<Func<T, bool>> condicao,
        Expression<Func<T, object>>? orderBy = null,
        EnumSortDirection sortDirection = EnumSortDirection.Asc,
        int? take = null,
        bool asNoTracking = false,
        bool ignoreQueryFilters = false
    );
    Task<T?> FindAsync(int id); 
}
