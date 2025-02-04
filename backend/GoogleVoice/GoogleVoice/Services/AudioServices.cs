using GoogleVoice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NAudio.Wave;

namespace GoogleVoice.Services;

public class AudioServices : IAudioServices
{
    private const string _wavExtension = ".wav";
    private readonly IWebHostEnvironment _environment;

    public AudioServices(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Arquivo inválido.");

        var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }

    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
    }


    public string ConvertToWav(string inputFile)
    {
        // Usa o diretório de uploads dentro da aplicação
        string uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads");
        string filePath = Path.Combine(uploadsFolder, Path.GetFileName(inputFile));

        // Garante que o diretório existe
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("O arquivo não foi encontrado");

        if (!Path.GetExtension(filePath).Equals(_wavExtension, StringComparison.CurrentCultureIgnoreCase))
        {
            string wavFile = Path.ChangeExtension(filePath, _wavExtension);

            using var reader = new AudioFileReader(filePath);

            WaveFileWriter.CreateWaveFile(wavFile, reader);

            return wavFile;
        }

        return filePath;
    }
}