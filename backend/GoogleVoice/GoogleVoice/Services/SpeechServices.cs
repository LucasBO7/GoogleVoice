using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using GoogleVoice.Services.Interfaces;

namespace GoogleVoice.Services;

public class SpeechServices : ISpeechServices
{
    private readonly IAudioServices _audioConverter;
    private readonly IConfiguration _configuration;
    private SpeechConfig _speechConfig;

    private readonly string? speechKey;
    private readonly string? speechRegion;

    public SpeechServices(IConfiguration configuration, IAudioServices audioConverter)
    {
        _configuration = configuration;
        _audioConverter = audioConverter;

        speechKey = _configuration.GetValue<string>("SPEECH_KEY") ?? throw new Exception("SPEECH_KEY não foi definida.");
        speechRegion = _configuration.GetValue<string>("SPEECH_REGION") ?? throw new Exception("SPEECH_REGION não foi definida.");

        _speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
        _speechConfig.SpeechRecognitionLanguage = "pt-BR";
    }

    public async Task<string> UploadAndConvertAndRecognizeSpeechAsync(IFormFile file)
    {
        string fileUploaded = await _audioConverter.Upload(file);
        string wavFile = _audioConverter.ConvertToWav(fileUploaded);

        string recognizedText = await RecognizeSpeechFromFileAsync(wavFile);

        await Task.WhenAll(
            Task.Run(() => _audioConverter.DeleteFile(fileUploaded)),
            Task.Run(() => _audioConverter.DeleteFile(wavFile))
        );


        return recognizedText;
    }

    public async Task<string> RecognizeSpeechFromFileAsync(string fileName)
    {
        using AudioConfig audioConfig = AudioConfig.FromWavFileInput(fileName);

        using SpeechRecognizer speechRecognizer = new(_speechConfig, audioConfig);


        SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();

        return speechRecognitionResult.Text;
    }

    public async Task<string> SpeechTextAsync(string textToSpeech)
    {
        using SpeechSynthesizer speechSynthesizer = new(_speechConfig);

        SpeechSynthesisResult result = await speechSynthesizer.SpeakTextAsync(textToSpeech);
        string messageSpeeched = OutputSpeechSynthesizerResult(result, textToSpeech);
        return messageSpeeched;
    }

    private static string OutputSpeechSynthesizerResult(SpeechSynthesisResult result, string textSpeeched)
    {
        switch (result.Reason)
        {
            case ResultReason.SynthesizingAudioCompleted:
                return textSpeeched;
            case ResultReason.Canceled:
                return "CANCELADO!!";
            default:
                return "NENHUM TRATAMENTO PRO RESULTADO";
        }
    }
}