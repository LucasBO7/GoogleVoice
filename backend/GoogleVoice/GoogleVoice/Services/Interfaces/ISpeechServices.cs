namespace GoogleVoice.Services.Interfaces;

public interface ISpeechServices
{
    Task<string> UploadAndConvertAndRecognizeSpeech(IFormFile file);
    Task<string> RecognizeSpeechFromFile(string fileName);
}