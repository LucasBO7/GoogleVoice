using GoogleVoice.Entities;
using Microsoft.Playwright;

namespace GoogleVoice.Services.Interfaces;

public interface IBrowserService
{
    Task<IPage> GetOrCreatePageAsync();
    void OpenWebsite(string websiteInfo, EntityCategory entityCategory);
}