using Chat.Application.Extensions;
using Chat.Application.Services.Groups;
using Chat.Domain.DTOs.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebAPI.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public sealed class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost]
    public async Task<ActionResult<GroupDto>> CreateGroupAsync(NewGroupDto newGroupDto)
    {
        newGroupDto.CreatorId ??= HttpContext.User.GetIdClaim();
        return Ok(await _groupService.CreateGroupAsync(newGroupDto));
    }

    [HttpGet("all")]
    public async Task<ActionResult<IList<GroupInfoDto>>> GetAllGroupsInfoAsync()
    {
        var groupCreatorId = HttpContext.User.GetIdClaim();
        return Ok(await _groupService.GetAllGroupsInfoAsync(groupCreatorId));
    }
}