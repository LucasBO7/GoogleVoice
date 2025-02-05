using GoogleVoice.Entities;

namespace GoogleVoice.Services.Interfaces;

public interface IConversationService
{
    Task<AnalyzedConversationResult>? AnalyzeConversationAsync(string message);
}