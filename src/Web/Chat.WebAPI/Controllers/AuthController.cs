using Chat.DomainServices.UnitsOfWork;
using Chat.Persistence.Contexts;
using Chat.Persistence.UnitsOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly ChatDbContext _unitOfWork;

    public AuthController(ChatDbContext unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _unitOfWork.Database.EnsureCreated();
    }


    [HttpGet("register")]
    public async Task<IActionResult> Register()
    {
        return Ok();
    }
}