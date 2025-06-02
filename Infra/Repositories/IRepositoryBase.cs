
using System.Linq.Expressions; 

namespace GestaoDeTarefas.Infra;

public interface IRepositoryBase<T> where T : class
{
    Task DeletarListaAsync(IEnumerable<T> entities, bool commit = true);
    Task<T> SalvarAsync(T entity, bool commit = true);
    Task<IList<T>> InserirListaAsync(IList<T> lstEntity, bool commit = true, bool criarSomenteRegistrosQueNaoExistem = false);
    Task DeletarFisicamenteAsync(T entity, bool commit = true);
    Task<IEnumerable<T>> ObterTodosAsync(Expression<Func<T, object>> orderBy = null, EnumSortDirection sortDirection = EnumSortDirection.Asc);
    Task<IEnumerable<T>> ObterComCondicaoAsync(
        Expression<Func<T, bool>> condicao,
        Expression<Func<T, object>> orderBy = null,
        EnumSortDirection sortDirection = EnumSortDirection.Asc,
        int? take = null,
        bool asNoTracking = false,
        bool ignoreQueryFilters = false
    );
    Task<T> FindAsync(int id);
    Task<long> ContarAsync(Expression<Func<T, bool>> condicao = null);
    Task<bool> PossuiRegistrosAsync(Expression<Func<T, bool>> condicao = null);
    Task<bool> ExisteAsync(int id);
    Task<T> InserirSeNaoExiste(T entity, bool commit = true);
}
