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

    [HttpPost("ClickInElement")]
    public async Task<IActionResult> ClickElement(string elementReferenceTexto)
    {
        try
        {
            var clickElements = await _interactionService.ClickElement(elementReferenceTexto);
            return Ok(clickElements);
        }
        catch (Exception ex)
        {
            return BadRequest("Houve um erro! " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ScrollDown()
    {
        try
        {
            await _interactionService.ScrollDown();
            return Ok("FOI!");
        }
        catch (Exception ex)
        {
            return BadRequest("Houve um erro! " + ex.Message);
        }
    }
}