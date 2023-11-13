using Chat.Application.Extensions;
using Chat.Application.Services.Conversations;
using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebAPI.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public sealed class ConversationsController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationsController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<ConversationsPageDto>> SearchConversationsPagedAsync(
        [FromQuery] PagedSearchDto searchData)
    {
        return Ok(await _conversationService.SearchConversationsPagedAsync(searchData));
    }

    [HttpGet("all")]
    public async Task<ActionResult<IList<ConversationDto>>> GetAllUserConversationAsync()
    {
        return Ok(await _conversationService.GetAllUserConversationsAsync(default));
    }

    [HttpPost("dialogs")]
    public async Task<ActionResult<DialogDto>> CreateDialogAsync(NewDialogDto newDialogData)
    {
        if (newDialogData.CreatorId == default)
        {
            newDialogData.CreatorId = HttpContext.User.GetIdClaim();
        }
        
        return Ok(await _conversationService.CreateOrGetDialogAsync(newDialogData));
    }
}