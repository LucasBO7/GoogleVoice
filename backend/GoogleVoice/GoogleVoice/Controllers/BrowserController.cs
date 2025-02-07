using GoogleVoice.Entities;
using GoogleVoice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoogleVoice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrowserController : ControllerBase
{
    private readonly IBrowserService _browserService;

    public BrowserController(IBrowserService browserService)
    {
        _browserService = browserService;
    }

    [HttpGet]
    public async Task<IActionResult> CreateNewPageAsync()
    {
        try
        {
            await _browserService.GetOrCreatePageAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest("Houve um erro! " + ex.Message);
        }
    }

    [HttpGet("OpenWebsite")]
    public async Task<IActionResult> OpenWebsite(string websiteInfo, EntityCategory entityCategory)
    {
        try
        {
            _browserService.OpenWebsite(websiteInfo, entityCategory);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest("Houve um erro! " + ex.Message);
        }
    }
}