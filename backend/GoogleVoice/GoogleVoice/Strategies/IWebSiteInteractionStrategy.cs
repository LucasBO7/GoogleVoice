namespace GoogleVoice.Strategies;

public interface IWebSiteInteractionStrategy
{
    Task<string> ClickElement(string elementReferenceText);
    Task FillInput();
    Task ScrollUp();
    Task ScrollDown();
}