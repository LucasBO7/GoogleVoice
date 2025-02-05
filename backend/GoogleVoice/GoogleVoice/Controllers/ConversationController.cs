using GoogleVoice.Entities;
using GoogleVoice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoogleVoice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConversationController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpPost]
    public async Task<ActionResult<AnalyzedConversationResult>> AnalyzeConversationAsync(string message)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(message))
                return BadRequest("O campo 'message' é obrigatório!");

            AnalyzedConversationResult? jsonResponse = await _conversationService.AnalyzeConversationAsync(message)!;

            if (jsonResponse == null)
                throw new Exception("Não foi possível buscar os dados do JSON de retorno do serviço da Azure.");

            return Ok(jsonResponse);
        }
        catch (Exception ex)
        {
            return BadRequest("Houve um erro! " + ex.Message);
        }
    }
}