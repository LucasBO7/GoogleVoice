namespace GoogleVoice.Services.Interfaces;

public interface IAudioServices
{
    Task<string> Upload(IFormFile file);
    void DeleteFile(string filePath);
    string ConvertToWav(string inputFile);
}