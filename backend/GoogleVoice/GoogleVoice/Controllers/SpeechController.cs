using GoogleVoice.Services;
using GoogleVoice.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace GoogleVoice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeechController : ControllerBase
{
    private readonly ISpeechServices _speechServices;

    public SpeechController(ISpeechServices speechServices)
    {
        _speechServices = speechServices;
    }

    [HttpPost("RecognizeSpeechFromFile")]
    public async Task<IActionResult> RecognizeSpeechAsync(IFormFile formFile)
    {
        try
        {
            string recognizedText = await _speechServices.UploadAndConvertAndRecognizeSpeechAsync(formFile);
            return Ok(recognizedText);
        }
        catch (Exception ex)
        {
            return BadRequest("Houve um erro ao executar a requisição: " + ex.Message);
        }
    }

    [HttpPost("SpeechText")]
    public async Task<IActionResult> SpeechTextAsync(string textToSpeech)
    {
        try
        {
            string textSpeeched = await _speechServices.SpeechTextAsync(textToSpeech);
            return Ok(textSpeeched);
        }
        catch (Exception)
        {

            throw;
        }
    }


}