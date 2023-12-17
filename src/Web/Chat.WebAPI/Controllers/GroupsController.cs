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
    
    [HttpGet("all")]
    public async Task<ActionResult<IList<GroupInfoDto>>> GetAllGroupsInfoAsync()
    {
        var groupCreatorId = HttpContext.User.GetIdClaim();
        return Ok(await _groupService.GetAllGroupsInfoAsync(groupCreatorId));
    }

    [HttpPost]
    public async Task<ActionResult<GroupDto>> CreateGroupAsync(NewGroupDto newGroupDto)
    {
        var currentUserId = HttpContext.User.GetIdClaim();
        newGroupDto.CreatorId ??= currentUserId;
        return currentUserId == newGroupDto.CreatorId
            ? Ok(await _groupService.CreateGroupAsync(newGroupDto))
            : BadRequest();
    }

    [HttpPost("member")]
    public async Task<ActionResult<GroupDto>> AddGroupMemberAsync(NewGroupMemberDto newGroupMemberDto)
    {
        return Ok(await _groupService.AddGroupMemberAsync(newGroupMemberDto));
    }

    [HttpDelete("{groupId:int}")]
    public async Task<IActionResult> DeleteGroupAsync(int groupId)
    {
        return await _groupService.DeleteGroupAsync(groupId) ? Ok() : BadRequest();
    }
}