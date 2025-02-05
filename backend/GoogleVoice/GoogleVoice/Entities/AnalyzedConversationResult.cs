namespace GoogleVoice.Entities;

public class AnalyzedConversationResult
{
    public string UserMessage { get; private set; }
    public string TopIntentCategoryName { get; private set; }
    private ActionCategory TopIntentCategory { get { return TopIntentCategory; } set => TopIntentCategoryName = value.ToString(); }
    public double? TopIntentConfidenceScore { get; private set; }
    public List<TextEntity> Entities { get; set; } = [];

    private AnalyzedConversationResult(string userMessage, ActionCategory topIntentCategory, double topIntentConfidenceScore, List<TextEntity>? entities = null)
    {
        UserMessage = userMessage;
        TopIntentCategory = topIntentCategory;
        TopIntentConfidenceScore = topIntentConfidenceScore;
        Entities = entities ?? new List<TextEntity>();
    }

    public static AnalyzedConversationResult NewInstanceWithEntities(string userMessage, ActionCategory topIntentCategory, double topIntentConfidenceScore, List<TextEntity> entities)
        => new AnalyzedConversationResult(userMessage, topIntentCategory, topIntentConfidenceScore, entities);
}

public enum ActionCategory
{
    OpenWebsite,
    ElementClick,
    InputInsertion,
    None
}