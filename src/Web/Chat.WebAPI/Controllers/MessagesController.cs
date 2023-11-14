using Chat.Application.Extensions;
using Chat.Application.Services.Messages;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Chat.WebAPI.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.WebAPI.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public sealed class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IHubContext<ChatHub> _hubContext;

    public MessagesController(IMessageService messageService, IHubContext<ChatHub> hubContext)
    {
        _messageService = messageService;
        _hubContext = hubContext;
    }

    [HttpGet("search")]
    public async Task<ActionResult<MessagesPageDto>> SearchMessagesPagedAsync([FromQuery] PagedSearchDto searchData)
    {
        return Ok(await _messageService.SearchMessagesPagedAsync(searchData));
    }

    [HttpPost]
    public async Task<ActionResult<MessageWithSenderDto>> CreateMessageAsync(MessageDto messageData)
    {
        messageData.SenderId = HttpContext.User.GetIdClaim();
        return Ok(await _messageService.CreateMessageAsync(messageData));
    }
}