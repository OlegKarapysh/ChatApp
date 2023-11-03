using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebAPI.Controllers;

[ApiController, Authorize, Route("api/[controller]]")]
public sealed class UsersController : ControllerBase
{
    
}