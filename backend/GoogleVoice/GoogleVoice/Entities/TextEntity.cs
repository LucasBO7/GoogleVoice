namespace GoogleVoice.Entities;

public class TextEntity
{
    public string Text { get; private set; }
    public string Category { get; private set; }
    public double ConfidenceScore { get; private set; }

    public TextEntity(string text, string category, double confidenceScore)
    {
        Text = text;
        Category = category;
        ConfidenceScore = confidenceScore;
    }
}