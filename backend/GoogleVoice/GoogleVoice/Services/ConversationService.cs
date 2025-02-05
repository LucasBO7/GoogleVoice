using Azure.AI.Language.Conversations;
using GoogleVoice.Services.Interfaces;
using Azure;
using Azure.Core;
using System.Text.Json;
using GoogleVoice.Entities;

namespace GoogleVoice.Services;

public class ConversationService : IConversationService
{
    private readonly IConfiguration _configuration;

    private readonly string? _cluEndpoint;
    private readonly string? _apiKey;

    public ConversationService(IConfiguration configuration)
    {
        _configuration = configuration;

        _cluEndpoint = _configuration!.GetValue<string>("CLU_ENDPOINT") ?? throw new Exception("SPEECH_KEY não foi definida.");
        _apiKey = _configuration!.GetValue<string>("CLU_APIKEY") ?? throw new Exception("SPEECH_REGION não foi definida.");
    }

    public async Task<AnalyzedConversationResult> AnalyzeConversationAsync(string message)
    {
        ConversationAnalysisClient client = new(
            new Uri(_cluEndpoint!),
            new AzureKeyCredential(_apiKey!)
        );

        var data = BuildDataRequest(message);

        Response response = await client.AnalyzeConversationAsync(RequestContent.Create(data));
        using var jsonDocument = JsonDocument.Parse(response.ContentStream!);
        var root = jsonDocument.RootElement;

        AnalyzedConversationResult analyzedConversationResult = GetJsonValues(root);

        return analyzedConversationResult;
    }

    public object BuildDataRequest(string message)
    {
        string projectName = "GoogleVoice-clu";
        string deploymentName = "GoogleVoice-clu-deploy";

        var data = new
        {
            analysisInput = new
            {
                conversationItem = new
                {
                    text = message,
                    id = Guid.NewGuid().ToString(),
                    participantId = Guid.NewGuid().ToString(),
                }
            },
            parameters = new
            {
                projectName,
                deploymentName,

                // Use Utf16CodeUnit for strings in .NET.
                stringIndexType = "Utf16CodeUnit",
            },
            kind = "Conversation",
        };

        return data;
    }

    public AnalyzedConversationResult GetJsonValues(JsonElement root)
    {
        // Extração direta dos campos necessários
        var result = root.GetProperty("result");
        var prediction = result.GetProperty("prediction");
        var topIntent = prediction.GetProperty("topIntent").GetString();
        var userMessage = result.GetProperty("query").GetString();

        // Extração do confidence score do intent principal
        var confidenceScore = prediction.GetProperty("intents")
            .EnumerateArray()
            .First(i => i.GetProperty("category").GetString() == topIntent)
            .GetProperty("confidenceScore").GetDouble();

        // Extração e conversão das entidades
        var entities = new List<TextEntity>();
        if (prediction.TryGetProperty("entities", out var entitiesElement))
        {
            foreach (var entity in entitiesElement.EnumerateArray())
            {
                entities.Add(new TextEntity(
                    text: entity.GetProperty("text").GetString()!,
                    category: entity.GetProperty("category").GetString()!,
                    confidenceScore: entity.GetProperty("confidenceScore").GetDouble()
                ));
            }
        }

        return AnalyzedConversationResult.NewInstanceWithEntities(
                userMessage: userMessage!,
                topIntentCategory: Enum.TryParse<ActionCategory>(topIntent, out ActionCategory category) ? category : ActionCategory.None,
                topIntentConfidenceScore: confidenceScore,
                entities: entities
        );
    }
}