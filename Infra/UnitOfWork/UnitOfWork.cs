using System.Diagnostics.CodeAnalysis;
using GestaoDeTarefas.Infra.Repositories.Persistence;
using GestaoDeTarefas.Module.Tasks.Domain.Repositories;
using GestaoDeTarefas.Module.Users.Domain.Repositories;

namespace GestaoDeTarefas.Infra.UnitOfWork;

[ExcludeFromCodeCoverage]
public class UnitOfWork : IUnitOfWork
{
  private readonly AppDbContext _context;

  private IUserRepository? _userRepository;
  private ITaskRepository? _taskRepository;

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

  public ITaskRepository Task
  {
    get
    {
      if (_taskRepository == null)
        _taskRepository = new TaskRepository(_context);

      return _taskRepository;
    }
  }
}