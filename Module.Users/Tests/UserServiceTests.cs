using Xunit;
using Moq;
using FluentAssertions;
using System.Linq.Expressions;
using GestaoDeTarefas.Module.Users.Domain.Models;
using GestaoDeTarefas.Module.Users.Domain.Repositories;
using GestaoDeTarefas.Module.Users.Domain.Services;
using UserEntity = GestaoDeTarefas.Module.Users.Domain.Entities.User;
using GestaoDeTarefas.Infra.UnitOfWork;
using GestaoDeTarefas.Infra.Enums;

namespace GestaoDeTarefas.Module.Users.Tests;

public class UserServiceTests
{
  private readonly Mock<IUnitOfWork> _unitOfWorkMock;
  private readonly Mock<IUserRepository> _userRepositoryMock;
  private readonly UserService _userService;

  public UserServiceTests()
  {
    _unitOfWorkMock = new Mock<IUnitOfWork>();
    _userRepositoryMock = new Mock<IUserRepository>();

    _unitOfWorkMock.SetupGet(x => x.User).Returns(_userRepositoryMock.Object);

    _userService = new UserService(_unitOfWorkMock.Object);
  }

  [Fact]
  public async Task RegisterAsync_DeveCriarUsuario_SeNaoExistir()
  {
    var dto = new UserDto
    {
      UserName = "felipe",
      Password = "123",
      Role = "admin"
    };

    _userRepositoryMock
        .Setup(repo => repo.ObterComCondicaoAsync(
            It.IsAny<Expression<Func<UserEntity, bool>>>(),
            null,
            EnumSortDirection.Asc,
            null,
            false,
            false
        ))
        .ReturnsAsync(new List<UserEntity>());

    _userRepositoryMock
        .Setup(repo => repo.SalvarAsync(It.IsAny<UserEntity>(), true))
        .ReturnsAsync((UserEntity u, bool _) => u);

    var result = await _userService.RegisterAsync(dto);

    result.Should().NotBeNull();
    result!.UserName.Should().Be("felipe");
    result.Role.Should().Be("admin");
  }

  [Fact]
  public async Task RegisterAsync_DeveRetornarNull_SeUsuarioJaExistir()
  {
    var dto = new UserDto
    {
      UserName = "felipe",
      Password = "123",
      Role = "admin"
    };

    _userRepositoryMock
        .Setup(repo => repo.ObterComCondicaoAsync(
            It.IsAny<Expression<Func<UserEntity, bool>>>(),
            null,
            EnumSortDirection.Asc,
            null,
            false,
            false
        ))
        .ReturnsAsync(new List<UserEntity> { new UserEntity { UserName = "felipe" } });

    var result = await _userService.RegisterAsync(dto);

    result.Should().BeNull();
  }

  [Fact]
  public async Task GetUserByIdAsync_DeveRetornarUsuario_SeExistirEAtivo()
  {
    var user = new UserEntity
    {
      Id = 1,
      UserName = "teste",
      Role = "admin",
      IsDeleted = false
    };

    _userRepositoryMock
        .Setup(repo => repo.FindAsync(1))
        .ReturnsAsync(user);

    var result = await _userService.GetUserByIdAsync(1);

    result.Should().NotBeNull();
    result!.UserName.Should().Be("teste");
    result.Role.Should().Be("admin");
  }

  [Fact]
  public async Task GetUserByIdAsync_DeveRetornarNull_SeNaoEncontradoOuDeletado()
  {
    _userRepositoryMock
        .Setup(repo => repo.FindAsync(1))
        .ReturnsAsync((UserEntity?)null);

    var resultNull = await _userService.GetUserByIdAsync(1);
    resultNull.Should().BeNull();

    _userRepositoryMock
        .Setup(repo => repo.FindAsync(2))
        .ReturnsAsync(new UserEntity { Id = 2, IsDeleted = true });

    var resultDeletado = await _userService.GetUserByIdAsync(2);
    resultDeletado.Should().BeNull();
  }

  [Fact]
  public async Task UpdateUserAsync_DeveAtualizarDados_SeUsuarioExistir()
  {
    var existingUser = new UserEntity
    {
      Id = 1,
      UserName = "old",
      Role = "user",
      IsDeleted = false
    };

    var dto = new UpdateUserDto
    {
      UserName = "new",
      Role = "admin"
    };

    _userRepositoryMock
        .Setup(repo => repo.FindAsync(1))
        .ReturnsAsync(existingUser);

    _userRepositoryMock
        .Setup(repo => repo.SalvarAsync(It.IsAny<UserEntity>(), true))
        .ReturnsAsync((UserEntity u, bool _) => u);

    var result = await _userService.UpdateUserAsync(1, dto);

    result.Should().BeTrue();
    existingUser.UserName.Should().Be("new");
    existingUser.Role.Should().Be("admin");
  }

  [Fact]
  public async Task UpdateUserAsync_DeveRetornarNull_SeUsuarioNaoExistirOuDeletado()
  {
    _userRepositoryMock
        .Setup(repo => repo.FindAsync(1))
        .ReturnsAsync((UserEntity?)null);

    var dto = new UpdateUserDto { UserName = "new", Role = "admin" };

    var resultNull = await _userService.UpdateUserAsync(1, dto);
    resultNull.Should().BeNull();

    _userRepositoryMock
        .Setup(repo => repo.FindAsync(2))
        .ReturnsAsync(new UserEntity { Id = 2, IsDeleted = true });

    var resultDeleted = await _userService.UpdateUserAsync(2, dto);
    resultDeleted.Should().BeNull();
  }

  [Fact]
  public async Task DeleteUserAsync_DeveMarcarComoDeletado_SeUsuarioExistir()
  {
    var user = new UserEntity
    {
      Id = 1,
      UserName = "teste",
      IsDeleted = false
    };

    _userRepositoryMock
        .Setup(repo => repo.FindAsync(1))
        .ReturnsAsync(user);

    _userRepositoryMock
        .Setup(repo => repo.SalvarAsync(It.IsAny<UserEntity>(), true))
        .ReturnsAsync((UserEntity u, bool _) => u);

    var result = await _userService.DeleteUserAsync(1);

    result.Should().BeTrue();
    user.IsDeleted.Should().BeTrue();
  }

  [Fact]
  public async Task DeleteUserAsync_DeveRetornarFalse_SeUsuarioNaoExistir()
  {
    _userRepositoryMock
        .Setup(repo => repo.FindAsync(99))
        .ReturnsAsync((UserEntity?)null);

    var result = await _userService.DeleteUserAsync(99);

    result.Should().BeFalse();
  }
}
