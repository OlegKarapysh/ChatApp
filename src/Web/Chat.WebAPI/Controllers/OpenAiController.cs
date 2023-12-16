using Chat.Application.Services.OpenAI;
using Chat.Domain.DTOs.AssistantFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebAPI.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public class OpenAiController : ControllerBase
{
    private readonly IOpenAiService _openAiService;

    public OpenAiController(IOpenAiService openAiService)
    {
        _openAiService = openAiService;
    }

    [HttpPost("file")]
    public async Task<ActionResult<UploadedFileDto>> UploadFileAsync(IFormFile? file)
    {
        return Ok(await _openAiService.UploadFileAsync(file));
    }
}