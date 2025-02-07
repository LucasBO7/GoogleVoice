using Microsoft.Playwright;

namespace GoogleVoice.Strategies;

public class WebSiteInteractionStrategy : IWebSiteInteractionStrategy
{
    private readonly IPage _currentPage;

    public WebSiteInteractionStrategy(IPage currentPage)
    {
        _currentPage = currentPage;
    }

    public async void ClickElement()
    {
        throw new NotImplementedException();
    }

    public void FillInput()
    {
        throw new NotImplementedException();
    }

    public async void ScrollDown()
    {
        await _currentPage.EvaluateAsync("window.scrollBy(0, 1000);");
    }

    public async void ScrollUp()
    {
        await _currentPage.EvaluateAsync("window.scrollBy(0, -1000);");
    }
}