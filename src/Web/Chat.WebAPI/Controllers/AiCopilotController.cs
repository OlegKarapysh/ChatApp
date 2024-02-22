using Chat.Application.Services.AiCopilot;

namespace Chat.WebAPI.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public sealed class AiCopilotController : ControllerBase
{
    private readonly IAiCopilotService _aiCopilotService;

    public AiCopilotController(IAiCopilotService aiCopilotService)
    {
        _aiCopilotService = aiCopilotService;
    }

    [HttpPost("messages")]
    public async Task<ActionResult<SimpleMessageDto>> CreateUserMessage(SimpleMessageDto message)
    {
        return Ok(await _aiCopilotService.SendMessageToChatAsync(message));
    }
}