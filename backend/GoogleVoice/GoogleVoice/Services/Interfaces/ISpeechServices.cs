namespace GoogleVoice.Services.Interfaces;

public interface ISpeechServices
{
    Task<string> UploadAndConvertAndRecognizeSpeechAsync(IFormFile file);
    Task<string> RecognizeSpeechFromFileAsync(string fileName);
    Task<string> SpeechTextAsync(string textToSpeech);
}