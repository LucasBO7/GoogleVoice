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

    public void ScrollDown()
    {
        _interactionStrategy.ScrollDown();
    }

    public void ScrollUp()
    {
        _interactionStrategy.ScrollUp();
    }
}