using GoogleVoice.Entities;
using Microsoft.Playwright;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace GoogleVoice.Strategies;

public class WebSiteInteractionStrategy : IWebSiteInteractionStrategy
{
    private readonly IPage _currentPage;

    public WebSiteInteractionStrategy(IPage currentPage)
    {
        _currentPage = currentPage;
    }

    //private async Task<string> SelectElementFromLocator(ILocator locatedElements)
    //{
    //var allElements = await locatedElements.AllTextContentsAsync();

    //if (allElements!.Count <= 1) return String.Empty;

    //StringBuilder matchedElementsContent = new();
    //Console.WriteLine("______________________");
    //Console.WriteLine("Elementos encontrados:");
    //foreach (var elementContent in allElements)
    //{
    //    Console.WriteLine(elementContent);
    //    matchedElementsContent.AppendLine(elementContent);
    //}

    //return matchedElementsContent.ToString();
    //}

    //private async Task<string> ClickInElementByText(ILocator locatedElement)
    //{

    //}

    //private async Task<string>? FindClickableElements(string elementReferenceText, StringBuilder matchedElementsContent)
    //{
    //    var elements = _currentPage
    //        .GetByText(elementReferenceText);
    //    //.QuerySelectorAllAsync($"button, a, [role='button'], input[type='submit'], input[type='button']");

    //    var strings = elements.EvaluateAllAsync<string>("el => el.tagName").Result;


    //    //List<IElementHandle> matchingElements = [];
    //    //matchedElementsContent = new();

    //    //foreach (var element in elements)
    //    //{
    //    //    var elementContentText = await element.TextContentAsync();
    //    //    if (elementContentText?.Contains(elementReferenceText, StringComparison.OrdinalIgnoreCase) == true)
    //    //    {
    //    //        matchingElements?.Add(element);
    //    //        matchedElementsContent.AppendLine(elementContentText);
    //    //    }
    //    //}

    //    //return matchingElements!;
    //    return strings;
    //}


    public async Task<string> ClickElement(string elementReferenceText)
    {
        IsPageOpened();

        var elements = await FindClickableElement(new(text: elementReferenceText, customSelectors: null))
            ?? throw new Exception("Elemento não encontrado no site.");
        string clickedElement = elements!.TextContentAsync().Result!;

        await _currentPage.BringToFrontAsync();
        await elements.ClickAsync();

        Console.WriteLine("Elemento clicado:");
        Console.WriteLine(clickedElement);
        return clickedElement;
    }

    public Task FillInput()
    {
        throw new NotImplementedException();
    }

    public async Task ScrollDown()
    {
        IsPageOpened();

        await _currentPage.BringToFrontAsync();
        await _currentPage.EvaluateAsync("document.documentElement.scrollBy(0, 1000);");
    }

    public async Task ScrollUp()
    {
        IsPageOpened();

        await _currentPage.EvaluateAsync("window.scrollBy(0, -1000);");
    }

    private void IsPageOpened()
    {
        if (_currentPage == null)
            throw new InvalidOperationException("A página não foi inicializada.");
    }
    private async Task<IElementHandle?> FindClickableElement(ClickableElementSearchCriteria criteria)
    {
        // Lista de seletores para diferentes tipos de elementos clicáveis
        var selectors = new List<string>();

        // Constrói seletores baseados nos critérios fornecidos
        if (!string.IsNullOrEmpty(criteria.Text))
        {
            selectors.AddRange(new[]
            {
                // Seletores diretos para elementos com o texto exato
                $"button:has-text('{criteria.Text}')",
                $"a:has-text('{criteria.Text}')",
                $"div[role='button']:has-text('{criteria.Text}')",
                $"span:has-text('{criteria.Text}')",
                $"span[role='button']:has-text('{criteria.Text}')",
                
                // Seletores para elementos clicáveis que contêm spans
                $"button:has(span:text('{criteria.Text}'))",
                $"a:has(span:text('{criteria.Text}'))",
                $"div[role='button']:has(span:text('{criteria.Text}'))",
                
                // Seletores genéricos
                $"*[role='button']:has-text('{criteria.Text}')",
                $"*[class*='button']:has-text('{criteria.Text}')",
                $"*[type='button']:has-text('{criteria.Text}')",
                $"*[type='submit']:has-text('{criteria.Text}')",
                
                // Seletores para spans dentro de elementos clicáveis
                $"*[role='button'] span:text('{criteria.Text}')",
                $"*[class*='button'] span:text('{criteria.Text}')"

            });
        }

        if (!string.IsNullOrEmpty(criteria.Text))
            selectors.Add($"#{criteria.Text}");

        if (!string.IsNullOrEmpty(criteria.Text))
            selectors.Add($"[data-testid='{criteria.Text}']");

        if (criteria.CustomSelectors?.Any() == true)
            selectors.AddRange(criteria.CustomSelectors);


        // Tenta cada seletor até encontrar um elemento
        foreach (var selector in selectors)
        {
            try
            {
                var element = await _currentPage.QuerySelectorAsync(selector);

                if (element != null && await IsElementInteractable(element))
                    return element;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "Erro ao tentar seletor: {Selector}" + selector);
                continue;
            }
        }

        return null;
    }
    private async Task<IElementHandle?> FindInputElement(ClickableElementSearchCriteria criteria)
    {
        var selectors = BuildSelectors(criteria);

        foreach (var selector in selectors)
        {
            try
            {
                var element = await _currentPage.QuerySelectorAsync(selector);

                if (element != null && await IsElementInteractable(element))
                    return element;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "Erro ao tentar seletor: {Selector}" + selector);
                continue;
            }
        }

        return null;
    }

    private List<string> BuildSelectors(ClickableElementSearchCriteria criteria)
    {
        var selectors = new List<string>();

        if (!string.IsNullOrEmpty(criteria.Text))
        {
            // Input selectors
            selectors.AddRange(new[]
            {
                $"input[value='{criteria.Text}']",
                $"input[placeholder='{criteria.Text}']",
                $"input[aria-label='{criteria.Text}']",
                $"input[name='{criteria.Text}']",
                $"textarea[placeholder='{criteria.Text}']",
                
                // Inputs com labels associados
                $"label:has-text('{criteria.Text}') input",
                $"label:has-text('{criteria.Text}') textarea",
                
                // Seletores existentes para outros elementos clicáveis
                $"button:has-text('{criteria.Text}')",
                $"a:has-text('{criteria.Text}')",
                $"div[role='button']:has-text('{criteria.Text}')",
                $"span:has-text('{criteria.Text}')"
            });
        }

        if (!string.IsNullOrEmpty(criteria.Text))
        {
            selectors.AddRange(new[]
            {
                $"#{criteria.Text}",
                $"input#{criteria.Text}",
                $"textarea#{criteria.Text}",
                $"select#{criteria.Text}"
            });
        }

        if (!string.IsNullOrEmpty(criteria.Text))
        {
            selectors.Add($"[name='{criteria.Text}']");
        }

        if (!string.IsNullOrEmpty(criteria.Text))
        {
            selectors.Add($"[data-testid='{criteria.Text}']");
        }

        if (criteria.CustomSelectors?.Any() == true)
        {
            selectors.AddRange(criteria.CustomSelectors);
        }

        return selectors;
    }

    private async Task<bool> IsElementInteractable(IElementHandle element)
    {
        // Verifica se o elemento está visível e clicável
        var isVisible = await element.IsVisibleAsync();
        var isEnabled = await element.IsEnabledAsync();

        return isVisible && isEnabled;
    }
}