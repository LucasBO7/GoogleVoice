namespace GoogleVoice.Services.Interfaces;

public interface IWebSiteInteractionService
{
    Task<string> ClickElement(string elementReferenceText);
    Task ScrollDown();
    Task ScrollUp();
}