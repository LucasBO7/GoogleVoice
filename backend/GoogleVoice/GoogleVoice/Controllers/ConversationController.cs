using Microsoft.AspNetCore.Mvc;

namespace GoogleVoice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConversationController : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok("Funcionando!");
    }
}