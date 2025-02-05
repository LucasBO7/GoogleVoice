using GoogleVoice.Services.Interfaces;

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


}