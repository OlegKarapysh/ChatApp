using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chat.Application.Extensions;
using Chat.Application.Mappings;
using Chat.Application.Services.Groups;
using Chat.Domain.DTOs.AssistantFiles;
using Chat.Domain.DTOs.Groups;

namespace Chat.WebAPI.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public sealed class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GroupDto>> GetGroupAsync(int id)
    {
        return Ok((await _groupService.GetGroupByIdAsync(id)).MapToDto());
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<IList<GroupInfoDto>>> GetAllGroupsInfoAsync()
    {
        var groupCreatorId = HttpContext.User.GetIdClaim();
        return Ok(await _groupService.GetAllGroupsInfoAsync(groupCreatorId));
    }

    [HttpGet("{id:int}/members")]
    public async Task<ActionResult<GroupWithMembersDto>> GetGroupWithMembersAsync(int id)
    {
        return Ok(await _groupService.GetGroupWithMembersAsync(id));
    }
    
    [HttpGet("{id:int}/files")]
    public async Task<ActionResult<GroupWithFilesDto>> GetGroupWithFilesAsync(int id)
    {
        return Ok(await _groupService.GetGroupWithFilesAsync(id));
    }
    
    [HttpPost("{id:int}/files")]
    public async Task<ActionResult<AssistantFileDto>> AddFileToGroupAsync(int id, UploadedFileDto uploadedFileDto)
    {
        return Ok(await _groupService.AddFileToGroupAsync(id, uploadedFileDto));
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

    [HttpPost("members")]
    public async Task<ActionResult<GroupDto>> AddGroupMemberAsync(NewGroupMemberDto newGroupMemberDto)
    {
        return Ok(await _groupService.AddGroupMemberAsync(newGroupMemberDto));
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<GroupDto>> EditGroupAsync(int id, [FromBody] NewGroupDto groupDto)
    {
        return Ok(await _groupService.EditGroupAsync(id, groupDto));
    }
    
    [HttpDelete("{groupId:int}/files/{fileId}")]
    public async Task<IActionResult> DeleteFileFromGroupAsync(int groupId, int fileId)
    {
        return await _groupService.DeleteFileFromGroupAsync(fileId, groupId) ? NoContent() : BadRequest();
    }
    
    [HttpDelete("{groupId:int}/members/{userName}")]
    public async Task<IActionResult> DeleteGroupMemberAsync(int groupId, string userName)
    {
        return await _groupService.DeleteGroupMember(userName, groupId) ? NoContent() : BadRequest();
    }

    [HttpDelete("{groupId:int}")]
    public async Task<IActionResult> DeleteGroupAsync(int groupId)
    {
        return await _groupService.DeleteGroupAsync(groupId) ? NoContent() : BadRequest();
    }
}