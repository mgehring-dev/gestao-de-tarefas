using System.Diagnostics.CodeAnalysis;
using GestaoDeTarefas.Module.Users;

namespace GestaoDeTarefas.Infra;

[ExcludeFromCodeCoverage]
public class UnitOfWork : IUnitOfWork
{
  private AppDbContext _context;

  private IUserRepository _userRepository;

  public UnitOfWork(AppDbContext context)
  {
    _context = context;
  }

  public IUserRepository User
  {
    get
    {
      if (_userRepository == null)
        _userRepository = new UserRepository(_context);

      return _userRepository;
    }
  }
}