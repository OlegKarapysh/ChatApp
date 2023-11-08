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
        return Ok(await _conversationService.SearchUsersPagedAsync(searchData));
    }
}