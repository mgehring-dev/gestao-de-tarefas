using GestaoDeTarefas.Module.Users.Domain.Models;
using GestaoDeTarefas.Module.Users.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoDeTarefas.Module.Users.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
  [Authorize]
  [HttpPost]
  [SwaggerOperation(Summary = "Criar um novo usuário.")]
  public async Task<ActionResult<string>> Register(UserDto request)
  {
    var user = await userService.RegisterAsync(request);
    if (user is null)
    {
      return BadRequest("Username already exists.");
    }

    return Ok(user.Id);
  }

  [Authorize]
  [HttpGet("{id}")]
  [SwaggerOperation(Summary = "Obter informações de um usuário específico.")]
  public async Task<ActionResult<GetUserDto>> GetById(int id)
  {
    var user = await userService.GetUserByIdAsync(id);
    if (user is null)
    {
      return NotFound("User not found.");
    }

    return Ok(user);
  }

  [Authorize]
  [HttpPut("{id}")]
  [SwaggerOperation(Summary = "Atualizar informações do usuário.")]
  public async Task<ActionResult<GetUserDto>> Update(int id, UpdateUserDto request)
  {
    if (id != request.Id)
    {
      return BadRequest("User ID mismatch.");
    }

    var updateUser = await userService.UpdateUserAsync(id, request);
    if (updateUser is null)
    {
      return NotFound("User not found or already deleted.");
    }
    return Ok(updateUser);
  }

  [Authorize]
  [HttpDelete("{id}")]
  [SwaggerOperation(Summary = "Remover um usuário (soft delete).")]
  public async Task<ActionResult<bool>> Delete(int id)
  {
    var result = await userService.DeleteUserAsync(id);

    if (!result)
    {
      return NotFound("User not found.");
    }

    return Ok(true);
  }
}