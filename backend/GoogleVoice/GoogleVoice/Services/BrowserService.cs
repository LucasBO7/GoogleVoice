using GoogleVoice.Entities;
using GoogleVoice.Services.Interfaces;
using Microsoft.Playwright;

namespace GoogleVoice.Services;

public class BrowserService : IBrowserService
{
    private IBrowser? _browser;
    private IPage? _currentPage = null;

    public BrowserService()
    {
        if (_browser is not null)
            return;

        var playwright = Playwright.CreateAsync().Result;
        _browser = playwright.Chromium.LaunchAsync(new() { Headless = false }).Result; // Headless = false Permite a visualização do navegador
    }

    public async Task<IPage> GetOrCreatePageAsync()
    {
        if (_currentPage == null)
            _currentPage = await _browser!.NewPageAsync();

        return _currentPage;
    }

    public async void OpenWebsite(string websiteInfo, EntityCategory entityCategory)
    {
        var page = await GetOrCreatePageAsync();

        // Abre site com URL
        if (entityCategory == EntityCategory.URLs && ValidateWebsiteUrl(websiteInfo, out string websiteUrl))
        {
            await page.GotoAsync(websiteUrl);
            return;
        }

        // Abre site por nome (predefinido)
        string? predefinedUrl = GetPredefinedURLs(websiteInfo);
        if (predefinedUrl != null)
        {
            await page.GotoAsync(predefinedUrl);
            return;
        }

        // Pesquisa no google
        await page.GotoAsync("https://www.google.com/");
        await page.Keyboard.TypeAsync(websiteInfo, new KeyboardTypeOptions { Delay = 800 });
        //await page.Locator("textarea[name='q']").FillAsync(websiteInfo);
        await page.Keyboard.PressAsync("Enter");
    }

    public bool ValidateWebsiteUrl(string url, out string formattedUrl)
    {
        formattedUrl = string.Empty;

        // Verifica se a URL é válida
        if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
        {
            formattedUrl = uriResult.ToString();
            return true;
        }

        // Adiciona o formato da URL e a valida
        formattedUrl = $"https://{url}";
        if (Uri.TryCreate(formattedUrl, UriKind.Absolute, out Uri? formattedUri))
        {
            formattedUrl = formattedUri.ToString();
            return true;
        }

        return false;
    }

    public string? GetPredefinedURLs(string websiteName)
    {
        Enum.TryParse(websiteName, true, out PredefinedUrl enumResult);

        return enumResult switch
        {
            PredefinedUrl.Spotify => "https://www.spotify.com/",
            PredefinedUrl.Youtube => "https://www.youtube.com/",
            PredefinedUrl.Chatgpt => "https://chatgpt.com/",
            _ => null,
        };
    }
}

public enum PredefinedUrl
{
    None,
    Spotify,
    Youtube,
    Chatgpt,
}