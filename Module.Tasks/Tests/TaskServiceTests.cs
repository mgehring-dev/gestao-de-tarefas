using Xunit;
using Moq;
using FluentAssertions;
using System.Linq.Expressions;
using GestaoDeTarefas.Module.Tasks.Domain.Models;
using GestaoDeTarefas.Module.Tasks.Domain.Enums;
using GestaoDeTarefas.Module.Tasks.Domain.Repositories;
using GestaoDeTarefas.Module.Tasks.Domain.Services;
using GestaoDeTarefas.Module.Users.Domain.Repositories;
using GestaoDeTarefas.Infra.UnitOfWork;
using TaskEntity = GestaoDeTarefas.Module.Tasks.Domain.Entities.Task;
using UserEntity = GestaoDeTarefas.Module.Users.Domain.Entities.User;
using GestaoDeTarefas.Infra.Enums;

namespace GestaoDeTarefas.Module.Tasks.Tests;

public class TaskServiceTests
{
  private readonly Mock<IUnitOfWork> _unitOfWorkMock;
  private readonly Mock<ITaskRepository> _taskRepositoryMock;
  private readonly Mock<IUserRepository> _userRepositoryMock;
  private readonly TaskService _taskService;

  public TaskServiceTests()
  {
    _unitOfWorkMock = new Mock<IUnitOfWork>();
    _taskRepositoryMock = new Mock<ITaskRepository>();
    _userRepositoryMock = new Mock<IUserRepository>();

    _unitOfWorkMock.SetupGet(x => x.Task).Returns(_taskRepositoryMock.Object);
    _unitOfWorkMock.SetupGet(x => x.User).Returns(_userRepositoryMock.Object);

    _taskService = new TaskService(_unitOfWorkMock.Object);
  }

  [Fact]
  public async Task RegisterAsync_DeveCriarTarefa_SeUsuarioValido()
  {
    var dto = new TaskDto
    {
      Title = "Nova tarefa",
      Description = "Detalhes",
      IdUser = 1,
      DueDate = DateTime.Today,
      Status = EnumStatus.New
    };

    _userRepositoryMock
      .Setup(repo => repo.ObterComCondicaoAsync(It.IsAny<Expression<Func<UserEntity, bool>>>(), null,
            EnumSortDirection.Asc,
            null,
            false,
            false))
      .ReturnsAsync(new List<UserEntity> { new UserEntity { Id = 1, IsDeleted = false } });

    _taskRepositoryMock
      .Setup(repo => repo.SalvarAsync(It.IsAny<TaskEntity>(), true))
      .ReturnsAsync((TaskEntity t, bool _) => t);

    var result = await _taskService.RegisterAsync(dto);

    result.Should().NotBeNull();
    result!.Title.Should().Be("Nova tarefa");
  }

  [Fact]
  public async Task RegisterAsync_DeveRetornarNull_SeUsuarioInvalido()
  {
    var dto = new TaskDto
    {
      Title = "Tarefa inválida",
      IdUser = 999
    };

    _userRepositoryMock
      .Setup(repo => repo.ObterComCondicaoAsync(It.IsAny<Expression<Func<UserEntity, bool>>>(), null,
            EnumSortDirection.Asc,
            null,
            false,
            false))
      .ReturnsAsync(new List<UserEntity>());

    var result = await _taskService.RegisterAsync(dto);

    result.Should().BeNull();
  }

  [Fact]
  public async Task GetTaskByIdAsync_DeveRetornarTarefaComUsuario()
  {
    var task = new TaskEntity
    {
      Id = 1,
      Title = "Tarefa",
      IdUser = 1,
      Status = EnumStatus.New
    };

    var user = new UserEntity { Id = 1, UserName = "usuario" };

    _taskRepositoryMock
      .Setup(repo => repo.FindAsync(1))
      .ReturnsAsync(task);

    _userRepositoryMock
      .Setup(repo => repo.FindAsync(1))
      .ReturnsAsync(user);

    var result = await _taskService.GetTaskByIdAsync(1);

    result.Should().NotBeNull();
    result!.Title.Should().Be("Tarefa");
    result.User!.UserName.Should().Be("usuario");
  }

  [Fact]
  public async Task GetTaskByIdAsync_DeveRetornarNull_SeNaoExistir()
  {
    _taskRepositoryMock
      .Setup(repo => repo.FindAsync(1))
      .ReturnsAsync((TaskEntity?)null);

    var result = await _taskService.GetTaskByIdAsync(1);

    result.Should().BeNull();
  }

  [Fact]
  public async Task GetAllByIdUserAsync_DeveRetornarTarefas_SeExistirem()
  {
    var tasks = new List<TaskEntity>
    {
      new TaskEntity { Id = 1, Title = "T1", Status = EnumStatus.New, IdUser = 1 },
      new TaskEntity { Id = 2, Title = "T2", Status = EnumStatus.Active, IdUser = 1 }
    };

    _taskRepositoryMock
      .Setup(repo => repo.ObterComCondicaoAsync(It.IsAny<Expression<Func<TaskEntity, bool>>>(), null,
            EnumSortDirection.Asc,
            null,
            false,
            false))
      .ReturnsAsync(tasks);

    var result = await _taskService.GetAllByIdUserAsync(1);

    result.Should().HaveCount(2);
  }

  [Fact]
  public async Task GetAllByIdUserAsync_DeveRetornarListaVazia_SeNenhumaEncontrada()
  {
    _taskRepositoryMock
      .Setup(repo => repo.ObterComCondicaoAsync(It.IsAny<Expression<Func<TaskEntity, bool>>>(), null,
            EnumSortDirection.Asc,
            null,
            false,
            false))
      .ReturnsAsync(new List<TaskEntity>());

    var result = await _taskService.GetAllByIdUserAsync(1);

    result.Should().BeEmpty();
  }

  [Fact]
  public async Task UpdateTaskAsync_DeveAtualizar_SeValido()
  {
    var dto = new UpdateTaskDto
    {
      Id = 1,
      Title = "Atualizado",
      Description = "Nova descrição",
      IdUser = 1,
      Status = EnumStatus.Closed
    };

    _userRepositoryMock
      .Setup(repo => repo.FindAsync(1))
      .ReturnsAsync(new UserEntity { Id = 1, IsDeleted = false });

    _taskRepositoryMock
      .Setup(repo => repo.FindAsync(1))
      .ReturnsAsync(new TaskEntity { Id = 1 });

    _taskRepositoryMock
      .Setup(repo => repo.SalvarAsync(It.IsAny<TaskEntity>(), true))
      .ReturnsAsync((TaskEntity t, bool _) => t);

    var result = await _taskService.UpdateTaskAsync(1, dto);

    result.Should().BeTrue();
  }

  [Fact]
  public async Task UpdateTaskAsync_DeveRetornarNull_SeUsuarioOuTarefaInvalido()
  {
    var dto = new UpdateTaskDto { Id = 1, IdUser = 1 };

    _userRepositoryMock
      .Setup(repo => repo.FindAsync(1))
      .ReturnsAsync((UserEntity?)null);

    var result1 = await _taskService.UpdateTaskAsync(1, dto);
    result1.Should().BeNull();

    _userRepositoryMock
      .Setup(repo => repo.FindAsync(1))
      .ReturnsAsync(new UserEntity { Id = 1 });

    _taskRepositoryMock
      .Setup(repo => repo.FindAsync(1))
      .ReturnsAsync((TaskEntity?)null);

    var result2 = await _taskService.UpdateTaskAsync(1, dto);
    result2.Should().BeNull();
  }

  [Fact]
  public async Task DeleteTaskAsync_DeveDeletar_SeTarefaExistir()
  {
    _taskRepositoryMock
      .Setup(repo => repo.FindAsync(1))
      .ReturnsAsync(new TaskEntity { Id = 1 });

    _taskRepositoryMock
      .Setup(repo => repo.DeletarFisicamenteAsync(It.IsAny<TaskEntity>(), true))
      .Returns(Task.CompletedTask);

    var result = await _taskService.DeleteTaskAsync(1);

    result.Should().BeTrue();
  }

  [Fact]
  public async Task DeleteTaskAsync_DeveRetornarFalse_SeNaoEncontrar()
  {
    _taskRepositoryMock
      .Setup(repo => repo.FindAsync(1))
      .ReturnsAsync((TaskEntity?)null);

    var result = await _taskService.DeleteTaskAsync(1);

    result.Should().BeFalse();
  }
}
