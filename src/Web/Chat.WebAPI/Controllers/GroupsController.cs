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
        var groupCreatorId = HttpContext.User.GetIdClaim();
        return Ok(await _groupService.GetGroupWithMembersAsync(id, groupCreatorId));
    }
    
    [HttpGet("{id:int}/files")]
    public async Task<ActionResult<GroupWithFilesDto>> GetGroupWithFilesAsync(int id)
    {
        var groupCreatorId = HttpContext.User.GetIdClaim();
        return Ok(await _groupService.GetGroupWithFilesAsync(id, groupCreatorId));
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
        groupDto.CreatorId = HttpContext.User.GetIdClaim();
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