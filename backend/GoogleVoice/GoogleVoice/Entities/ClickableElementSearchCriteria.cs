namespace GoogleVoice.Entities;

public class ClickableElementSearchCriteria
{
    public string? Text { get; set; }
    public List<string>? CustomSelectors { get; set; }

    public ClickableElementSearchCriteria(string? text, List<string>? customSelectors)
    {
        Text = text;
        CustomSelectors = customSelectors;
    }
}