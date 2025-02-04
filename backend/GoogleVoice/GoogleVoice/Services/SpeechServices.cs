using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using GoogleVoice.Services.Interfaces;

namespace GoogleVoice.Services;

public class SpeechServices : ISpeechServices
{
    private readonly IAudioServices _audioConverter;
    private readonly IConfiguration _configuration;
    static string? speechKey;
    static string? speechRegion;

    public SpeechServices(IConfiguration configuration, IAudioServices audioConverter)
    {
        _configuration = configuration;
        _audioConverter = audioConverter;

        speechKey = _configuration.GetValue<string>("SPEECH_KEY") ?? throw new Exception("SPEECH_KEY não foi definida.");
        speechRegion = _configuration.GetValue<string>("SPEECH_REGION") ?? throw new Exception("SPEECH_REGION não foi definida.");
    }

    public async Task<string> UploadAndConvertAndRecognizeSpeech(IFormFile file)
    {
        string fileUploaded = await _audioConverter.Upload(file);
        string wavFile = _audioConverter.ConvertToWav(fileUploaded);
        string recognizedText = await RecognizeSpeechFromFile(wavFile);
        _audioConverter.DeleteFile(fileUploaded);
        _audioConverter.DeleteFile(wavFile);
        return recognizedText;
    }

    public async Task<string> RecognizeSpeechFromFile(string fileName)
    {
        try
        {
            SpeechConfig speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechRecognitionLanguage = "pt-BR";
            using AudioConfig audioConfig = AudioConfig.FromWavFileInput(fileName);

            using SpeechRecognizer speechRecognizer = new(speechConfig, audioConfig);


            SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();

            return speechRecognitionResult.Text;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}