using Chat.Application.Services.Groups;
using Chat.Domain.DTOs.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebAPI.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public sealed class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost]
    public async Task<ActionResult<GroupDto>> CreateGroupAsync(NewGroupDto newGroupDto)
    {
        return Ok(await _groupService.CreateGroupAsync(newGroupDto));
    }
}