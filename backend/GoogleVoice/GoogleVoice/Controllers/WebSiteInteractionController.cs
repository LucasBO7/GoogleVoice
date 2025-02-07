using GoogleVoice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoogleVoice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebSiteInteractionController : ControllerBase
{
    private readonly IWebSiteInteractionService _interactionService;

    public WebSiteInteractionController(IWebSiteInteractionService interactionService)
    {
        _interactionService = interactionService;
    }

    [HttpPost]
    public IActionResult ScrollDown()
    {
        try
        {
            _interactionService.ScrollDown();
            return Ok("FOI!");
        }
        catch (Exception ex)
        {
            return BadRequest("Houve um erro! " + ex.Message);
        }
    }
}