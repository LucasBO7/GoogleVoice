namespace GoogleVoice.Entities;

public class TextEntity
{
    public string Text { get; private set; }
    public string? Category { get; private set; }

    private EntityCategory _categoryEnum; // Campo privado para armazenar o enum
    public EntityCategory CategoryEnum
    {
        get => _categoryEnum;
        set
        {
            _categoryEnum = value;
            Category = value.ToString();
        }
    }
    public double ConfidenceScore { get; private set; }

    public TextEntity(string text, EntityCategory categoryEnum, double confidenceScore)
    {
        Text = text;
        CategoryEnum = categoryEnum;
        ConfidenceScore = confidenceScore;
    }
}

public enum EntityCategory
{
    WebsiteName,
    URLs,
    InputText,
    ElementText,
    ElementColor,
    None
}