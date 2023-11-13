using Chat.Application.Services.Messages;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebAPI.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public sealed class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<MessagesPageDto>> SearchMessagesPagedAsync([FromQuery] PagedSearchDto searchData)
    {
        return Ok(await _messageService.SearchMessagesPagedAsync(searchData));
    }

    [HttpPost]
    public async Task<ActionResult> CreateMessageAsync(MessageDto messageData)
    {
        return Ok(await _messageService.CreateMessageAsync(messageData));
    }
}