using GoogleVoice.Services.Interfaces;
using GoogleVoice.Strategies;

namespace GoogleVoice.Services;

public class WebSiteInteractionService : IWebSiteInteractionService
{
    private readonly IBrowserService _browserService;
    private readonly WebSiteInteractionStrategy _interactionStrategy;

    public WebSiteInteractionService(IBrowserService browserService)
    {
        _browserService = browserService;
        _interactionStrategy = new(_browserService.GetOrCreatePageAsync().Result);
    }

    public async Task<string> ClickElement(string elementReferenceText)
    {
        var clickElements = await _interactionStrategy.ClickElement(elementReferenceText);
        return clickElements;
    }

    public async Task ScrollDown()
    {
        await _interactionStrategy.ScrollDown();
    }

    public async Task ScrollUp()
    {
        await _interactionStrategy.ScrollUp();
    }
}